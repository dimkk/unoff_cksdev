using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EnvDTE;
using Microsoft.VisualStudio.SharePoint;
using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Common;
using CKS.Dev.WCT.Resources;
using CKS.Dev.WCT.SolutionModel;
using CKS.Dev.WCT.Framework.IO;

namespace CKS.Dev.WCT.Mappers
{

    public class VisualStudioProjectMapper
    {
        public WCTContext WCTContext { get; set; }


        public VisualStudioProjectMapper(WCTContext context)
        {
            this.WCTContext = context;
        }

        public void Map()
        {
            this.WCTContext.SharePointProject.IsSandboxedSolution = this.WCTContext.SourceProject.IsSandboxedSolution;
            this.WCTContext.SharePointProject.SiteUrl = new Uri(this.WCTContext.SourceProject.SiteURL);
            this.WCTContext.SharePointProject.Package.Model.SolutionId = new Guid(this.WCTContext.Solution.SolutionId);

            if (!String.IsNullOrEmpty(this.WCTContext.SourceProject.AssemblyFileName))
            {
                this.WCTContext.DteProject.Properties.Item("AssemblyName").Value = this.WCTContext.SourceProject.AssemblyFileName;
            }

            if (!String.IsNullOrEmpty(this.WCTContext.SourceProject.RootNamespace))
            {
                this.WCTContext.DteProject.Properties.Item("RootNamespace").Value = this.WCTContext.SourceProject.RootNamespace;
            }

            foreach (ReferenceInformation reference in this.WCTContext.SourceProject.References)
            {
                this.WCTContext.ProjectReferences.Add(new ProjectReference(
                    reference.Include,
                    reference.IsCopyLocal,
                    reference.IsProjectReference));
            }

            this.AddStrongNameKey();

            this.AddProjectReferences();

            this.AddProjectBuildConfigurations();
        }


        public void MapProjectFiles()
        {

            foreach (KeyValuePair<string, ProjectFile> entry in this.WCTContext.SourceProject.Files)
            {
                ProjectFile file = entry.Value;
                if (!file.Referenced && !file.Used && file.IsInProjDocument)
                {
                    MapProjectFile(file);
                }
            }
        }

        private void MapProjectFile(ProjectFile file)
        {
            string fileRelativePath;
            string targetFilePath;
            string targetFolderPath;

            if (string.IsNullOrEmpty(file.LinkPath))
            {
                fileRelativePath = file.Info.FullName.Replace(WCTContext.SourceProject.Folder + @"\", string.Empty);
            }
            else
            {
                fileRelativePath = file.LinkPath.Replace(WCTContext.SourceProject.Folder + @"\", string.Empty);
            }

            targetFilePath = Path.Combine(this.WCTContext.TargetProjectFolder, fileRelativePath);
            targetFolderPath = Path.GetDirectoryName(targetFilePath);

            if (fileRelativePath.StartsWithIgnoreCase(CKS.Dev.WCT.Common.Constants.SourcePackageFolderPath + @"\"))
            {
                return;
            }

            if (file.Info.Name.EndsWithIgnoreCase("solutionid.txt"))
            {
                return;
            }

            if (file.Info.Name.EndsWithIgnoreCase("manifest.config"))
            {
                return;
            }

            if (fileRelativePath.StartsWithIgnoreCase(WCTContext.SharePointRootName))
            {
                return;
            }

            try
            {
                FileSystem.Copy(file.Info.FullName, targetFilePath, true);

                this.WCTContext.DteProject.ProjectItems.AddFromFile(targetFilePath);

                file.Used = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(
                    StringResources.Strings_Errors_MigrateProjectItem,
                    file.Info.FullName,
                    ex.ToString()));
            }
        }

        private void AddStrongNameKey()
        {
            if (!String.IsNullOrEmpty(this.WCTContext.SourceProject.AssemblyOriginatorKeyFile))
            {
                string destinationFileNamePath = Path.Combine(Path.GetDirectoryName(this.WCTContext.TargetProjectFilePath), this.WCTContext.SourceProject.AssemblyOriginatorKeyFile);

                try
                {
                    //this.WCTContext.DteProject.ProjectItems.AddFromFile(destinationFileNamePath);
                    this.WCTContext.DteProject.Properties.Item("SignAssembly").Value = true;
                    this.WCTContext.DteProject.Properties.Item("AssemblyOriginatorKeyFile").Value = destinationFileNamePath;
                }
                catch (Exception ex)
                {
                    Logger.LogError(String.Format(StringResources.Strings_Errors_AddSNKFile, destinationFileNamePath, ex.ToString()));
                }
            }
        }

        private void AddProjectBuildConfigurations()
        {
            if (this.WCTContext.SourceProject.BuildConfigurations == null)
            {
                return;
            }

            VSLangProj.VSProject vsProj = (VSLangProj.VSProject)this.WCTContext.DteProject.Object;

            try
            {
                ConfigurationManager configManager = vsProj.Project.ConfigurationManager;

                foreach (ProjectBuildConfiguration buildConfiguration in this.WCTContext.SourceProject.BuildConfigurations)
                {
                    if (buildConfiguration.ConfigurationRowName == "Debug" || buildConfiguration.ConfigurationRowName == "Release")
                    {
                        Configurations buildCondition = configManager.ConfigurationRow(buildConfiguration.ConfigurationRowName);
                        foreach (KeyValuePair<string, string> property in buildConfiguration.Properties)
                        {
                            buildCondition.Item(1).Properties.Item(property.Key).Value = property.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(StringResources.Strings_Errors_CorruptBuildConfiguration, ex.ToString()));
            }
        }

        private void AddProjectReferences()
        {
            if (this.WCTContext.ProjectReferences == null)
            {
                return;
            }

            VSLangProj.VSProject vsProj = (VSLangProj.VSProject)this.WCTContext.DteProject.Object;

            foreach (ProjectReference reference in this.WCTContext.ProjectReferences)
            {
                try
                {
                    //if (reference.IsProjectReference)
                    //{
                    //    Logger.LogInformation(String.Format(StringResources.Strings_Errors_AddProjectReference, reference.ReferenceName));

                    //    continue;
                    //}

                    //this.CleanupReference(reference);

                    VSLangProj.Reference vsRef = vsProj.References.Add(reference.ReferenceName);
                    vsRef.CopyLocal = reference.IsCopyLocal;
                }
                catch (Exception ex)
                {
                    Logger.LogError(String.Format(StringResources.Strings_Errors_AddReference, reference.ReferenceName, ex.ToString()));
                }
            }
        }

        private void CleanupReference(ProjectReference reference)
        {
            string referenceName = reference.ReferenceName;
            referenceName = this.UpdateProjectReference(
                referenceName,
                "Microsoft.SharePoint",
                "12.0.0.0",
                "14.0.0.0");

            referenceName = this.UpdateProjectReference(
                referenceName,
                "Microsoft.Office",
                "12.0.0.0",
                "14.0.0.0");

            if (!reference.ReferenceName.Equals(referenceName))
            {
                Logger.LogInformation(String.Format(StringResources.String_LogMessages_UpdateReferenceVersion, reference.ReferenceName, referenceName));

                reference.ReferenceName = referenceName;
            }
        }

        private string UpdateProjectReference(string referenceName, string assemblyNamePrefix, string oldAssemblyVersion, string newAssemblyVersion)
        {
            string cleanReferenceName = referenceName;

            Regex referenceRegex = new Regex(
                String.Format(@"{0}(\..+?\b)*\s*,\s*Version\s*=\s*{1}\b", assemblyNamePrefix, oldAssemblyVersion),
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            if (referenceRegex.IsMatch(referenceName))
            {
                cleanReferenceName = referenceName.Replace(oldAssemblyVersion, newAssemblyVersion);
            }

            return cleanReferenceName;
        }
    }
}
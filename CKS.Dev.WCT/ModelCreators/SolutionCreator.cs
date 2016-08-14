
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using CKS.Dev.WCT.Common;
using System.Text.RegularExpressions;
using CKS.Dev.WCT.Resources;
using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Framework.IO;
using CKS.Dev.WCT.Extensions;
using CKS.Dev.WCT.SolutionModel;

namespace CKS.Dev.WCT.ModelCreators
{
    public class SolutionCreator
    {
        #region Properties 

        public WCTContext Context { get; set; }

        


        #endregion

        
        public SolutionCreator (WCTContext context)
        {
            this.Context = context;
        }


        public SolutionDefinition LoadSolution()
        {
            SolutionDefinition solution = new SolutionDefinition();


            if (this.Context.SourceProject.WebReferences.Count > 0)
            {
                Logger.LogWarning(StringResources.Strings_Errors_AddWebReference);
            }

            if (this.Context.SourceProject.ServiceReferences.Count > 0)
            {
                Logger.LogWarning(StringResources.Strings_Errors_AddServiceReference);
            }


            solution.SolutionId = File.ReadAllText(Path.Combine(this.Context.SourceProjectPath, Constants.SOLUTIONID_FILENAME));
            solution.Name = this.Context.SourceProject.Name;

            if (this.Context.SourceSharePointRootExist)
            {
                this.AddArtifacts(solution);
            }

            string folder80Path = Path.Combine(this.Context.SourceProjectPath, "80");
            if(Directory.Exists(folder80Path))
            {
                DirectoryInfo dir = new DirectoryInfo(folder80Path);
                this.Add80Resources(solution, dir);
            }
            


            return solution;
        }

        private void Add80Resources(SolutionDefinition solution, DirectoryInfo currentDir)
        {
            solution.ApplicationResourceFiles = new ApplicationResourceFileDefinitions();

            foreach (DirectoryInfo childDir in currentDir.GetDirectories(this.Context))
            {
                switch(childDir.Name.ToLower())
                {
                    case "app_globalresources" : AddApp_GlobalResources(solution, childDir); break;
                    case "wpresources": AddApplicationResources(solution, childDir); break;
                    case "wpcatalog": AddWPCatalog(solution, childDir); break;
                    // Bin
                }
            }
            
            

        }


        private void AddApp_GlobalResources(SolutionDefinition solution, DirectoryInfo dir)
        {
            List<App_GlobalResourceFileDefinition> resourceList = new List<App_GlobalResourceFileDefinition>();
            VSAppGlobalResourcesItem vsItem = new VSAppGlobalResourcesItem();

            IEnumerable<FileInfo> files = dir.GetFiles(this.Context, "*", SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                ProjectFile prjFile = this.Context.SourceProject.Files[file.FullName];
                if (prjFile != null && prjFile.IsInProjDocument)
                {
                    App_GlobalResourceFileDefinition def = new App_GlobalResourceFileDefinition();
                    def.Location = file.GetLocalName(dir.FullName);
                    resourceList.Add(def);
                    prjFile.LocalName = def.Location;

                    vsItem.AddProjectFile(prjFile);
                }
            }

            if (vsItem.Files.Count > 0)
            {
                solution.ApplicationResourceFiles.VSGlobalItem = vsItem;
                solution.ApplicationResourceFiles.Items = solution.ApplicationResourceFiles.Items.AddRange(resourceList.ToArray());
            }            
        }

        private void AddApplicationResources(SolutionDefinition solution, DirectoryInfo dir)
        {
            List<ApplicationResourceFileDefinition> resourceList = new List<ApplicationResourceFileDefinition>();

            VSApplicationResourcesItem vsItem = new VSApplicationResourcesItem();

            IEnumerable<FileInfo> files = dir.GetFiles(this.Context, "*", SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                ProjectFile prjFile = this.Context.SourceProject.Files[file.FullName];
                if (prjFile != null && prjFile.IsInProjDocument)
                {
                    ApplicationResourceFileDefinition def = new ApplicationResourceFileDefinition();
                    def.Location = file.GetLocalName(dir.FullName);
                    resourceList.Add(def);
                    prjFile.LocalName = def.Location;

                    vsItem.AddProjectFile(prjFile);
                }
            }

            if (vsItem.Files.Count > 0)
            {
                solution.ApplicationResourceFiles.VSApplicationItem = vsItem;
                solution.ApplicationResourceFiles.Items = solution.ApplicationResourceFiles.Items.AddRange(resourceList.ToArray());
            }
        }


        private void AddWPCatalog(SolutionDefinition solution, DirectoryInfo dir)
        {
            List<DwpFileDefinition> list = new List<DwpFileDefinition>();

            VSWPCatalogItem vsItem = new VSWPCatalogItem();

            IEnumerable<FileInfo> files = dir.GetFiles(this.Context, "*", SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                ProjectFile prjFile = this.Context.SourceProject.Files[file.FullName];
                if (prjFile != null && prjFile.IsInProjDocument)
                {
                    DwpFileDefinition def = new DwpFileDefinition();
                    def.Location = file.GetLocalName(dir.FullName);
                    list.Add(def);

                    vsItem.AddProjectFile(this.Context, file.FullName, def.Location);
                }
            }

            if (vsItem.Files.Count > 0)
            {
                solution.VSWPCatalogItem = vsItem;
                solution.DwpFiles = list.ToArray();
            }
        }


        private void AddArtifacts(SolutionDefinition solution)
        {
            Logger.LogVerbose(StringResources.Strings_LogModel_ParseTemplateAndRootFiles);

            DirectoryInfo currentDir = new DirectoryInfo(this.Context.SourceSharePointRootPath);
            AddFiles(solution, currentDir, true);

            foreach (DirectoryInfo childDir in currentDir.GetDirectories(this.Context))
            {
                if ("Template".Equals(childDir.Name, StringComparison.OrdinalIgnoreCase))
                {
                    AddTemplateFiles(solution, childDir, false);
                }
                else
                {
                    AddHiveFiles(solution, childDir, true);
                }
            }

        }


        private void AddTemplateFiles(SolutionDefinition solution, DirectoryInfo currentDir, bool isRoot)
        {
            AddFiles(solution, currentDir, isRoot);

            foreach (DirectoryInfo childDir in currentDir.GetDirectories(this.Context))
            {
                switch (childDir.Name.ToLower())
                {
                    case "features": AddFeatures(solution, childDir); break;
                    case "sitetemplates": AddSiteTemplates(solution, childDir); break;
                    default: AddHiveFiles(solution, childDir, isRoot); break;
                }
            }
        }

        private void AddSiteTemplates(SolutionDefinition solution, DirectoryInfo childDir)
        {
            SiteTemplatesCreator creator = new SiteTemplatesCreator(this.Context, solution, childDir);
            creator.AddTemplates();
        }

        private void AddFeatures(SolutionDefinition solution, DirectoryInfo currentDir)
        {
            foreach (DirectoryInfo featureDir in currentDir.GetDirectories(this.Context))
            {
                AddFeature(solution, featureDir);
            }            
        }

        private void AddFeature(SolutionDefinition solution, DirectoryInfo featureDir)
        {
            string featurePath = Path.Combine(featureDir.FullName, "feature.xml");
            if (File.Exists(featurePath))
            {
                FeatureDefinition feature = FileSystem.Load<FeatureDefinition>(featurePath);
                feature.Name = featureDir.Name;
                feature.SourceFileInfo = new FileInfo(featurePath);

                AddFeatureReceiverClass(feature);

                AddFeatureElements(feature, featureDir);

                AddUnreferencedFilesItem(feature);

                BindUpVSItems(feature);

                solution.FeatureList.Add(feature);

            }
        }

        private void AddFeatureReceiverClass(FeatureDefinition feature)
        {
            feature.Classes = this.Context.SourceProject.Classes.GetValue(feature.ReceiverClass);
            if (feature.Classes != null)
            {
                foreach (ClassInformation classInfo in feature.Classes)
                {
                    classInfo.File.Referenced = true;
                }
            }
        }

        private void AddFeatureElements(FeatureDefinition feature, DirectoryInfo featureDir)
        {

            IEnumerable<FileInfo> xmlFiles = featureDir.GetFiles(this.Context, "*", SearchOption.AllDirectories);
            foreach (FileInfo info in xmlFiles)
            {
                if ("feature.xml".Equals(info.Name, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                ElementManifestReference element = new ElementManifestReference();
                element.Location = GetRelativeLocation(featureDir, info.FullName);
                element.SourceFileInfo = info;

                bool added = false;
                if (".xml".Equals(info.Extension, StringComparison.OrdinalIgnoreCase))
                {

                    try
                    {
                        if (SolutionFileChecker.IsElementManifest(info.FullName))
                        {
                            feature.ElementManifestList.Add(element);

                            FeatureCreator creator = new FeatureCreator(this.Context);
                            creator.AddVSSharePointItems(feature, info);

                            added = true;
                        }
                    }
                    catch(Exception ex)
                    {
                        Logger.LogError(String.Format(
                                            "{0} - {1}",
                                            "Method AddFeatureElements",
                                            ex.ToString()));

                    }
                }

                if(!added)
                {
                    feature.ElementFileList.Add(element);
                }
            }
        }





        private void BindUpVSItems(FeatureDefinition feature)
        {
            VSSharePointItemCollection list = new VSSharePointItemCollection(feature.SharePointItems);

            // Bind ListInstances up to ListTemplates
            foreach (VSListInstanceItem instance in feature.SharePointItems.OfType<VSListInstanceItem>())
            {
                if (instance.ListInstance.TemplateTypeSpecified)
                {
                    foreach (VSListDefinitionItem template in feature.SharePointItems.OfType<VSListDefinitionItem>())
                    {
                        if (template.ListTemplate.TypeSpecified)
                        {
                            if (instance.ListInstance.TemplateType == template.ListTemplate.Type)
                            {
                                template.VSItems.Add(instance);
                                list.Remove(instance);
                            }
                        }
                    }
                }
            }

            feature.SharePointItems = list;
        }



        private void AddUnreferencedFilesItem(FeatureDefinition feature)
        {
            VSGenericItem vsItem = new VSGenericItem(feature);
            vsItem.Name = feature.Title + " Files";

            foreach (ElementManifestReference fileRef in feature.ElementFileList)
            {
                ProjectFile prjFile = this.Context.SourceProject.Files.GetValue(fileRef.SourceFileInfo.FullName);
                if (prjFile != null && !prjFile.Referenced)
                {
                    vsItem.AddProjectFile(prjFile);
                }
            }

            if (vsItem.Files.Count > 0)
            {
                feature.SharePointItems.Add(vsItem);
            }
        }

        private void AddHiveFiles(SolutionDefinition solution, DirectoryInfo currentDir, bool isRoot)
        {
            AddFiles(solution, currentDir, isRoot);

            foreach (DirectoryInfo childDir in currentDir.GetDirectories(this.Context))
            {
                AddHiveFiles(solution, childDir, isRoot);
            }

        }

        private void AddFiles(SolutionDefinition solution, DirectoryInfo currentDir, bool isRoot)
        {
            foreach (FileInfo file in currentDir.GetFiles(this.Context))
            {
                string projectRelativePath = this.Context.SourceProject.GetRelativePath(file.FullName);

                Logger.LogVerbose(String.Format(
                    StringResources.Strings_LogModel_ParseTemplateFile,
                    projectRelativePath));


                if (isRoot)
                {
                    RootFileReference template = new RootFileReference();
                    template.Location = GetServerRelativeLocation(file.FullName);
                    template.SourceFileInfo = file;
                    solution.RootFileList.Add(template);
                }
                else
                {
                    TemplateFileReference template = new TemplateFileReference();
                    template.Location = GetServerRelativeLocation(file.FullName);
                    template.SourceFileInfo = file;
                    solution.TemplateFileList.Add(template);
                }
            }
        }


        
        private string GetRelativeLocation(DirectoryInfo currentDir, string fullPath)
        {
            string relPath = fullPath;

            relPath = relPath.Replace(currentDir.FullName + Path.DirectorySeparatorChar, "");
            relPath = relPath.Replace(currentDir.FullName, "");

            return relPath;

        }

        private string GetServerRelativeLocation(string fullPath)
        {
            string relPath = fullPath;

            relPath = relPath.Replace(this.Context.SourceSharePointRootPath+ Path.DirectorySeparatorChar, "");
            relPath = relPath.Replace(this.Context.SourceSharePointRootPath, "");

            return relPath;

        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.SharePoint;
using CKS.Dev.WCT.Common;
using CKS.Dev.WCT.Resources;
using CKS.Dev.WCT.SolutionModel;
using CKS.Dev.WCT.ModelCreators;
using Microsoft.VisualStudio.SharePoint.Features;
using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Extensions;
using CKS.Dev.WCT.Framework.IO;
using EnvDTE;

namespace CKS.Dev.WCT.Mappers
{

    class FeatureMapper
    {

        private IList<string> _associatedFiles = new List<string>();

        private IList<bool> _featureResults;

        private IList<XmlNode> PropertyNodes;

        public bool SplitOccured { get; set; }

        public WCTContext WCTContext { get; set; }


        public FeatureMapper(WCTContext context)
        {
            this.WCTContext = context;
        }


        public void Map()
        {

            foreach (FeatureDefinition feature in this.WCTContext.Solution.FeatureList)
            {
                try
                {
                    this.MapFeature(feature);
                }
                catch (Exception ex)
                {
                    Logger.LogError(String.Format(StringResources.Strings_Errors_MapFeature, feature.Name, ex.ToString()));
                }
            }
        }

        private void MapFeature(FeatureDefinition featureDef)
        {
            Logger.LogStatus(String.Format(StringResources.String_LogMessages_ImportingFeature, "Feature", featureDef.Name));

            ISharePointProjectFeature spFeature = CreateSPF(featureDef);

            VSItemMapper itemMapper = new VSItemMapper(this.WCTContext);

            itemMapper.CreateItems(featureDef, spFeature);

        }

        public ISharePointProjectFeature CreateSPF(FeatureDefinition featureDefinition)
        {
            ISharePointProjectFeature vsFeature = null;

            try
            {
                if (!string.IsNullOrEmpty(featureDefinition.Name))
                {
                    // Create the feature

                    // Create a safe name for the feature creation. It seems that there is a problem with to many . (dots)
                    string safename = featureDefinition.Name.Replace(".", "_");

                    vsFeature = this.WCTContext.SharePointProject.Features.Add(safename);

                    // Change the name back to the original from the safename, if it has changed
                    if (!safename.Equals(featureDefinition.Name))
                    {
                        // Correct the name
                        ProjectItem folder = this.WCTContext.DteProject.ProjectItems.FindFolder(vsFeature.FullPath);
                        folder.Name = featureDefinition.Name;
                    }
                }
                else
                {
                    vsFeature = this.WCTContext.SharePointProject.Features.Add();
                }


                Setup(featureDefinition, vsFeature.Model);
                
                if (featureDefinition.Classes != null)
                {
                    foreach (ClassInformation classInfo in featureDefinition.Classes)
                    {
                        ProjectFile sourceFile = classInfo.File;
                        if (!sourceFile.Used)
                        {
                            string targetPath = Path.Combine(vsFeature.FullPath, sourceFile.Info.Name);

                            ProjectItem featureItem = this.WCTContext.DteProject.ProjectItems.FindFile(vsFeature.FeatureFile.FullPath);
                            if (featureItem != null)
                            {
                                FileSystem.Copy(sourceFile.Info.FullName, targetPath);
                                featureItem.ProjectItems.AddFromFile(targetPath);
                                sourceFile.Used = true;
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(
                                    StringResources.Strings_Errors_CreateFeature,
                                    featureDefinition.Name,
                                    ex.ToString()));
            }

            return vsFeature;
        }

    
        public void Setup(FeatureDefinition featureDef, IFeature vsFeature)
        {
            if (featureDef.ActivateOnDefaultSpecified)
            {
                vsFeature.ActivateOnDefault = TRUEFALSEExtensions.IsTrue(featureDef.ActivateOnDefault);
            }

            foreach (FeatureActivationDependencyDefinition def in featureDef.ActivationDependencies.AsSafeEnumable())
            {
                ICustomFeatureActivationDependency dep = vsFeature.ActivationDependencies.AddCustomFeatureActivationDependency();
                dep.FeatureDescription = def.FeatureDescription;
                if (!String.IsNullOrEmpty(def.SolutionId))
                {
                    dep.FeatureId = new Guid(def.FeatureId);
                }
                dep.FeatureTitle = def.FeatureTitle;
                dep.MinimumVersion = new System.Version(def.MinimumVersion);
                dep.SolutionId = dep.SolutionId;
                dep.SolutionName = dep.SolutionName;
                dep.SolutionTitle = dep.SolutionTitle;
                dep.SolutionUrl = dep.SolutionUrl;
            }

            if (featureDef.AlwaysForceInstallSpecified)
            {
                vsFeature.AlwaysForceInstall = TRUEFALSEExtensions.IsTrue(featureDef.AlwaysForceInstall);
            }

            if (featureDef.AutoActivateInCentralAdminSpecified)
            {
                vsFeature.AutoActivateInCentralAdmin = TRUEFALSEExtensions.IsTrue(featureDef.AutoActivateInCentralAdmin);
            }

            vsFeature.Creator = featureDef.Creator;
            vsFeature.DefaultResourceFile = featureDef.DefaultResourceFile;
            
            //vsFeature.DeploymentPath = "$SharePoint.Feature.FileNameWithoutExtension$";

            vsFeature.Description = featureDef.Description;
            vsFeature.FeatureId = new Guid(featureDef.Id);

            vsFeature.ImageAltText = featureDef.ImageUrlAltText;
            if (!String.IsNullOrEmpty(featureDef.ImageUrl))
            {
                vsFeature.ImageUrl = new Uri(featureDef.ImageUrl, UriKind.RelativeOrAbsolute);
            }

            if (featureDef.HiddenSpecified)
            {
                vsFeature.IsHidden = TRUEFALSEExtensions.IsTrue(featureDef.Hidden);
            }

            foreach (FeaturePropertyDefinition def in featureDef.Properties.AsSafeEnumable())
            {
                IProperty prop = vsFeature.Properties.Add();
                prop.Key = def.Key;
                prop.Value = def.Value;
            }

            vsFeature.ReceiverAssembly = featureDef.ReceiverAssembly;
            vsFeature.ReceiverClass = featureDef.ReceiverClass;

            if (featureDef.RequireResourcesSpecified)
            {
                vsFeature.RequireResources = TRUEFALSEExtensions.IsTrue(featureDef.RequireResources);
            }


            Microsoft.VisualStudio.SharePoint.Features.FeatureScope featureScope = Microsoft.VisualStudio.SharePoint.Features.FeatureScope.Site;
            if (Enum.TryParse(featureDef.Scope.ToString(), out featureScope))
            {
                vsFeature.Scope = featureScope;
            }

            if (!String.IsNullOrEmpty(featureDef.SolutionId))
            {
                vsFeature.SolutionId = new Guid(featureDef.SolutionId);
            }

            vsFeature.Title = featureDef.Title;

            vsFeature.UIVersion = featureDef.UIVersion;

            if (featureDef.UpgradeActions != null)
            {
                featureDef.UpgradeActions.Setup(vsFeature);
            }
        }


        private string GetUniqueProjectPath(string projectRoot, string baseRelativePath, bool alwaysUseNumberSuffix)
        {
            int i = alwaysUseNumberSuffix ? 1 : 0;
            string relativePath = alwaysUseNumberSuffix ? baseRelativePath + i : baseRelativePath;
            string folder = Path.Combine(projectRoot, relativePath);
            while (Directory.Exists(folder))
            {
                i++;
                relativePath = baseRelativePath + i;
                folder = Path.Combine(projectRoot, relativePath);
            }

            return relativePath;
        }

        private string GetAbsolutePath(string relativePath)
        {
            string projectRootFolderPath = Path.GetDirectoryName(this.WCTContext.DteProject.FileName);
            string classRelativePath = relativePath;
            if (classRelativePath.StartsWith(@"\"))
            {
                classRelativePath = classRelativePath.Substring(1);
            }

            return Path.Combine(projectRootFolderPath, classRelativePath);
        }

        private string GetFullClassName(string fullClassName)
        {
            if (!this.WCTContext.SourceProject.IsCSharp
                && !string.IsNullOrEmpty(this.WCTContext.SourceProject.RootNamespace))
            {
                fullClassName = String.Format("{0}.{1}", this.WCTContext.SourceProject.RootNamespace, fullClassName);
            }

            return fullClassName;
        }
    }
}
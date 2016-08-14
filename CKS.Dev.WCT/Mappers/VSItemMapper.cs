using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.SolutionModel;
using Microsoft.VisualStudio.SharePoint;
using System.IO;
using EnvDTE;
using CKS.Dev.WCT.Framework.IO;
using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Framework.Serialization;

namespace CKS.Dev.WCT.Mappers
{
    public class VSItemMapper
    {

        public WCTContext WCTContext { get; set; }


        public VSItemMapper(WCTContext context)
        {
            this.WCTContext = context;
        }




        public void CreateItems(FeatureDefinition featureDef, ISharePointProjectFeature spFeature)
        {
            CreateItems(null, featureDef.SharePointItems, spFeature);
        }

        public void CreateItem(VSSharePointItem vsItem)
        {
            if (vsItem == null)
            {
                return;
            }

            List<VSSharePointItem> list = new List<VSSharePointItem>();
            list.Add(vsItem);
            CreateItems(null, list, null);
        }


        private void CreateItems(string subFolder, IList<VSSharePointItem> vsItems, ISharePointProjectFeature spFeature)
        {
            if (vsItems != null)
            {
                foreach (VSSharePointItem vsItem in vsItems)
                {
                    string currentFolder = subFolder;
                    if (String.IsNullOrEmpty(currentFolder))
                    {
                        currentFolder = @"Items\" + vsItem.GroupName;
                    }

                    ISharePointProjectItem spItem = CreateSPI(currentFolder, vsItem);
                    if (spFeature != null)
                    {
                        if (vsItem is VSWebPartItem && spFeature.Model.Scope != Microsoft.VisualStudio.SharePoint.Features.FeatureScope.Site)
                        {
                            // Ensure that the Feature is set to Scope = Site, if it contains a WebPart Item.
                            spFeature.Model.Scope = Microsoft.VisualStudio.SharePoint.Features.FeatureScope.Site;
                        }

                        spFeature.ProjectItems.Add(spItem);
                    }

                    string childFolder = currentFolder + @"\" + spItem.Name;
                    CreateItems(childFolder, vsItem.VSItems, spFeature);
                }
            }
        }


        private ISharePointProjectItem CreateSPI(string subFolder, VSSharePointItem vsItem)
        {
            string spiName = GetSPIName(vsItem.Name);

            // Create a new Visual Studio SharePoint Item
            ISharePointProjectItem projectItem = this.WCTContext.SharePointProject.ProjectItems.Add(subFolder, spiName, vsItem.TypeName, true);
            

          
            // Copy all resource files into the vsItem before creating an elements.xml file.
            foreach (ProjectFile file in vsItem.Files)
            {
                if (!file.Used && File.Exists(file.Info.FullName))
                {
                    string target = Path.Combine(projectItem.FullPath, file.LocalName);
                    FileSystem.Copy(file.Info.FullName, target);
                    ISharePointProjectItemFile spiFile = projectItem.Files.AddFromFile(target);
                    spiFile.DeploymentType = file.SPDeploymentType;
                    file.Used = true;
                }
            }

            if (vsItem is VSWebPartItem)
            {
                VSWebPartItem webpartItem = (VSWebPartItem)vsItem;
                foreach (ClassInformation classInfo in webpartItem.Classes)
                {
                    ISafeControlEntry entry = null;
                    foreach (ISafeControlEntry item in projectItem.SafeControlEntries)
                    {
                        if (classInfo.NameSpace.EqualsIgnoreCase(item.NamespaceName))
                        {
                            entry = item;
                            break;
                        }
                    }

                    if (entry == null)
                    {
                        entry = projectItem.SafeControlEntries.Add(classInfo.NameSpace, "*", "$SharePoint.Project.AssemblyFullName$", true);
                        entry.Name = classInfo.FullClassNameWithoutAssembly;
                    }
                }
            }

            if (vsItem is VSFeatureItem)
            {
                // Create the elements.xml file as the last file, to ensure that Visual Studio do not update it.
                VSFeatureItem vsFeatureItem = (VSFeatureItem)vsItem;
                ElementDefinitionCollection elements = vsFeatureItem.GetElementManifest();
                if (elements != null)
                {
                    // Add the Elements.xml file
                    String xml = Serializer.ObjectToXML(elements);
                    string manifestFilename = Path.Combine(projectItem.FullPath, "Elements.xml");
                    File.WriteAllText(manifestFilename, xml);
                    ISharePointProjectItemFile spiElementFile = projectItem.Files.AddFromFile(manifestFilename);
                    spiElementFile.DeploymentType = DeploymentType.ElementManifest;
                }
            }

            return projectItem;
        }

        /// <summary>
        /// Gets the SPI name. Checks if the name has already been used and then adds a number to the end of the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetSPIName(string name)
        {
            string result = name;
            int count = 2; // Start from number two, because the first SPI is without number

            if (this.WCTContext.SPINames.ContainsKey(name))
            {
                count = this.WCTContext.SPINames[name];
                result = name + count;
                count++;
            }

            this.WCTContext.SPINames.AddOrReplace(name, count);

            return name;
        }

    }
}

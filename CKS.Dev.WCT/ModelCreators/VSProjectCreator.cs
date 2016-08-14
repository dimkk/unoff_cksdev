using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Text.RegularExpressions;

using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Common;
using CKS.Dev.WCT.Extensions;
using CKS.Dev.WCT.SolutionModel;

namespace CKS.Dev.WCT.ModelCreators
{

    public class VSProjectCreator
    {
        public WCTContext Context { get; set; }

        public VSProjectCreator(WCTContext context)
        {
            this.Context = context;
        }

        public VSProject Create(string path, bool isCSharp)
        {
            VSProject project = new VSProject();

            
            project.Name = Path.GetFileNameWithoutExtension(path);
            project.FileName = path;
            project.Folder = Path.GetDirectoryName(path);
            project.IsCSharp = isCSharp;

            LoadConfigurations(project);

            project.Files = CreateProjectFiles(project);


            return project;
        }

        private void LoadConfigurations(VSProject project)
        {
            if (project.ProjDocument == null)
            {
                return;
            }

            XmlElement xDocElement = project.ProjDocument.DocumentElement;

            string assemblyName = string.Empty;
            string rootNameSpace = string.Empty;

            CreateBuildConfigurations(project, xDocElement);

            SetOutputPath(project, xDocElement);

            XmlElement node = XmlHelper.SelectSingleElement(xDocElement, "/Project/PropertyGroup/AssemblyName");
            if (node != null)
            {
                //assemblyName = String.Format("{0}.dll", node.InnerText);
                //assemblyName = String.Format("{0}", node.InnerText);
                project.AssemblyFileName = node.InnerText;
                //project.AssemblyFileName = assemblyName;
            }

            node = XmlHelper.SelectSingleElement(xDocElement, "/Project/PropertyGroup/RootNamespace");
            if (node != null)
            {
                rootNameSpace = node.InnerText;

                project.RootNamespace = rootNameSpace;
            }

            node = XmlHelper.SelectSingleElement(xDocElement, "/Project/PropertyGroup/ProjectGuid");
            if (node != null
                && !String.IsNullOrEmpty(node.InnerText))
            {
                project.ProjectGuid = new Guid(node.InnerText);
            }

            node = XmlHelper.SelectSingleElement(xDocElement, "/Project/PropertyGroup/AssemblyOriginatorKeyFile");
            if (node != null
                && !String.IsNullOrEmpty(node.InnerText))
            {
                string assemblyOriginatorKeyFile = node.InnerText;

                project.AssemblyOriginatorKeyFile = assemblyOriginatorKeyFile;
            }

            foreach (XmlNode referenceNode in XmlHelper.SelectNodes(xDocElement, "ItemGroup/Reference", xDocElement.NamespaceURI))
            {
                project.References.Add(this.GetReferenceInformation(referenceNode));
            }

            foreach (XmlNode referenceNode in XmlHelper.SelectNodes(xDocElement, "ItemGroup/ProjectReference", xDocElement.NamespaceURI))
            {
                project.References.Add(this.GetReferenceInformation(referenceNode));
            }

            foreach (XmlNode referenceNode in XmlHelper.SelectNodes(xDocElement, "ItemGroup/WebReferences", xDocElement.NamespaceURI))
            {
                project.WebReferences.Add(referenceNode.InnerText);
            }

            foreach (XmlNode referenceNode in XmlHelper.SelectNodes(xDocElement, "ItemGroup/WCFMetadata", xDocElement.NamespaceURI))
            {
                project.ServiceReferences.Add(referenceNode.InnerText);
            }
        }

        private ProjectFileDictionary CreateProjectFiles(VSProject project)
        {
            ProjectFileDictionary result = new ProjectFileDictionary();
            DirectoryInfo dir = new DirectoryInfo(project.Folder);

            string filter = ".cs";
            if (!project.IsCSharp)
            {
                filter = ".vb";
            }


            // Add the files from the filsystem, root of the project
            IEnumerable<FileInfo> files = dir.GetFiles(this.Context, "*", SearchOption.AllDirectories);
            foreach (FileInfo info in files)
            {
                if (info.Name.EndsWithIgnoreCase(filter))
                {
                    ClassFile classFile = new ClassFile(info);
                    LoadClasses(project, classFile);
                    result.AddOrReplace(info.FullName, classFile);
                }
                else
                {
                    result.AddOrReplace(info.FullName, new ProjectFile(info));
                }
            }

            // Add the files from the Project File
            List<string> paths = GetProjectFiles(project, filter);
            foreach (string path in paths)
            {
                ProjectFile projFile = result.GetValue(path);
                if (projFile != null)
                {
                    projFile.IsInProjDocument = true;
                }
            }

            return result;
        }


        private List<string> GetProjectFiles(VSProject project, string filter)
        {
            List<string> result = new List<string>();

            if (project.ProjDocument == null)
            {
                // Return because there is no project files available!
                return result;
            }

            XmlElement xElement = project.ProjDocument.DocumentElement;

            XmlNodeList items = XmlHelper.SelectNodes(xElement, "ItemGroup/*[@Include]", xElement.NamespaceURI);

            foreach (XmlNode item in items)
            {
                if (!item.Name.Equals("Compile", StringComparison.OrdinalIgnoreCase)
                    && !item.Name.Equals("Content", StringComparison.OrdinalIgnoreCase)
                    && !item.Name.Equals("None", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    continue;
                }

                string itemRelativeLocation = item.GetAttributeValueSafe("Include");

                XmlElement linkNode = XmlHelper.SelectSingleElement(item as XmlElement, "Link", item.NamespaceURI);

                string linkLocation = string.Empty;
                if (linkNode != null && !string.IsNullOrEmpty(linkNode.InnerText))
                {
                    linkLocation = Path.Combine(project.Folder, linkNode.InnerText);
                }

                if (!string.IsNullOrEmpty(itemRelativeLocation))
                {
                    result.Add(Path.Combine(project.Folder, itemRelativeLocation));
                    //FileInfo info = new FileInfo();
                    //ProjectFile file = null;
                    //if (info.Name.EndsWithIgnoreCase(filter))
                    //{
                    //    ClassFile classFile = new ClassFile(info);
                    //    LoadClasses(project, classFile);
                    //    file = classFile;
                    //}
                    //else
                    //{
                    //    file = new ProjectFile(info);
                    //}

                    //file.LinkPath = linkLocation;
                    //file.IsInProjDocument = true;
                    //result.Add(file);
                }
            }

            return result;
        }

        private void SetOutputPath(VSProject project, XmlElement xDocElement)
        {
            XmlElement node = XmlHelper.SelectSingleElement(xDocElement, "/Project/PropertyGroup/OutputPath");
            if (node != null)
            {
                project.OutputPath = Path.Combine(project.Folder, node.InnerText);
            }
        }

        private void CreateBuildConfigurations(VSProject project, XmlElement xDocElement)
        {
            XmlNodeList buildConfigNodes = XmlHelper.SelectNodes(xDocElement, "/Project/PropertyGroup[@Condition]");
            if (buildConfigNodes != null)
            {
                project.BuildConfigurations = new List<ProjectBuildConfiguration>();

                foreach (XmlElement buildConfigNode in buildConfigNodes.OfType<XmlElement>())
                {
                    ProjectBuildConfiguration projectBuildConfiguration = new ProjectBuildConfiguration();

                    string conditionValue = buildConfigNode.GetAttribute("Condition");

                    string configRowName = Regex.Split(conditionValue, "==")[1].Split('|')[0].Replace("'", "").Trim();
                    projectBuildConfiguration.ConfigurationRowName = configRowName;
                    projectBuildConfiguration.Properties = new Dictionary<string, string>();

                    foreach (XmlElement property in buildConfigNode.ChildNodes.OfType<XmlElement>())
                    {
                        string elementName = property.Name;
                        string elementValue = property.InnerText;

                        if (elementName != "DebugType" &&
                            elementName != "StartAction" &&
                            elementName != "StartURL")
                        {
                            projectBuildConfiguration.Properties.Add(elementName, elementValue);
                        }
                    }

                    project.BuildConfigurations.Add(projectBuildConfiguration);
                }
            }
        }


        public void LoadClasses(VSProject project, ClassFile classfile)
        {
            classfile.Classes = this.GetClassInformation(classfile.Info.FullName, project);

            foreach (ClassInformation classInfo in classfile.Classes)
            {
                classInfo.File = classfile;

                project.Classes.Add(classInfo);
            }
        }

        private IList<ClassInformation> GetClassInformation(string path, VSProject project)
        {
            List<ClassInformation> classInfos = new List<ClassInformation>();

            string classContents = File.ReadAllText(path);

            string nameSpaceRegexString = @"namespace\s+((.+?)\b(\..+?\b)*)";
            string usingRegexString = @"using\s+((.+?)\b(\..+?\b)*)";
            string classNameRegexString = "\\bclass\\s+(.+?)\\b";
            string guidAttributeRegexString = "\\[Guid\\(\"(.+?)\"\\)\\]";
            if (!project.IsCSharp)
            {
                nameSpaceRegexString = @"Namespace\s+((.+?)\b(\..+?\b)*)";
                classNameRegexString = "\\bClass\\s+(.+?)\\b";
                guidAttributeRegexString = "<Guid\\(\"(.+?)\"\\)> _";
            }

            Regex usingRegex = new Regex(usingRegexString, RegexOptions.Multiline);
            Regex nameSpaceRegex = new Regex(nameSpaceRegexString, RegexOptions.Multiline);
            Regex classNameRegex = new Regex(classNameRegexString, RegexOptions.Multiline);
            Regex featureReceiverRegex = new Regex("\\bSPFeatureReceiver\\b");
            Regex guidAttributeRegex;
            if (!project.IsCSharp)
            {
                guidAttributeRegex = new Regex(guidAttributeRegexString);
            }
            else
            {
                guidAttributeRegex = new Regex(guidAttributeRegexString, RegexOptions.Multiline);
            }

            string namespaceName = string.Empty;
            Guid id = Guid.Empty;
            string className = string.Empty;
            bool isFeatureReceiver = false;

            List<string> usings = new List<string>();

            int lineCount = 0;
            //int nameSpaceStart = 0;
            //int classStart = 0;
            //int classEnd = 0;

            string[] lines = classContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                MatchCollection usingMatch = usingRegex.Matches(line);
                if (usingMatch.Count > 0 && usingMatch[0].Groups.Count > 1)
                {
                    string usingString = usingMatch[0].Groups[1].Value;
                    usings.Add(usingString);
                }


                MatchCollection nameSpaceMatch = nameSpaceRegex.Matches(line);
                if (nameSpaceMatch.Count > 0 && nameSpaceMatch[0].Groups.Count > 1)
                {
                    namespaceName = nameSpaceMatch[0].Groups[1].Value;
                }

                MatchCollection guidAttributeMatch = guidAttributeRegex.Matches(line);
                if (guidAttributeMatch.Count > 0 && guidAttributeMatch[0].Groups.Count > 1)
                {
                    try
                    {
                        id = new Guid(guidAttributeMatch[0].Groups[1].Value);
                    }
                    catch
                    {
                        id = Guid.Empty;
                    }
                }

                MatchCollection featureReceiverMatch = featureReceiverRegex.Matches(line);
                if (featureReceiverMatch.Count > 0 && featureReceiverMatch[0].Groups.Count == 1)
                {
                    isFeatureReceiver = true;
                }

                MatchCollection classNameMatch = classNameRegex.Matches(line);
                if (classNameMatch.Count > 0 && classNameMatch[0].Groups.Count > 1)
                {
                    className = classNameMatch[0].Groups[1].Value;
                    classInfos.Add(new ClassInformation(id, className, namespaceName, string.Empty, project.GetRelativePath(path), isFeatureReceiver));
                    id = Guid.Empty;
                }

                lineCount++;
            }

            return classInfos;
        }


        private ReferenceInformation GetReferenceInformation(XmlNode referenceNode)
        {
            string include = referenceNode.GetAttributeValueSafe("Include");
            bool isCopyLocal = false;
            bool isProjectReference = referenceNode.Name.Equals("ProjectReference", StringComparison.OrdinalIgnoreCase); ;

            foreach (XmlNode referenceChildNode in referenceNode.ChildNodes)
            {
                if (referenceChildNode.Name.Equals("Private", StringComparison.OrdinalIgnoreCase)
                    && !string.IsNullOrEmpty(referenceChildNode.Value)
                    && referenceChildNode.Value.Equals("True", StringComparison.OrdinalIgnoreCase))
                {
                    isCopyLocal = true;
                    break;
                }
            }

            return new ReferenceInformation(include, isCopyLocal, isProjectReference);
        }


        public bool IsVS2010SharePointProject(string source)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(source);

            return this.IsVS2010SharePointProject(xDoc);
        }

        public bool IsVS2010SharePointProject(XmlDocument xDoc)
        {
            XmlElement element = XmlHelper.SelectSingleElement(xDoc.DocumentElement, "PropertyGroup/ProjectTypeGuids", xDoc.NamespaceURI);

            bool isVSeWSSProject = false;

            if (element != null)
            {
                isVSeWSSProject = element.InnerText.Contains(Constants.VS2010SPProjectFlavorGuid);
            }

            return isVSeWSSProject;
        }
    }
}
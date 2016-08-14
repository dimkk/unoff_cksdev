using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Framework.IO;
using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Common;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public abstract class VSSharePointItem
    {
        internal static Dictionary<string, Guid> _itemNames = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);


        public Guid InternalID = Guid.NewGuid();

        private string _name = null;
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(_name) && _itemNames.ContainsKey(_name) && _itemNames[_name] == this.InternalID)
                {
                    _itemNames.Remove(_name);
                }

                _name = FileSystem.MakeSafeFilename(value, ' ');
                if (String.IsNullOrEmpty(_name))
                {
                    // Ensure that there is a name, no matter what!
                    _name = FileSystem.MakeSafeFilename(Guid.NewGuid().ToString(), ' ');
                }

                //Ensure that the name is unique!

                string baseName = _name;
                int count = 2;
                while (_itemNames.ContainsKey(_name) && _itemNames[_name] != this.InternalID)
                {
                    _name = baseName + count;
                    count++;
                }

                if (!_itemNames.ContainsKey(_name))
                {
                    _itemNames.Add(_name, this.InternalID);
                }
            }
        }

        private string _supportedTrustLevels = "All";
        public string SupportedTrustLevels
        {
            get { return _supportedTrustLevels; }
            set { _supportedTrustLevels = value; }
        }


        public string SupportedDeploymentScopes = "Web, Site, WebApplication, Farm";

        public String TypeName = Constants.SPTypeNameGenericElement;

        public string GroupName { get; set; }

        public DeploymentType DefaultDeploymentType { get; set; }


        //public List<VSSharePointItem> Children = new List<VSSharePointItem>();

        public List<ProjectFile> Files = new List<ProjectFile>();


        private VSSharePointItemCollection _vsItems = new VSSharePointItemCollection();
        public VSSharePointItemCollection VSItems
        {
            get { return _vsItems; }
            set { _vsItems = value; }
        }


        public void AddCodeFile(WCTContext wctContext, string className)
        {
            ClassInformationCollection classes = wctContext.SourceProject.Classes.GetValue(className);
            if (classes != null)
            {
                foreach (ClassInformation classInfo in classes)
                {
                    if (!classInfo.File.Referenced)
                    {
                        this.AddProjectFile(classInfo.File);
                    }
                }
            }


        }

        public ProjectFile AddProjectFile(WCTContext wctContext, string fullname)
        {
            return AddProjectFile(wctContext, fullname, null, this.DefaultDeploymentType);
        }

        public ProjectFile AddProjectFile(WCTContext wctContext, string fullname, DeploymentType deploymentType)
        {
            return AddProjectFile(wctContext, fullname, null, deploymentType);
        }

        public ProjectFile AddProjectFile(WCTContext wctContext, string fullname, string localName)
        {
            return AddProjectFile(wctContext, fullname, localName, this.DefaultDeploymentType);
        }

        public ProjectFile AddProjectFile(WCTContext wctContext, string fullname, string localName, DeploymentType deploymentType)
        {
            ProjectFile prjFile = wctContext.SourceProject.Files.GetValue(fullname);
            if (prjFile != null)
            {
                AddProjectFile(prjFile);
                prjFile.SPDeploymentType = deploymentType;
                if (!String.IsNullOrWhiteSpace(localName))
                {
                    prjFile.LocalName = localName;
                }
            }
            return prjFile;
        }

        public void AddProjectFile(ProjectFile file)
        {
            this.Files.Add(file);
            file.SPDeploymentType = this.DefaultDeploymentType;
            file.Referenced = true;
        }


        public static void Reset()
        {
            _itemNames = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
        }

    }
}

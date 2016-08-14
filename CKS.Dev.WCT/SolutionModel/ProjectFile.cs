using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CKS.Dev.WCT.SolutionModel;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public class ProjectFile
    {
        public string LinkPath { get; set; }

        private FileInfo _info = null;
        public FileInfo Info 
        {
            get
            {
                return _info;
            }
            set
            {
                _info = value;
            }
        }

        private string _localName;
        /// <summary>
        /// Defines the path of the file locally from the SharePoint Items location.
        /// Usually the path is the name or a subfolder and the name of the file.
        /// </summary>
        public string LocalName
        {
            get 
            {
                if (String.IsNullOrWhiteSpace(_localName) && this.Info != null)
                {
                    _localName = this.Info.Name;
                }
                return _localName; 
            }
            set 
            { 
                _localName = value; 
            }
        }


        public bool IsInProjDocument { get; set; }
        public bool Used { get; set; }
        public bool Referenced { get; set; }

        private DeploymentType _spDeploymentType = DeploymentType.TemplateFile;
        public DeploymentType SPDeploymentType
        {
            get { return _spDeploymentType; }
            set { _spDeploymentType = value; }
        }



        public ProjectFile()
        {
        }

        public ProjectFile(FileInfo info)
        {
            this.Info = info;
        }

        public ProjectFile(FileInfo info, string linkPath)
        {
            this.Info = info;
            this.LinkPath = linkPath;
        }

    }
}

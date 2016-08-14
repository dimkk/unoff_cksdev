using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CKS.Dev.WCT.Common;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public class ClassFile : ProjectFile
    {
        public IList<ClassInformation> Classes { get; set; }

        public ClassFile()
        {
            this.SPDeploymentType = DeploymentType.NoDeployment;
        }

        public ClassFile(FileInfo info)
        {
            this.Info = info;
            this.SPDeploymentType = DeploymentType.NoDeployment;
        }
    }
}

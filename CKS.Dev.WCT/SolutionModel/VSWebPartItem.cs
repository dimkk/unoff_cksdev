using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Common;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public class VSWebPartItem : VSModuleItem
    {

        private List<ClassInformation> _classes = new List<ClassInformation>();
        public List<ClassInformation> Classes
        {
            get { return _classes; }
            set { _classes = value; }
        }


        public VSWebPartItem(FeatureDefinition feature) 
        {
            this.Feature = feature;
            this.Name = "WebPart_" + Guid.NewGuid();
            this.TypeName = Constants.SPTypeNameWebPart;
            this.GroupName = "WebParts";
            this.SupportedDeploymentScopes = "Site";
            this.DefaultDeploymentType = DeploymentType.ElementFile;
        }



    }
}

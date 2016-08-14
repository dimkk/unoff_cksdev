using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Common;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public class VSApplicationResourcesItem : VSSharePointItem
    {

        public VSApplicationResourcesItem()
        {
            this.Name = "ApplicationResources";
            this.TypeName = Constants.SPTypeNameGenericElement;
            this.GroupName = "Resources";
            this.SupportedDeploymentScopes = "None";
            this.DefaultDeploymentType = DeploymentType.ApplicationResource;
        }

    }
}

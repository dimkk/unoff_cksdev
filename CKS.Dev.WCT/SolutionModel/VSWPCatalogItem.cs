using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Common;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public class VSWPCatalogItem : VSSharePointItem
    {

        public VSWPCatalogItem()
        {
            this.Name = "WPCatalog";
            this.TypeName = Constants.SPTypeNameGenericElement;
            this.GroupName = "Resources";
            this.SupportedDeploymentScopes = "None";
            this.DefaultDeploymentType = DeploymentType.DwpFile;
        }

    }
}

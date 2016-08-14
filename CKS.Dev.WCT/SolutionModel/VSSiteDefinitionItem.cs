using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Common;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public class VSSiteDinitionItem : VSSharePointItem
    {

        private SiteDefinitionManifestFileReference _siteDefinition = null;
        public SiteDefinitionManifestFileReference SiteDefinition
        {
            get { return _siteDefinition; }
            set
            {
                _siteDefinition = value;
                if (_siteDefinition != null && !String.IsNullOrEmpty(_siteDefinition.Location))
                {
                    this.Name = _siteDefinition.Location;
                }
            }
        }

        public VSSiteDinitionItem(SiteDefinitionManifestFileReference siteDefinition)
        {
            this.Name = "SiteDefinition_" + Guid.NewGuid();
            this.TypeName = Constants.SPTypeNameSiteDefinition;
            this.GroupName = "SiteDefinitions";
            this.SupportedDeploymentScopes = "None";
            this.SupportedTrustLevels = "All";
            this.DefaultDeploymentType = DeploymentType.TemplateFile;
            this.SiteDefinition = siteDefinition;
        }

    }
}

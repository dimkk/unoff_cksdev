using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Common;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public class VSModuleItem : VSFeatureItem
    {

        private ModuleDefinition _module = null;

        public ModuleDefinition Module
        {
            get { return _module; }
            set
            {
                _module = value;
                if (_module != null && !String.IsNullOrEmpty(_module.Name))
                {
                    this.Name = _module.Name;
                }
            }
        }

        public VSModuleItem()
        {

        }

        public VSModuleItem(FeatureDefinition feature)
        {
            this.Feature = feature;
            this.Name = "Module_" + Guid.NewGuid();
            this.TypeName = Constants.SPTypeNameModule;
            this.GroupName = "Modules";
            this.SupportedDeploymentScopes = "Web, Site";
            this.DefaultDeploymentType = DeploymentType.ElementFile;
        }

        public override ElementDefinitionCollection GetElementManifest()
        {
            ElementDefinitionCollection elements = new ElementDefinitionCollection();

            elements.Items = new object[] { this.Module };

            return elements;
        }
    }
}

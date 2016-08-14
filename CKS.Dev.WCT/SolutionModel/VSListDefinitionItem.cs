using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Common;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public class VSListDefinitionItem : VSFeatureItem
    {

        private ListTemplateDefinition _listTemplate = null;

        public ListTemplateDefinition ListTemplate
        {
            get { return _listTemplate; }
            set
            {
                _listTemplate = value;
            }
        }

        public override string Name
        {
            get
            {
                if (_listTemplate != null && !String.IsNullOrWhiteSpace(_listTemplate.DisplayName))
                {
                    base.Name = _listTemplate.DisplayName;
                }

                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }


        public VSListDefinitionItem(FeatureDefinition feature)
        {
            this.Feature = feature;
            this.Name = "ListDefinition_" + Guid.NewGuid();
            this.TypeName = Constants.SPTypeNameListDefinition;
            this.GroupName = "ListDefinitions";
            this.SupportedDeploymentScopes = "Web, Site";
            this.DefaultDeploymentType = DeploymentType.ElementFile;
        }

        public VSListDefinitionItem(FeatureDefinition feature, ListTemplateDefinition listTemplate) : this(feature)
        {
            this.ListTemplate = listTemplate;
        }

        public override ElementDefinitionCollection GetElementManifest()
        {
            ElementDefinitionCollection elements = new ElementDefinitionCollection();

            elements.Items = new object[] { this.ListTemplate};

            return elements;
        }
    }
}

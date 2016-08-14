using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Common;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public class VSListInstanceItem : VSFeatureItem
    {


        private ListInstanceDefinition _listInstance = null;

        public ListInstanceDefinition ListInstance
        {
            get { return _listInstance; }
            set
            {
                _listInstance = value;
            }
        }

        public override string Name
        {
            get
            {
                if (this.ListInstance != null && !String.IsNullOrWhiteSpace(this.ListInstance.Title))
                {
                    base.Name = this.ListInstance.Title;
                }
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        public VSListInstanceItem(FeatureDefinition feature)
        {
            this.Feature = feature;
            this.Name = "ListInstance";
            this.TypeName = Constants.SPTypeNameListInstance;
            this.GroupName = "ListInstances";
            this.SupportedDeploymentScopes = "Web, Site";
            this.DefaultDeploymentType = DeploymentType.ElementFile;
        }

        public VSListInstanceItem(FeatureDefinition feature, ListInstanceDefinition listInstance) : this(feature)
        {
            this.ListInstance = listInstance;
        }

        public override ElementDefinitionCollection GetElementManifest()
        {
            ElementDefinitionCollection elements = new ElementDefinitionCollection();

            elements.Items = new object[] { this.ListInstance};

            return elements;
        }
    }
}

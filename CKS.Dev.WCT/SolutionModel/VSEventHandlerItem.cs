using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Common;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public class VSEventHandlerItem : VSFeatureItem
    {

        private ReceiverDefinitionCollection _receivers;

        public ReceiverDefinitionCollection Receivers
        {
            get { return _receivers; }
            set 
            { 
                _receivers = value;
                if (_receivers != null && _receivers.ListTemplateIdSpecified)
                {
                    this.Name = "Receivers" + _receivers.ListTemplateId;
                }
            }
        }


        public VSEventHandlerItem(FeatureDefinition feature)
        {
            this.Feature = feature;
            this.Name = "Receivers_" + Guid.NewGuid();
            this.TypeName = Constants.SPTypeNameEventHandler;
            this.GroupName = "Receivers";
            this.SupportedDeploymentScopes = "Web, Site";
            this.DefaultDeploymentType = DeploymentType.ElementFile;
        }

        public VSEventHandlerItem(FeatureDefinition feature, ReceiverDefinitionCollection receivers) : this(feature)
        {
            this.Receivers = receivers;
        }

        public override ElementDefinitionCollection GetElementManifest()
        {
            ElementDefinitionCollection elements = new ElementDefinitionCollection();

            elements.Items = new object[] { this.Receivers };

            return elements;
        }
    }
}

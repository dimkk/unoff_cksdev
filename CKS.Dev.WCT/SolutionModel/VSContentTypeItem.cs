using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Common;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public class VSContentTypeItem : VSFeatureItem
    {

        private ContentTypeDefinition _contentType = null;

        public ContentTypeDefinition ContentType
        {
            get { return _contentType; }
            set
            {
                _contentType = value;
                if (_contentType != null && !String.IsNullOrEmpty(_contentType.Name))
                {
                    this.Name = _contentType.Name;
                }
            }
        }



        public VSContentTypeItem(FeatureDefinition feature)
        {
            this.Feature = feature;
            this.Name = "ContentType_" + Guid.NewGuid();
            this.TypeName = Constants.SPTypeNameContentType;
            this.GroupName = "ContentTypes";
            this.SupportedDeploymentScopes = "Web, Site";
            this.DefaultDeploymentType = DeploymentType.ElementFile;
        }

        public override ElementDefinitionCollection GetElementManifest()
        {
            ElementDefinitionCollection elements = new ElementDefinitionCollection();

            elements.Items = new object[] { this.ContentType };

            return elements;
        }
    }
}

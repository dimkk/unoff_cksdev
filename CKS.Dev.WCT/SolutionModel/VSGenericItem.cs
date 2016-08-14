using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Common;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public class VSGenericItem : VSFeatureItem
    {

        //private object _item = null;

        //public Object Item
        //{
        //    get { return _item; }
        //    set
        //    {
        //        _item = value;
        //        if (_item != null)
        //        {
        //            Type type = _item.GetType();
        //            this.Name = type.Name + "_" + Guid.NewGuid();
        //        }
        //    }
        //}

        private List<object> _items;
        public List<object> Items
        {
            get { return _items; }
            set { _items = value; }
        }
        

        public VSGenericItem(FeatureDefinition feature)
        {
            this.Feature = feature;
            this.Name = "Generic_" + Guid.NewGuid();
            this.TypeName = Constants.SPTypeNameGenericElement;
            this.GroupName = "Generic";
            this.SupportedDeploymentScopes = "Web, Site";
            this.DefaultDeploymentType = DeploymentType.ElementFile;
        }

        public override ElementDefinitionCollection GetElementManifest()
        {
            if (this.Items != null)
            {
                ElementDefinitionCollection elements = new ElementDefinitionCollection();

                elements.Items = this.Items.ToArray();

                return elements;
            }
            return null;
        }
    }
}

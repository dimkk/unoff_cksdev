using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Features;
using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Extensions;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class UpgradeActionsDefinition 
    {

        public void Setup(IFeature vsFeature)
        {
            vsFeature.UpgradeActionsReceiverAssembly = this.ReceiverAssembly;
            vsFeature.UpgradeActionsReceiverClass = this.ReceiverClass;

            foreach (object item in this.Items.AsSafeEnumable())
            {
                //[System.Xml.Serialization.XmlElementAttribute("AddContentTypeField", typeof(AddContentTypeFieldDefinition))]
                if (item is AddContentTypeFieldDefinition)
                {
                    AddContentTypeFieldDefinition def = (AddContentTypeFieldDefinition)item;
                    def.Setup(vsFeature.UpgradeActions.AddAddContentTypeFieldUpgradeAction());
                    continue;
                }

                //[System.Xml.Serialization.XmlElementAttribute("ApplyElementManifests", typeof(ElementManifestReferences))]
                if (item is ElementManifestReferences)
                {
                    ElementManifestReferences def = (ElementManifestReferences)item;
                    def.Setup(vsFeature.UpgradeActions.AddApplyElementManifestsUpgradeAction());
                    continue;
                }
                
                //[System.Xml.Serialization.XmlElementAttribute("CustomUpgradeAction", typeof(CustomUpgradeActionDefinition))]
                if (item is CustomUpgradeActionDefinition)
                {
                    CustomUpgradeActionDefinition def = (CustomUpgradeActionDefinition)item;
                    def.Setup(vsFeature.UpgradeActions.AddCustomUpgradeAction());
                    continue;
                }

                //[System.Xml.Serialization.XmlElementAttribute("MapFile", typeof(MapFileDefinition))]
                if (item is MapFileDefinition)
                {
                    MapFileDefinition def = (MapFileDefinition)item;
                    def.Setup(vsFeature.UpgradeActions.AddMapFileUpgradeAction());
                    continue;
                }

                //[System.Xml.Serialization.XmlElementAttribute("VersionRange", typeof(VersionRangeDefinition))]
                if (item is VersionRangeDefinition)
                {
                    VersionRangeDefinition def = (VersionRangeDefinition)item;
                    def.Setup(vsFeature.UpgradeActions.AddVersionRange());
                    continue;
                }
            }
        }
    }
}

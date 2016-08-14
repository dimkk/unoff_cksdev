using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Features;
using CKS.Dev.WCT.Framework.Extensions;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class VersionRangeDefinition
    {
        public void Setup(IVersionRange version)
        {
            version.BeginVersion = new System.Version(this.BeginVersion);
            version.EndVersion = new System.Version(this.EndVersion);
            
            foreach(AddContentTypeFieldDefinition def in this.AddContentTypeField.AsSafeEnumable())
            {
                def.Setup(version.UpgradeActions.AddAddContentTypeFieldUpgradeAction());
            }

            foreach (ElementManifestReferences def in this.ApplyElementManifests.AsSafeEnumable())
            {
                def.Setup(version.UpgradeActions.AddApplyElementManifestsUpgradeAction());
            }

            foreach (CustomUpgradeActionDefinition def in this.CustomUpgradeAction.AsSafeEnumable())
            {
                def.Setup(version.UpgradeActions.AddCustomUpgradeAction());
            }

            foreach (MapFileDefinition def in this.MapFile.AsSafeEnumable())
            {
                def.Setup(version.UpgradeActions.AddMapFileUpgradeAction());
            }
            
        }
    }
}

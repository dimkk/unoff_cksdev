using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Features;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class ElementManifestReferences
    {
        public void Setup(IApplyElementManifestsUpgradeAction action)
        {
            for(long i = 0; i < this.Items.LongLength; i++)
            {
                ElementManifestReference item = this.Items[i];

                if (this.ItemsElementName[i] == ItemsChoiceType.ElementFile)
                {
                    IElement element = action.Elements.AddElementFile();
                    element.Location = item.Location;
                }
                else
                {
                    IElement element = action.Elements.AddElementManifest();
                    element.Location = item.Location;
                }
            }
            
        }
    }
}

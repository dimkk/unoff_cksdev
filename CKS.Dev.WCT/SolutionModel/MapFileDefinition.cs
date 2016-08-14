using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Features;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class MapFileDefinition
    {
        public void Setup(IMapFileUpgradeAction action)
        {
            action.FromPath = this.FromPath;
            action.ToPath = this.ToPath;
        }
    }
}

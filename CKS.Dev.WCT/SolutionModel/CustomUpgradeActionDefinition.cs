using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Features;
using CKS.Dev.WCT.Framework.Extensions;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class CustomUpgradeActionDefinition 
    {
        public void Setup(ICustomUpgradeAction action)
        {
            action.Name = this.Name;

            foreach(ParameterDefinition def in this.Parameters.AsSafeEnumable())
            {
                ICustomUpgradeActionParameter parameter = action.Parameters.Add();
                parameter.Name = def.Name;
                parameter.Value = String.Join(Environment.NewLine, def.Text);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;

namespace CKS.Dev11.VisualStudio.SharePoint.Commands.Info
{
    [Serializable]
    internal class FeatureActivationInfo
    {
        public DeploymentFeatureInfo[] Features { get; set; }
        public bool IsSandboxedSolution { get; set; }
    }
}

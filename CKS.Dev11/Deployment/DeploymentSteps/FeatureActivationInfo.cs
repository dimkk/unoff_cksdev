using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;

namespace CKS.Dev11.VisualStudio.SharePoint.Deployment.DeploymentSteps
{
    [Serializable]
    internal class FeatureActivationInfo
    {
        /// <summary>
        /// Gets or sets the features.
        /// </summary>
        /// <value>
        /// The features.
        /// </value>
        public DeploymentFeatureInfo[] Features { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is sandboxed solution.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is sandboxed solution; otherwise, <c>false</c>.
        /// </value>
        public bool IsSandboxedSolution { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Commands.Info
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Commands.Info
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Commands.Info
#else
    namespace CKS.Dev.VisualStudio.SharePoint.Commands.Info
#endif
    {
    /// <summary>
    /// Feature Activation Info
    /// </summary>
    [Serializable]
    public class FeatureActivationInfo
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

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
    /// Feature scopes.
    /// </summary>
    public enum DeploymentFeatureScope
    {
        /// <summary>
        /// Farm
        /// </summary>
        Farm = 0,
        /// <summary>
        /// Web Application
        /// </summary>
        WebApplication = 1,
        /// <summary>
        /// Site Collection
        /// </summary>
        Site = 2,
        /// <summary>
        /// Web
        /// </summary>
        Web = 3
    }
}

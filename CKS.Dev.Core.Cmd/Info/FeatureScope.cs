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
    public enum FeatureScope
    {
        /// <summary>
        /// Web.
        /// </summary>
        Web,
        /// <summary>
        /// Site.
        /// </summary>
        Site,
        /// <summary>
        /// Web application.
        /// </summary>
        WebApplication,
        /// <summary>
        /// Farm.
        /// </summary>
        Farm
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Commands
#elif VS2013Build_SYMBOL
    namespace CKS.Dev12.VisualStudio.SharePoint.Commands
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Commands
#else
    namespace CKS.Dev.VisualStudio.SharePoint.Commands
#endif
    {
    /// <summary>
    /// Site Command Ids
    /// </summary>
    public static class SiteCommandIds
    {
        /// <summary>
        /// Is the site a publishing site
        /// </summary>
        public const string IsPublishingSiteCommandId = "Site.IsPublishingSite";

        /// <summary>
        /// Get the publishing pages
        /// </summary>
        public const string GetPublishingPagesCommandId = "Site.GetPublishingPages";
    }
}

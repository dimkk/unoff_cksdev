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
    /// Publishing Page Command Ids
    /// </summary>
    public static class PublishingPageCommandIds
    {
        /// <summary>
        /// Export the publishing page to xml
        /// </summary>
        public const string ExportToXml = "PublishingPage.ExportToXml";

        /// <summary>
        /// Get the publishing page properties
        /// </summary>
        public const string GetProperties = "PublishingPage.GetProperties";
    }
}

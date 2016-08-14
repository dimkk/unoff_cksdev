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
    /// SharePoint Command Ids for web part gallery.
    /// </summary>
    public static class WebPartGallerySharePointCommandIds
    {
        #region Constants

        /// <summary>
        /// Get web parts.
        /// </summary>
        public const string GetWebParts = "WebPartGallery.GetWebParts";

        /// <summary>
        /// Get the web part gallery properties.
        /// </summary>
        public const string GetWebPartGalleryProperties = "WebPartGallery.GetWebPartGalleryProperties";

        /// <summary>
        /// Get the web part gallery all items url.
        /// </summary>
        public const string GetWebPartGalleryAllItemsUrl = "WebPartGallery.GetWebPartGalleryAllItemsUrl";

        #endregion
    }
}

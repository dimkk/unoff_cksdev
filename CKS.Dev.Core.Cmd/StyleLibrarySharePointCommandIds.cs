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
    /// SharePoint Command Ids for style library.
    /// </summary>
    public static class StyleLibrarySharePointCommandIds
    {
        #region Constants

        /// <summary>
        /// The get style library properties
        /// </summary>
        public const string GetStyleLibraryProperties = "StyleLibrary.GetStyleLibraryProperties";

        /// <summary>
        /// The get style library all items URL
        /// </summary>
        public const string GetStyleLibraryAllItemsUrl = "StyleLibrary.GetStyleLibraryAllItemsUrl";

        #endregion
    }
}

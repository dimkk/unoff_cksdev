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
    /// SharePoint Command Ids for solution gallery.
    /// </summary>
    public static class SolutionGallerySharePointCommandIds
    {
        #region Constants

        /// <summary>
        /// Get solutions.
        /// </summary>
        public const string GetSolutions = "SolutionGallery.GetSolutions";

        /// <summary>
        /// Get the solution gallery properties.
        /// </summary>
        public const string GetSolutionGalleryProperties = "SolutionGallery.GetSolutionGalleryProperties";

        /// <summary>
        /// Get the solution gallery all items url.
        /// </summary>
        public const string GetSolutionGalleryAllItemsUrl = "SolutionGallery.GetSolutionGalleryAllItemsUrl";

        #endregion
    }
}

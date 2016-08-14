using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev.VisualStudio.SharePoint.Commands
{
    /// <summary>
    /// The SharePoint command Ids for Content Types.
    /// </summary>
    public class ContentTypeSharePointCommandIds
    {
        #region Constants

        /// <summary>
        /// Create Page Layout from a Publishing Content Type
        /// </summary>
        public const string CreatePageLayoutCommand = "Explorer.CreatePageLayout";

        /// <summary>
        /// Check if the given Content Type is a Publishing Content Type
        /// </summary>
        public const string IsPublishingContentTypeCommand = "Explorer.IsPublishingContentType";

        #endregion
    }
}

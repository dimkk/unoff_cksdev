using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev11.VisualStudio.SharePoint.Commands
{
    /// <summary>
    /// SharePoint Command Ids for content types.
    /// </summary>
    public static class ContentTypeSharePointCommandIds
    {
        #region Constants

        /// <summary>
        /// Get the Content Type ID.
        /// </summary>
        public const string GetContentTypeID = "ContentType.GetContentTypeID";

        /// <summary>
        /// Get the Is Built In Content Type check result.
        /// </summary>
        public const string IsBuiltInContentType = "ContentType.IsBuiltInContentType";

        /// <summary>
        /// Get the Content Type import properties.
        /// </summary>
        public const string GetContentTypeImportProperties = "ContentType.GetContentTypeImportProperties";

        /// <summary>
        /// Get Content Type groups from the given site.
        /// </summary>
        public const string GetContentTypeGroups = "ContentType.GetContentTypeGroups";

        /// <summary>
        /// Get Content Types that belong to a specific group.
        /// </summary>
        public const string GetContentTypesFromGroup = "ContentType.GetContentTypesFromGroup";

        #endregion
    }
}

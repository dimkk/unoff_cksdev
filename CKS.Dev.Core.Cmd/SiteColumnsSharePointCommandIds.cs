﻿using System;
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
    /// SharePoint Command Ids for Site Columns
    /// </summary>
    public static class SiteColumnsSharePointCommandIds
    {
        #region Constants

        /// <summary>
        /// Get site columns groups
        /// </summary>
        public const string GetSiteColumnsGroups = "SiteColumns.GetSiteColumnsGroups";

        /// <summary>
        /// Get site columns from group.
        /// </summary>
        public const string GetSiteColumnsFromGroup = "SiteColumns.GetSiteColumnsFromGroup";

        /// <summary>
        /// Get site column properties.
        /// </summary>
        public const string GetProperties = "SiteColumns.GetSiteColumnProperties";

        #endregion
    }
}

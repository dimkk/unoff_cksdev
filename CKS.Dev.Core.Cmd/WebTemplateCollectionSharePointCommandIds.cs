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
    /// SharePoint Command Ids for web templates.
    /// </summary>
    public static class WebTemplateCollectionSharePointCommandIds
    {
        #region Constants

        /// <summary>
        /// Get the web templates categories.
        /// </summary>
        public const string GetWebTemplateCategories = "WebTemplateCollection.GetWebTemplateCategories";

        /// <summary>
        /// Get the available web templates by category.
        /// </summary>
        public const string GetAvailableWebTemplatesByCategory = "WebTemplateCollection.GetAvailableWebTemplatesByCategory";

        /// <summary>
        /// Get the web templates
        /// </summary>
        public const string GetWebTemplates = "WebTemplateCollection.GetWebTemplates";

        #endregion
    }
}

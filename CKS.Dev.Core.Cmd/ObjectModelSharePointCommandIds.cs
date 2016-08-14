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
    /// SharePoint Command Ids for the object model.
    /// </summary>
    public static class ObjectModelSharePointCommandIds
    {
        #region Constants

        /// <summary>
        /// Get the SP Base Permissions.
        /// </summary>
        public const string GetSPBasePermissions = "ObjectModel.GetSPBasePermissions";

        /// <summary>
        /// Get the full SP Root folder path
        /// </summary>
        public const string GetFullSPRootFolderPath = "ObjectModel.GetFullSPRootFolderPath";

        #endregion
    }
}

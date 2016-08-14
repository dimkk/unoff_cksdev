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
    /// SharePoint Command Ids for themes.
    /// </summary>
    public static class ThemeSharePointCommandIds
    {
        #region Constants

        /// <summary>
        /// Get the theme properties.
        /// </summary>
        public const string GetThemeProperties = "Theme.GetThemeProperties";

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Commands;
using Microsoft.SharePoint;
using System.IO;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;

namespace CKS.Dev11.VisualStudio.SharePoint.Commands
{
    /// <summary>
    /// The theme commands.
    /// </summary>
    internal class ThemeSharePointCommands
    {
        #region Methods

        /// <summary>
        /// Gets additional property data for a specific theme.
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="nodeInfo">The node info</param>
        /// <returns>The properties</returns>
        [SharePointCommand(ThemeSharePointCommandIds.GetThemeProperties)]
        private static Dictionary<string, string> GetThemeProperties(ISharePointCommandContext context,
            FileNodeInfo nodeInfo)
        {
            SPList themes = context.Site.GetCatalog(SPListTemplateType.ThemeCatalog);
            SPListItem theme = themes.Items[nodeInfo.UniqueId];

            return SharePointCommandServices.GetProperties(theme);
        }

        #endregion
    }
}

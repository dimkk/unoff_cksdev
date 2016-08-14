using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Commands;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
using Microsoft.SharePoint;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Common.ExtensionMethods;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Properties;

namespace CKS.Dev11.VisualStudio.SharePoint.Commands
{
    /// <summary>
    /// The design catalog commands.
    /// </summary>
    class DesignCatalogSharePointCommands
    {
        /// <summary>
        /// Gets additional property data for the design catalog.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="nodeInfo">The node info.</param>
        /// <returns></returns>
        [SharePointCommand(DesignCatalogSharePointCommandIds.GetDesignCatalogProperties)]
        private static Dictionary<string, string> GetDesignCatalogProperties(ISharePointCommandContext context,
            DesignCatalogNodeInfo nodeInfo)
        {
            return SharePointCommandServices.GetProperties(context.Site.GetCatalog(SPListTemplateType.DesignCatalog));
        }

        /// <summary>
        /// Get the default view url for the design catalog.
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns>The default view of the design catalog</returns>
        [SharePointCommand(DesignCatalogSharePointCommandIds.GetDesignCatalogAllItemsUrl)]
        private static string GetDesignCatalogAllItemsUrl(ISharePointCommandContext context)
        {
            return context.Site.GetCatalog(SPListTemplateType.DesignCatalog).DefaultViewUrl;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Commands;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using System.IO;
using CKS.Dev.Core.Cmd.Imp.v5.Properties;


#if VS2012Build_SYMBOL
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
#elif VS2013Build_SYMBOL
    using CKS.Dev12.VisualStudio.SharePoint.Commands.Info;
#elif VS2014Build_SYMBOL
    using CKS.Dev13.VisualStudio.SharePoint.Commands.Info;
#else
    using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
#endif

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

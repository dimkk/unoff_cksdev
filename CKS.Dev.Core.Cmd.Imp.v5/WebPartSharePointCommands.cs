using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Commands;
using Microsoft.SharePoint;
using System.IO;

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
    /// The web part commands
    /// </summary>
    internal class WebPartSharePointCommands
    {
        #region Methods

        /// <summary>
        /// Gets additional property data for a specific Web Part.
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="nodeInfo">The node info</param>
        /// <returns>The properties</returns>
        [SharePointCommand(WebPartSharePointCommandIds.GetWebPartProperties)]
        private static Dictionary<string, string> GetWebPartProperties(ISharePointCommandContext context,
            FileNodeInfo nodeInfo)
        {
            SPList webParts = context.Site.GetCatalog(SPListTemplateType.WebPartCatalog);
            SPListItem webPart = webParts.Items[nodeInfo.UniqueId];

            return SharePointCommandServices.GetProperties(webPart);
        }

        /// <summary>
        /// Gets additional property data for a specific Web Part.
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="nodeInfo">The node info</param>
        /// <returns>The properties</returns>
        [SharePointCommand(WebPartSharePointCommandIds.GetWebPartDefinition)]
        public static string GetWebPartDefinition(ISharePointCommandContext context, WebPartNodeInfo nodeInfo)
        {
            SPList webParts = context.Site.GetCatalog(SPListTemplateType.WebPartCatalog);
            SPListItem webPart = webParts.Items[nodeInfo.UniqueId];
            using (StreamReader reader = new StreamReader(webPart.File.OpenBinaryStream(SPOpenBinaryOptions.None)))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion
    }
}

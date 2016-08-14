using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Commands;
using Microsoft.SharePoint;

using CKS.Dev.Core.Cmd.Imp.v5.Properties;
#if VS2012Build_SYMBOL
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Common.ExtensionMethods;
#elif VS2013Build_SYMBOL
    using CKS.Dev12.VisualStudio.SharePoint.Commands.Info;
    using CKS.Dev12.VisualStudio.SharePoint.Commands.Common.ExtensionMethods;
#elif VS2014Build_SYMBOL
    using CKS.Dev13.VisualStudio.SharePoint.Commands.Info;
    using CKS.Dev13.VisualStudio.SharePoint.Commands.Common.ExtensionMethods;
#else
    using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
    using CKS.Dev.VisualStudio.SharePoint.Commands.Common.ExtensionMethods;
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
    /// The solution gallery commands.
    /// </summary>
    class SolutionGallerySharePointCommands
    {
        /// <summary>
        /// Gets data for each solution on the SharePoint site, and returns an array of 
        /// serializable objects that contain the data.
        /// </summary>
        /// <param name="context">The command context</param>
        /// <returns>The solution infos</returns>
        [SharePointCommand(SolutionGallerySharePointCommandIds.GetSolutions)]
        private static FileNodeInfo[] GetSolutions(ISharePointCommandContext context)
        {
            List<FileNodeInfo> nodeInfos = new List<FileNodeInfo>();
            try
            {
                context.Logger.WriteLine(Resources.SolutionGallerySharePointCommands_TryingToRetrieveAvailableSolutions, LogCategory.Status);

                SPListItemCollection solutions = context.Web.GetCatalog(SPListTemplateType.SolutionCatalog).GetItems(
                    new SPQuery
                    {
                        ViewXml = "<View />"
                    }
                );
                nodeInfos = solutions.ToFileNodeInfo();

                context.Logger.WriteLine(Resources.SolutionGallerySharePointCommands_RetrievingException, LogCategory.Status);
            }
            catch (Exception ex)
            {
                context.Logger.WriteLine(String.Format(Resources.SolutionGallerySharePointCommands_RetrievingException,
                          ex.Message,
                          Environment.NewLine,
                          ex.StackTrace), LogCategory.Error);
            }

            return nodeInfos.ToArray();
        }

        /// <summary>
        /// Gets additional property data for the solution gallery.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="nodeInfo">The node info.</param>
        /// <returns></returns>
        [SharePointCommand(SolutionGallerySharePointCommandIds.GetSolutionGalleryProperties)]
        private static Dictionary<string, string> GetSolutionGalleryProperties(ISharePointCommandContext context,
            SolutionGalleryNodeInfo nodeInfo)
        {
            return SharePointCommandServices.GetProperties(context.Site.GetCatalog(SPListTemplateType.SolutionCatalog));
        }

        /// <summary>
        /// Get the default view url for the solution gallery.
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns>The default view of the solution gallery</returns>
        [SharePointCommand(SolutionGallerySharePointCommandIds.GetSolutionGalleryAllItemsUrl)]
        private static string GetSolutionGalleryAllItemsUrl(ISharePointCommandContext context)
        {
            return context.Site.GetCatalog(SPListTemplateType.SolutionCatalog).DefaultViewUrl;
        }
    }
}

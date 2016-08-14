using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Publishing;

using Microsoft.VisualStudio.SharePoint.Commands;
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
    /// The site commands
    /// </summary>
    public static class SiteCommands
    {
        /// <summary>
        /// Determines whether [is publishing site] [the specified context].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if [is publishing site] [the specified context]; otherwise, <c>false</c>.
        /// </returns>
        [SharePointCommand(SiteCommandIds.IsPublishingSiteCommandId)]
        private static bool IsPublishingSite(ISharePointCommandContext context)
        {
            bool isPublishingSite = PublishingWeb.IsPublishingWeb(context.Web);

            return isPublishingSite;
        }

        /// <summary>
        /// Gets the publishing pages.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        [SharePointCommand(SiteCommandIds.GetPublishingPagesCommandId)]
        private static List<PublishingPageInfo> GetPublishingPages(ISharePointCommandContext context)
        {
            List<PublishingPageInfo> pages = new List<PublishingPageInfo>();

            PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(context.Web);
            PublishingPageCollection publishingPages = publishingWeb.GetPublishingPages();
            pages = (from PublishingPage page
                    in publishingPages
                     select new PublishingPageInfo
                     {
                         Name = page.Name,
                         ServerRelativeUrl = page.Uri.AbsolutePath,
                         Title = page.Title,
                     }).ToList();

            return pages;
        }
    }
}

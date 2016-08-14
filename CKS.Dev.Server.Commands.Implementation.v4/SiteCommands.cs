using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Commands;
using Microsoft.SharePoint.Publishing;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;

namespace CKS.Dev.VisualStudio.SharePoint.Commands
{
    public static class SiteCommands
    {
        [SharePointCommand(SiteCommandIds.IsPublishingSiteCommandId)]
        private static bool IsPublishingSite(ISharePointCommandContext context)
        {
            bool isPublishingSite = PublishingWeb.IsPublishingWeb(context.Web);

            return isPublishingSite;
        }

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

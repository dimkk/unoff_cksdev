//--------------------------------------------------------------------------------
// This file is a "Sample" from the SharePoint Foundation 2010
// Samples
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// This source code is intended only as a supplement to Microsoft
// Development Tools and/or on-line documentation.  See these other
// materials for detailed information regarding Microsoft code samples.
// 
// THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//--------------------------------------------------------------------------------
using System;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.SharePoint.Commands;
using CKS.Dev.WCT.Commands;

namespace SharePoint.Tools.Extensions.VSeWSSUpgradeTool.Commands
{
    internal class SharePointCommands
    {
        [SharePointCommand(SharePointCommandIds.ValidateSite)]
        private bool ValidateSite(ISharePointCommandContext context, Uri url)
        {
            using (SPSite site = new SPSite(url.AbsoluteUri))
            {
                string webUrl = DetermineWebUrl(url.AbsolutePath, site.ServerRelativeUrl);

                if (webUrl != null)
                {
                    using (SPWeb web = site.OpenWeb(webUrl, true))
                    {
                        return web.Exists;
                    }
                }
            }

            return false;
        }

        private string DetermineWebUrl(string serverRelativeInputUrl, string serverRelativeSiteUrl)
        {
            serverRelativeInputUrl = EnsureTrailingSlash(serverRelativeInputUrl);
            serverRelativeSiteUrl = EnsureTrailingSlash(serverRelativeSiteUrl);

            string webUrl = null;
            bool isSubString = serverRelativeInputUrl.StartsWith(serverRelativeSiteUrl, StringComparison.OrdinalIgnoreCase);

            if (isSubString)
            {
                webUrl = Uri.UnescapeDataString(serverRelativeInputUrl.Substring(serverRelativeSiteUrl.Length));
            }

            return webUrl;
        }

        private string EnsureTrailingSlash(string url)
        {
            if (!String.IsNullOrEmpty(url)
                && url[url.Length - 1] != '/')
            {
                url += '/';
            }
            return url;
        }
    }
}
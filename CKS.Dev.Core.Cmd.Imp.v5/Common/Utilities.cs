﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if VS2012Build_SYMBOL
namespace CKS.Dev11.VisualStudio.SharePoint.Commands.Common
#elif VS2013Build_SYMBOL
    namespace CKS.Dev12.VisualStudio.SharePoint.Commands.Common
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Commands.Common
#else
    namespace CKS.Dev.VisualStudio.SharePoint.Commands.Common
#endif
{
    /// <summary>
    /// Utilities for the commands.
    /// </summary>
    internal static class Utilities
    {
        #region Methods

        /// <summary>
        /// Combine the string elements into a url.
        /// </summary>
        /// <param name="urls">The elements.</param>
        /// <returns>The combined url.</returns>
        public static string CombineUrl(params string[] urls)
        {
            if (urls == null)
            {
                throw new ArgumentNullException("urls");
            }

            for (int i = 0; i < urls.Length; i++)
            {
                string s = urls[i];
                if (s != null && s.StartsWith("/") || s.EndsWith("/"))
                {
                    urls[i] = s.Trim('/');
                }
            }

            string url = String.Join("/", urls);

            if (!url.StartsWith("/"))
            {
                url = String.Format("/{0}", url);
            }

            return url;
        }

        #endregion
    }
}

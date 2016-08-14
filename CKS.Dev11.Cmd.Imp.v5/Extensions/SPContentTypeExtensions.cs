using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace CKS.Dev11.VisualStudio.SharePoint.Commands.Extensions
{
    /// <summary>
    /// Extension methods for the SPContentType type.
    /// </summary>
    public static class SPContentTypeExtensions
    {
        /// <summary>
        /// Safes the name.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        internal static string SafeName(this SPContentType contentType)
        {
            return ContentTypeSharePointCommands.SafeContentTypeName(contentType.Name);
        }
    }
}

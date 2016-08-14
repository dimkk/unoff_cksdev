using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CKS.Dev.VisualStudio.SharePoint.Commands.Extensions
{
    /// <summary>
    /// Extension methods for the SPContentType type.
    /// </summary>
    public static class SPContentTypeExtensions
    {
        internal static string SafeName(this SPContentType contentType)
        {
            return ContentTypeSharePointCommands.SafeContentTypeName(contentType.Name);
        }
    }
}

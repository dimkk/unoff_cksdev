using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

#if VS2012Build_SYMBOL
namespace CKS.Dev11.VisualStudio.SharePoint.Commands.Extensions
#elif VS2013Build_SYMBOL
    namespace CKS.Dev12.VisualStudio.SharePoint.Commands.Extensions
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Commands.Extensions
#else
    namespace CKS.Dev.VisualStudio.SharePoint.Commands.Extensions
#endif
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

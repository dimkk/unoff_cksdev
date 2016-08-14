using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class FieldSharePointCommands
    {
        [SharePointCommand(FieldSharePointCommandIds.GetProperties)]
        public static Dictionary<string, string> GetProperties(ISharePointCommandContext context, FieldNodeInfo field)
        {
            Dictionary<string, string> properties = null;

            if (field.ListId == Guid.Empty)
            {
                properties = SharePointCommandServices.GetProperties(context.Web.AvailableContentTypes[field.ContentTypeName].Fields[field.Id]);
            }
            else
            {
                properties = SharePointCommandServices.GetProperties(context.Web.Lists[field.ListId].Fields[field.Id]);
            }

            return properties;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

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
namespace CKS.Dev11.VisualStudio.SharePoint.Commands.Common.ExtensionMethods
#elif VS2013Build_SYMBOL
    namespace CKS.Dev12.VisualStudio.SharePoint.Commands.Common.ExtensionMethods
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Commands.Common.ExtensionMethods
#else
    namespace CKS.Dev.VisualStudio.SharePoint.Commands.Common.ExtensionMethods
#endif
{
    internal static class SPFileCollectionExtensions
    {
        internal static List<FileNodeInfo> ToFileNodeInfo(this SPFileCollection files)
        {
            List<FileNodeInfo> nodeInfos = new List<FileNodeInfo>();

            foreach (SPFile file in files)
            {
                FileNodeInfo nodeInfo = new FileNodeInfo
                {
                    Id = file.Item.ID,
                    Name = file.Name,
                    UniqueId = file.Item.UniqueId,
                    FileType = file.Item[SPBuiltInFieldId.File_x0020_Type] as string,
                    ServerRelativeUrl = file.ServerRelativeUrl,
                    Title = file.Item.Title,
                    IsCheckedOut = file.Level == SPFileLevel.Checkout
                };
                nodeInfos.Add(nodeInfo);
            }

            return nodeInfos;
        }
    }
}

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
    internal static class SPFolderCollectionExtensions
    {
        internal static List<FolderNodeInfo> ToFolderNodeInfo(this SPFolderCollection folders)
        {
            List<FolderNodeInfo> nodeInfos = new List<FolderNodeInfo>();

            foreach (SPFolder folder in folders)
            {
                FolderNodeInfo nodeInfo = new FolderNodeInfo
                {
                    Name = folder.Name,
                    Url = folder.Url
                };
                nodeInfos.Add(nodeInfo);
            }

            return nodeInfos;
        }
    }
}

using System;
using System.Collections.Generic;

using Microsoft.SharePoint;
using Microsoft.VisualStudio.SharePoint.Commands;
using CKS.Dev.Core.Cmd.Imp.v4.Properties;

#if VS2012Build_SYMBOL
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Common.ExtensionMethods;
#elif VS2013Build_SYMBOL
    using CKS.Dev12.VisualStudio.SharePoint.Commands.Info;
    using CKS.Dev12.VisualStudio.SharePoint.Commands.Common.ExtensionMethods;
#elif VS2014Build_SYMBOL
    using CKS.Dev13.VisualStudio.SharePoint.Commands.Info;
    using CKS.Dev13.VisualStudio.SharePoint.Commands.Common.ExtensionMethods;
#else
    using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
    using CKS.Dev.VisualStudio.SharePoint.Commands.Common.ExtensionMethods;
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
    class MasterPageGallerySharePointCommands
    {
        [SharePointCommand(MasterPageGallerySharePointCommandIds.GetMasterPagesAndPageLayoutsCommand)]
        private static FileNodeInfo[] GetMasterPagesAndPageLayouts(ISharePointCommandContext context)
        {
            List<FileNodeInfo> nodeInfos = new List<FileNodeInfo>();
            try
            {
                context.Logger.WriteLine(Resources.MasterPageGallerySharePointCommands_TryingToRetrieveAvailableMasterPagesAndPageLayouts, LogCategory.Status);

                SPListItemCollection masterPagesAndPageLayouts = context.Web.GetCatalog(SPListTemplateType.MasterPageCatalog).GetItems(
                    new SPQuery
                    {
                        ViewXml = "<View />"
                    }
                );
                nodeInfos = masterPagesAndPageLayouts.ToFileNodeInfo();

                context.Logger.WriteLine(Resources.MasterPageGallerySharePointCommands_MasterPagesAndPageLayoutsSuccessfullyRetrieved, LogCategory.Status);
            }
            catch (Exception ex)
            {
                context.Logger.WriteLine(String.Format(Resources.MasterPageGallerySharePointCommands_RetrievingException,
                          ex.Message,
                          Environment.NewLine,
                          ex.StackTrace), LogCategory.Error);
            }

            return nodeInfos.ToArray();
        }

        [SharePointCommand(MasterPageGallerySharePointCommandIds.GetMasterPagesOrPageLayoutPropertiesCommand)]
        private static Dictionary<string, string> GetMasterPageOrPageLayoutProperties(ISharePointCommandContext context, FileNodeInfo fileNodeInfo)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            try
            {
                SPList masterPageGallery = context.Web.GetCatalog(SPListTemplateType.MasterPageCatalog);
                SPListItem masterPageOrPageLayout = masterPageGallery.Items[fileNodeInfo.UniqueId];

                properties = SharePointCommandServices.GetProperties(masterPageOrPageLayout);
            }
            catch { }

            return properties;
        }
    }
}

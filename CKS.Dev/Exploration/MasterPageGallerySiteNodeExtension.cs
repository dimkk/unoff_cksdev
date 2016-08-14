using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Explorer;
using System.ComponentModel.Composition;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    [Export(typeof(IExplorerNodeTypeExtension))]
    [ExplorerNodeType(ExplorerNodeTypes.SiteNode)]
    internal class MasterPageGallerySiteNodeExtension : IExplorerNodeTypeExtension
    {
        private Uri siteUrl = null;

        public void Initialize(IExplorerNodeType nodeType)
        {
            nodeType.NodeChildrenRequested += NodeChildrenRequested;
        }

        private void NodeChildrenRequested(object sender, ExplorerNodeEventArgs e)
        {
            IExplorerNode siteNode = e.Node;
            IExplorerSiteNodeInfo siteInfo = siteNode.Annotations.GetValue<IExplorerSiteNodeInfo>();
            if (siteInfo != null && siteInfo.IsConnectionRoot)
            {
                siteUrl = siteInfo.Url;
            }

            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.ViewMasterPageAndPageLayoutGallery, true))
            {
                e.Node.ChildNodes.AddFolder(Resources.MasterPageGallerySiteNodeExtension_NodeName, Resources.MasterPagesNode.ToBitmap(), CreateMasterPagesAndPageLayoutsNodes);
            }
        }

        private void CreateMasterPagesAndPageLayoutsNodes(IExplorerNode parentNode)
        {
            FileNodeInfo[] masterPagesAndPageLayouts = parentNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo[]>(MasterPageGallerySharePointCommandIds.GetMasterPagesAndPageLayoutsCommand);

            if (masterPagesAndPageLayouts != null)
            {
                foreach (FileNodeInfo masterPageOrPageLayout in masterPagesAndPageLayouts)
                {
                    var annotations = new Dictionary<object, object>
                    {
                        { typeof(FileNodeInfo), masterPageOrPageLayout }
                    };

                    string nodeTypeId = PageLayoutNodeTypeProvider.PageLayoutNodeTypeId;

                    if (masterPageOrPageLayout.FileType.Equals("master", StringComparison.InvariantCultureIgnoreCase))
                    {
                        nodeTypeId = MasterPageNodeTypeProvider.MasterPageNodeTypeId;
                    }

                    IExplorerNode masterPageOrPageLayoutNode = parentNode.ChildNodes.Add(nodeTypeId, masterPageOrPageLayout.Name, annotations);

                    if (masterPageOrPageLayout.IsCheckedOut)
                    {
                        if (masterPageOrPageLayout.FileType.Equals("master", StringComparison.InvariantCultureIgnoreCase))
                        {
                            masterPageOrPageLayoutNode.Icon = Resources.MasterPageNodeCheckedOut.ToBitmap();
                        }
                        else
                        {
                            masterPageOrPageLayoutNode.Icon = Resources.PageNodeCheckedOut.ToBitmap();
                        }
                    }
                }
            }
        }
    }
}

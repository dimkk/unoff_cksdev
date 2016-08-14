using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Explorer;
using System.ComponentModel.Composition;
using CKS.Dev.VisualStudio.SharePoint.Commands;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    [Export(typeof(IExplorerNodeTypeExtension))]
    [ExplorerNodeType(ExplorerNodeTypes.SiteNode)]
    public class PublishingPagesSiteExtension : IExplorerNodeTypeExtension
    {
        public void Initialize(IExplorerNodeType nodeType)
        {
            nodeType.NodeChildrenRequested += new EventHandler<ExplorerNodeEventArgs>(nodeType_NodeChildrenRequested);
        }

        void nodeType_NodeChildrenRequested(object sender, ExplorerNodeEventArgs e)
        {
            IExplorerNode siteNode = e.Node;
            if (siteNode.Context.SharePointConnection.ExecuteCommand<bool>(SiteCommandIds.IsPublishingSiteCommandId))
            {
                IExplorerNode pages = siteNode.ChildNodes.AddFolder("Pages", Properties.Resources.PagesNode.ToBitmap(), new Action<IExplorerNode>(PublishingPageNodeTypeProvider.CreatePublishingPageNodes));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Explorer;
using Microsoft.VisualStudio.SharePoint;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Commands;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    [Export(typeof(IExplorerNodeTypeProvider))]
    [ExplorerNodeType(PageLayoutNodeTypeProvider.PageLayoutNodeTypeId)]
    internal class PageLayoutNodeTypeProvider : FileNodeTypeProvider
    {
        internal const string PageLayoutNodeTypeId = "CKS.Dev.VisualStudio.SharePoint.Exploration.PageLayout";

        public override void InitializeType(IExplorerNodeTypeDefinition typeDefinition)
        {
            base.InitializeType(typeDefinition);

            typeDefinition.DefaultIcon = Resources.PageNode.ToBitmap();
            typeDefinition.IsAlwaysLeaf = true;

            typeDefinition.NodePropertiesRequested += NodePropertiesRequested;
        }

        private void NodePropertiesRequested(object sender, ExplorerNodePropertiesRequestedEventArgs e)
        {
            IExplorerNode pageLayoutNode = e.Node;
            FileNodeInfo pageLayout = pageLayoutNode.Annotations.GetValue<FileNodeInfo>();
            Dictionary<string, string> properties = pageLayoutNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, Dictionary<string, string>>(MasterPageGallerySharePointCommandIds.GetMasterPagesOrPageLayoutPropertiesCommand, pageLayout);
            object propertySource = pageLayoutNode.Context.CreatePropertySourceObject(properties);
            e.PropertySources.Add(propertySource);
        }
    }
}

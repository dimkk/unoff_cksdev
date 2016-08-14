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
    [ExplorerNodeType(MasterPageNodeTypeProvider.MasterPageNodeTypeId)]
    internal class MasterPageNodeTypeProvider : FileNodeTypeProvider
    {
        internal const string MasterPageNodeTypeId = "CKS.Dev.VisualStudio.SharePoint.Exploration.MasterPage";

        public override void InitializeType(IExplorerNodeTypeDefinition typeDefinition)
        {
            base.InitializeType(typeDefinition);

            typeDefinition.DefaultIcon = Resources.MasterPageNode.ToBitmap();
            typeDefinition.IsAlwaysLeaf = true;

            typeDefinition.NodePropertiesRequested += NodePropertiesRequested;
        }

        private void NodePropertiesRequested(object sender, ExplorerNodePropertiesRequestedEventArgs e)
        {
            IExplorerNode masterPageNode = e.Node;
            FileNodeInfo masterPage = masterPageNode.Annotations.GetValue<FileNodeInfo>();
            Dictionary<string, string> masterPageProperties = masterPageNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, Dictionary<string, string>>(MasterPageGallerySharePointCommandIds.GetMasterPagesOrPageLayoutPropertiesCommand, masterPage);
            object propertySource = masterPageNode.Context.CreatePropertySourceObject(masterPageProperties);
            e.PropertySources.Add(propertySource);
        }
    }
}

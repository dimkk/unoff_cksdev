using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Explorer;
using CKS.Dev.VisualStudio.SharePoint.Properties;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    [Export(typeof(IExplorerNodeTypeProvider))]
    [ExplorerNodeType(FolderNodeTypeProvider.FolderNodeTypeId)]
    internal class FolderNodeTypeProvider : IExplorerNodeTypeProvider
    {
        internal const string FolderNodeTypeId = "CKS.Dev.VisualStudio.SharePoint.Exploration.Folder";

        public void InitializeType(IExplorerNodeTypeDefinition typeDefinition)
        {
            typeDefinition.DefaultIcon = Resources.FolderNode.ToBitmap();
            typeDefinition.IsAlwaysLeaf = false;

            typeDefinition.NodeChildrenRequested += typeDefinition_NodeChildrenRequested;
        }

        void typeDefinition_NodeChildrenRequested(object sender, ExplorerNodeEventArgs e)
        {
            FileNodeTypeProvider.CreateFilesNodes(e.Node);
        }
    }
}

using CKS.Dev11.VisualStudio.SharePoint.Commands;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev11.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev11.VisualStudio.SharePoint.Properties;
using Microsoft.VisualStudio.SharePoint.Explorer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace CKS.Dev11.VisualStudio.SharePoint.Explorer
{
    /// <summary>
    /// Represents an extension of SharePoint site nodes in Server Explorer 
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(IExplorerNodeTypeExtension))]
    // Indicates that this class extends SharePoint site nodes in Server Explorer.
    [ExplorerNodeType(ExplorerNodeTypes.SiteNode)]
    internal class WebPartGallerySiteNodeExtension : IExplorerNodeTypeExtension
    {
        #region Methods

        /// <summary>
        /// Initialise the node.
        /// </summary>
        /// <param name="nodeType">The node type.</param>
        public void Initialize(IExplorerNodeType nodeType)
        {
            nodeType.NodeChildrenRequested += new EventHandler<ExplorerNodeEventArgs>(nodeType_NodeChildrenRequested);
        }

        /// <summary>
        /// Process the node children request to get the web part nodes.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The ExplorerNodeEventArgs object.</param>
        void nodeType_NodeChildrenRequested(object sender, ExplorerNodeEventArgs e)
        {
            Uri siteUrl = null;
            IExplorerNode siteNode = e.Node;
            IExplorerSiteNodeInfo siteInfo = siteNode.Annotations.GetValue<IExplorerSiteNodeInfo>();
            if (siteInfo != null && siteInfo.IsConnectionRoot)
            {
                siteUrl = siteInfo.Url;
            }

            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.ListWebParts, true))
            {
                e.Node.ChildNodes.AddFolder(Resources.SiteNodeExtension_WebPartGalleryNodeName, Resources.WebPartsNode.ToBitmap(), CreateWebPartNodes);
            }
        }

        /// <summary>
        /// Create the web part nodes.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        private void CreateWebPartNodes(IExplorerNode parentNode)
        {
            FileNodeInfo[] webParts = parentNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo[]>(WebPartGallerySharePointCommandIds.GetWebParts);

            if (webParts != null)
            {
                foreach (FileNodeInfo webPart in webParts)
                {
                    var annotations = new Dictionary<object, object>
                    {
                        { typeof(FileNodeInfo), webPart }
                    };

                    IExplorerNode webPartNode = parentNode.ChildNodes.Add(ExplorerNodeIds.WebPartNode, webPart.Name, annotations);

                    if (webPart.IsCheckedOut)
                    {
                        webPartNode.Icon = Resources.WebPartNodeCheckedOut.ToBitmap();
                    }
                }
            }
        }

        #endregion
    }
}


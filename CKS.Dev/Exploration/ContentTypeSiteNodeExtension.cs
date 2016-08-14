using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Explorer;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    /// <summary>
    /// The site columns node extensions for the 'Site' node type.
    /// </summary>
    [Export(typeof(IExplorerNodeTypeExtension))]
    [ExplorerNodeType(ExplorerNodeTypes.SiteNode)]
    public class ContentTypeSiteNodeExtension : IExplorerNodeTypeExtension
    {
        /// <summary>
        /// Initializes the node extension.
        /// </summary>
        /// <param name="nodeType">The node type that is being extended.</param>
        public void Initialize(IExplorerNodeType nodeType)
        {
            nodeType.NodeChildrenRequested += new EventHandler<ExplorerNodeEventArgs>(nodeType_NodeChildrenRequested);
        }

        /// <summary>
        /// Handles the NodeChildrenRequested event of the nodeType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.SharePoint.Explorer.ExplorerNodeEventArgs"/> instance containing the event data.</param>
        void nodeType_NodeChildrenRequested(object sender, ExplorerNodeEventArgs e)
        {
            //Only perform this action if the site is not hanging off the connection node
            //The connection node already has a content types node
            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.ContentTypesGroupedView, true))
            {
                if (e.Node.ParentNode.NodeType.Id != ExplorerNodeTypes.ConnectionNode)
                {
                    e.Node.ChildNodes.AddFolder(Resources.ContentTypesSiteNodeExtension_ContentTypesNode, Resources.ContentTypes.ToBitmap(), AddContentTypeGroups);
                }
            }
        }

        /// <summary>
        /// Adds the content type groups.
        /// </summary>
        /// <param name="contentTypesFolder">The content types folder.</param>
        private void AddContentTypeGroups(IExplorerNode contentTypesFolder)
        {
            if (contentTypesFolder.ParentNode != null &&
                contentTypesFolder.ParentNode.NodeType.Name == ExplorerNodeTypes.SiteNode)
            {
                string[] contentTypeGroups = GetContentTypeGroups(contentTypesFolder);
                if (contentTypeGroups != null)
                {
                    foreach (string groupName in contentTypeGroups)
                    {
                        IExplorerNode contentTypeGroup = contentTypesFolder.ChildNodes.Add(ExplorerNodeIds.ContentTypeGroupNode, groupName, null, -1);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the content type groups.
        /// </summary>
        /// <param name="contentTypesFolder">The content types folder.</param>
        /// <returns></returns>
        private string[] GetContentTypeGroups(IExplorerNode contentTypesFolder)
        {
            if (contentTypesFolder == null)
            {
                throw new ArgumentNullException("contentTypesFolder");
            }

            return contentTypesFolder.Context.SharePointConnection.ExecuteCommand<string[]>(ContentTypeSharePointCommandIds.GetContentTypeGroups);
        }
    }
}
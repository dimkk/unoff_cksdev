using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Explorer;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Exploration;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    /// <summary>
    /// The content types node extension.
    /// </summary>
    [Export(typeof(IExplorerNodeTypeExtension))] 
    [ExplorerNodeType(ExtensionNodeTypes.ContentTypeNode)] 
    public class ContentTypesNodeExtension : IExplorerNodeTypeExtension
    {
        /// <summary>
        /// Initializes the node extension.
        /// </summary>
        /// <param name="nodeType">The node type that is being extended.</param>
        public void Initialize(IExplorerNodeType nodeType)
        {
            nodeType.NodeInitialized += new EventHandler<ExplorerNodeEventArgs>(nodeType_NodeInitialized);
        }

        /// <summary>
        /// Handles the NodeInitialized event of the nodeType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.SharePoint.Explorer.ExplorerNodeEventArgs"/> instance containing the event data.</param>
        void nodeType_NodeInitialized(object sender, ExplorerNodeEventArgs e)
        {
            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.ContentTypesGroupedView, true))
            {
                AddContentTypeGroups(e.Node.ParentNode);
            }
        }

        /// <summary>
        /// Adds the content type groups.
        /// </summary>
        /// <param name="contentTypesFolder">The content types folder.</param>
        private void AddContentTypeGroups(IExplorerNode contentTypesFolder)
        {
            if (contentTypesFolder.ParentNode != null &&
                contentTypesFolder.ParentNode.NodeType.Name == ExplorerNodeTypes.SiteNode &&
                contentTypesFolder.ChildNodes != null &&
                contentTypesFolder.ChildNodes.Count() == 1)
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

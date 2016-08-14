using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Explorer;
using CKS.Dev.VisualStudio.SharePoint.Exploration;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using Microsoft.VisualStudio.SharePoint;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    /// <summary>
    /// The content type group node type provider.
    /// </summary>
    [Export(typeof(IExplorerNodeTypeProvider))]
    [ExplorerNodeType(ExplorerNodeIds.ContentTypeGroupNode)]
    public class ContentTypeGroupNodeTypeProvider : IExplorerNodeTypeProvider
    {
        /// <summary>
        /// Initializes the new node type.
        /// </summary>
        /// <param name="typeDefinition">The definition of the new node type.</param>
        public void InitializeType(IExplorerNodeTypeDefinition typeDefinition)
        {
            typeDefinition.NodeChildrenRequested += new EventHandler<ExplorerNodeEventArgs>(typeDefinition_NodeChildrenRequested);
            typeDefinition.NodeMenuItemsRequested += new EventHandler<ExplorerNodeMenuItemsRequestedEventArgs>(typeDefinition_NodeMenuItemsRequested);
        }

        /// <summary>
        /// Handles the NodeChildrenRequested event of the typeDefinition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.SharePoint.Explorer.ExplorerNodeEventArgs"/> instance containing the event data.</param>
        void typeDefinition_NodeChildrenRequested(object sender, ExplorerNodeEventArgs e)
        {
            ContentTypeNodeInfo[] contentTypes = e.Node.Context.SharePointConnection.ExecuteCommand<string, ContentTypeNodeInfo[]>(ContentTypeSharePointCommandIds.GetContentTypesFromGroup, e.Node.Text);

            if (contentTypes != null)
            {
                foreach (IContentTypeNodeInfo contentType in contentTypes)
                {
                    e.Node.ChildNodes.Add(ExtensionNodeTypes.ContentTypeNode, contentType.Name, new Dictionary<object, object>
                    {
                        { typeof(IContentTypeNodeInfo), contentType }
                    });
                }
            }
        }

        /// <summary>
        /// Handles the NodeMenuItemsRequested event of the typeDefinition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.SharePoint.Explorer.ExplorerNodeMenuItemsRequestedEventArgs"/> instance containing the event data.</param>
        void typeDefinition_NodeMenuItemsRequested(object sender, ExplorerNodeMenuItemsRequestedEventArgs e)
        {
            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.ImportContentTypeGroup, true))
            {
                if (DTEManager.ActiveSharePointProject != null)
                {
                    IMenuItem importContentTypesMenuItem = e.MenuItems.Add(Resources.ContentTypeGroupNodeTypeProvider_ImportContentTypes);
                    importContentTypesMenuItem.Click += new EventHandler<MenuItemEventArgs>(importContentTypesMenuItem_Click);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the importContentTypesMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.SharePoint.MenuItemEventArgs"/> instance containing the event data.</param>
        void importContentTypesMenuItem_Click(object sender, MenuItemEventArgs e)
        {
            IExplorerNode contentTypeGroupNode = e.Owner as IExplorerNode;
            if (contentTypeGroupNode != null &&
                contentTypeGroupNode.ChildNodes != null &&
                contentTypeGroupNode.ChildNodes.Count() > 0)
            {
                foreach (IExplorerNode childNode in contentTypeGroupNode.ChildNodes)
                {
                    ContentTypeNodeExtension.ImportContentType(childNode);
                }
            }
        }
    }
}

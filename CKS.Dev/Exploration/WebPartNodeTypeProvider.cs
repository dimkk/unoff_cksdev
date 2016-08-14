using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Explorer;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;
using CKS.Dev.VisualStudio.SharePoint.Exploration;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using System.Diagnostics;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    /// <summary>
    /// Represents an extension of SharePoint nodes in Server Explorer for web parts. 
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(IExplorerNodeTypeProvider))]
    // Indicates that this class extends SharePoint nodes in Server Explorer.
    [ExplorerNodeType(ExplorerNodeIds.WebPartNode)]
    internal class WebPartNodeTypeProvider : FileNodeTypeProvider
    {
        #region Methods

        /// <summary>
        /// Initialise the node.
        /// </summary>
        /// <param name="typeDefinition">The node type.</param>
        public override void InitializeType(IExplorerNodeTypeDefinition typeDefinition)
        {
            base.InitializeType(typeDefinition);

            typeDefinition.DefaultIcon = Resources.WebPartNode.ToBitmap();
            typeDefinition.IsAlwaysLeaf = true;

            typeDefinition.NodePropertiesRequested += NodePropertiesRequested;
            typeDefinition.NodeMenuItemsRequested += NodeMenuItemsRequested;
        }

        /// <summary>
        /// Register the menu items for the web part.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The ExplorerNodeMenuItemsRequestedEventArgs object</param>
        private void NodeMenuItemsRequested(object sender, ExplorerNodeMenuItemsRequestedEventArgs e)
        {
            e.MenuItems.Add(Resources.WebPartNodeTypeProvider_Preview, 5).Click += WebPartNodeTypeProvider_PreviewClick;
            e.MenuItems.Add(Resources.WebPartNodeTypeProvider_Export, 4).Click += WebPartNodeTypeProvider_ExportClick;
        }

        /// <summary>
        /// Export the .webpart or .dwp file.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The MenuItemEventArgs object.</param>
        void WebPartNodeTypeProvider_ExportClick(object sender, MenuItemEventArgs e)
        {
            IMenuItem menuItem = sender as IMenuItem;

            IExplorerNode owner = (IExplorerNode)e.Owner;

            FileNodeInfo info = owner.Annotations.GetValue<FileNodeInfo>();
            
            if (info != null)
            {
                Process.Start(new Uri(owner.Context.SiteUrl + info.ServerRelativeUrl.TrimStart(@"/".ToCharArray())).AbsoluteUri);
            }
        }

        /// <summary>
        /// Preview the web part on the site.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The MenuItemEventArgs object.</param>
        void WebPartNodeTypeProvider_PreviewClick(object sender, MenuItemEventArgs e)
        {
            IMenuItem menuItem = sender as IMenuItem;

            IExplorerNode owner = (IExplorerNode)e.Owner;

            FileNodeInfo info = owner.Annotations.GetValue<FileNodeInfo>();

            if (info != null)
            {
                Process.Start(new Uri(owner.Context.SiteUrl + String.Format(Resources.WebPartNodeTypeProvider_PreviewUrlMask, info.Id.ToString())).AbsoluteUri);
            }
        }

        /// <summary>
        /// Retrieves properties that are displayed in the Properties window when
        /// a web part node is selected.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The ExplorerNodePropertiesRequestedEventArgs object</param>
        private void NodePropertiesRequested(object sender, ExplorerNodePropertiesRequestedEventArgs e)
        {
            IExplorerNode webPartNode = e.Node;
            FileNodeInfo webPart = webPartNode.Annotations.GetValue<FileNodeInfo>();
            Dictionary<string, string> masterPageProperties = webPartNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, Dictionary<string, string>>(WebPartSharePointCommandIds.GetWebPartProperties, webPart);
            object propertySource = webPartNode.Context.CreatePropertySourceObject(masterPageProperties);
            e.PropertySources.Add(propertySource);
        }

        #endregion
    }
}

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
    /// Represents an extension of SharePoint nodes in Server Explorer for themes. 
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(IExplorerNodeTypeProvider))]
    // Indicates that this class extends SharePoint nodes in Server Explorer.
    [ExplorerNodeType(ExplorerNodeIds.ThemeNode)]
    internal class ThemeNodeTypeProvider : FileNodeTypeProvider
    {
        #region Methods

        /// <summary>
        /// Initialise the node.
        /// </summary>
        /// <param name="typeDefinition">The node type.</param>
        public override void InitializeType(IExplorerNodeTypeDefinition typeDefinition)
        {
            base.InitializeType(typeDefinition);

            typeDefinition.DefaultIcon = Resources.ThemeNode.ToBitmap();
            typeDefinition.IsAlwaysLeaf = true;

            typeDefinition.NodePropertiesRequested += NodePropertiesRequested;
            typeDefinition.NodeMenuItemsRequested += NodeMenuItemsRequested;
        }

        /// <summary>
        /// Register the menu items for the theme.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The ExplorerNodeMenuItemsRequestedEventArgs object</param>
        private void NodeMenuItemsRequested(object sender, ExplorerNodeMenuItemsRequestedEventArgs e)
        {
            e.MenuItems.Add(Resources.ThemeNodeTypeProvider_Export, 4).Click += ThemeNodeTypeProvider_ExportClick;
        }

        /// <summary>
        /// Export the thmx file.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The MenuItemEventArgs object.</param>
        void ThemeNodeTypeProvider_ExportClick(object sender, MenuItemEventArgs e)
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
        /// Retrieves properties that are displayed in the Properties window when
        /// a theme node is selected.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The ExplorerNodePropertiesRequestedEventArgs object</param>
        private void NodePropertiesRequested(object sender, ExplorerNodePropertiesRequestedEventArgs e)
        {
            IExplorerNode themeNode = e.Node;
            FileNodeInfo theme = themeNode.Annotations.GetValue<FileNodeInfo>();
            Dictionary<string, string> masterPageProperties = themeNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, Dictionary<string, string>>(ThemeSharePointCommandIds.GetThemeProperties, theme);
            object propertySource = themeNode.Context.CreatePropertySourceObject(masterPageProperties);
            e.PropertySources.Add(propertySource);
        }

        #endregion
    }
}

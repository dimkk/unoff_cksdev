using CKSProperties = CKS.Dev.Core.Properties.Resources;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Explorer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
#if VS2012Build_SYMBOL
using CKS.Dev11.VisualStudio.SharePoint.Commands;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev11.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev11.VisualStudio.SharePoint.Environment;
#elif VS2013Build_SYMBOL
using CKS.Dev12.VisualStudio.SharePoint.Commands;
using CKS.Dev12.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev12.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev12.VisualStudio.SharePoint.Environment;
#elif VS2014Build_SYMBOL
using CKS.Dev13.VisualStudio.SharePoint.Commands;
using CKS.Dev13.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev13.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev13.VisualStudio.SharePoint.Environment;
#else
using CKS.Dev.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev.VisualStudio.SharePoint.Environment;
#endif

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Explorer
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Explorer
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Explorer
#else
namespace CKS.Dev.VisualStudio.SharePoint.Explorer
#endif
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

            typeDefinition.DefaultIcon = CKSProperties.WebPartNode.ToBitmap();
            typeDefinition.IsAlwaysLeaf = true;
        }

        /// <summary>
        /// Register the menu items for the web part.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The ExplorerNodeMenuItemsRequestedEventArgs object</param>
        protected override void NodeMenuItemsRequested(object sender, ExplorerNodeMenuItemsRequestedEventArgs e)
        {
            base.NodeMenuItemsRequested(sender, e);
            e.MenuItems.Add(CKSProperties.WebPartNodeTypeProvider_Preview, 5).Click += WebPartNodeTypeProvider_PreviewClick;
            e.MenuItems.Add(CKSProperties.WebPartNodeTypeProvider_Export, 4).Click += WebPartNodeTypeProvider_ExportClick;
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
                ProcessUtilities utils = new ProcessUtilities();
                utils.ExecuteBrowserUrlProcess(new Uri(owner.Context.SiteUrl + info.ServerRelativeUrl.TrimStart(@"/".ToCharArray())));
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
                ProcessUtilities utils = new ProcessUtilities();
                utils.ExecuteBrowserUrlProcess(new Uri(owner.Context.SiteUrl + String.Format(CKSProperties.WebPartNodeTypeProvider_PreviewUrlMask, info.Id.ToString())));
            }
        }

        /// <summary>
        /// Retrieves properties that are displayed in the Properties window when
        /// a web part node is selected.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The ExplorerNodePropertiesRequestedEventArgs object</param>
        protected override void NodePropertiesRequested(object sender, ExplorerNodePropertiesRequestedEventArgs e)
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

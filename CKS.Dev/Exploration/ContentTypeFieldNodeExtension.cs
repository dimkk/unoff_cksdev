using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Explorer;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;
using Microsoft.VisualStudio.SharePoint;
using System.Windows.Forms;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    /// <summary>
    /// Extend the site node for a Feature with functionality.
    /// </summary>
    [Export(typeof(IExplorerNodeTypeExtension))]
    //The node type to bind with
    [ExplorerNodeType(ExtensionNodeTypes.FieldNode)]
    public class ContentTypeFieldNodeExtension
        : IExplorerNodeTypeExtension
    {
        #region Methods

        /// <summary>
        /// Initialise the node and register its events.
        /// </summary>
        /// <param name="nodeType">The node type.</param>
        public void Initialize(IExplorerNodeType nodeType)
        {
            nodeType.NodeMenuItemsRequested += NodeType_NodeMenuItemsRequested;
                //delegate(object sender, ExplorerNodeMenuItemsRequestedEventArgs e)
                //{
                //    IMenuItem copyIDMenuItem = e.MenuItems.Add("Copy ID");
                //    copyIDMenuItem.Click +=
                //        delegate
                //        {
            //            IFieldNodeInfo nodeInfo = e.Node.Annotations.GetValue<IFieldNodeInfo>();
                //            Clipboard.SetData(DataFormats.Text, nodeInfo.Id.ToString("B"));
                //        };
                //};
        }

        /// <summary>
        /// Create the child nodes and register their events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The ExplorerNodeMenuItemsRequestedEventArgs.</param>
        void NodeType_NodeMenuItemsRequested(object sender, ExplorerNodeMenuItemsRequestedEventArgs e)
        {
            //Add the child nodes
            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.FieldCopyID, true))
            {
                IMenuItem copyIdItem = e.MenuItems.Add(Resources.ContentTypeFieldNodeExtension_CopyIdNodeName, 1);
                copyIdItem.Click += new EventHandler<MenuItemEventArgs>(CopyIdItem_Click);
            }
        }

        /// <summary>
        /// The copy id item click event copies the selected content type node id to the clipboard.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The MenuItemEventArgs.</param>
        void CopyIdItem_Click(object sender, MenuItemEventArgs e)
        {
            IExplorerNode owner = (IExplorerNode)e.Owner;
            IFieldNodeInfo annotation = owner.Annotations.GetValue<IFieldNodeInfo>();
            if (annotation.Id != null)
            {
                Clipboard.SetData(DataFormats.Text, annotation.Id.ToString("B"));
            }
        }


        #endregion
    }
}

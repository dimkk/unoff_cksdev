﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Explorer;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev.VisualStudio.SharePoint.Exploration;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using System.ComponentModel.Composition;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    /// <summary>
    /// Lsist Event Receiever list extension.
    /// </summary>
    [Export(typeof(IExplorerNodeTypeExtension))]
    [ExplorerNodeType(ExtensionNodeTypes.ListNode)]
    public class ListEventReceiversListExtension : IExplorerNodeTypeExtension
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
            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.ViewListEventReceivers, true))
            {
                e.Node.ChildNodes.AddFolder("Event Receivers", Properties.Resources.EventReceiver.ToBitmap(), CreateListEventReceiversNodes);
            }
        }

        /// <summary>
        /// Creates the list event receivers nodes.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        private void CreateListEventReceiversNodes(IExplorerNode parentNode)
        {
            IExplorerNode listNode = parentNode.ParentNode;
            IListNodeInfo listNodeInfo = listNode.Annotations.GetValue<IListNodeInfo>();
            EventReceiverInfo[] eventReceivers = listNode.Context.SharePointConnection.ExecuteCommand<Guid, EventReceiverInfo[]>(ListEventReceiversCommandIds.GetListEventReceivers, listNodeInfo.Id);

            foreach (EventReceiverInfo eventReceiver in eventReceivers)
            {
                Dictionary<object, object> annotations = new Dictionary<object, object>
                {
                    { typeof(EventReceiverInfo), eventReceiver }
                };

                parentNode.ChildNodes.Add(ExplorerNodeIds.ListEventReceiverNode, eventReceiver.Name, annotations);
            }
        }
    }
}

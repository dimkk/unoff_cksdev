﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using CKS.Dev11.VisualStudio.SharePoint.Commands;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev11.VisualStudio.SharePoint.Properties;
using Microsoft.VisualStudio.SharePoint.Explorer;

namespace CKS.Dev11.VisualStudio.SharePoint.Explorer
{
    /// <summary>
    /// Represents an extension of SharePoint page layouts nodes in Server Explorer 
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(IExplorerNodeTypeProvider))]
    // Indicates that this class extends SharePoint page layouts nodes in Server Explorer.
    [ExplorerNodeType(ExplorerNodeIds.PageLayoutNode)]
    internal class PageLayoutNodeTypeProvider : FileNodeTypeProvider
    {
        #region Methods

        /// <summary>
        /// Initializes the new node type.
        /// </summary>
        /// <param name="typeDefinition">The definition of the new node type.</param>
        public override void InitializeType(IExplorerNodeTypeDefinition typeDefinition)
        {
            base.InitializeType(typeDefinition);

            typeDefinition.DefaultIcon = Resources.PageNode.ToBitmap();
            typeDefinition.IsAlwaysLeaf = true;
        }

        /// <summary>
        /// Nodes the properties requested.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExplorerNodePropertiesRequestedEventArgs" /> instance containing the event data.</param>
        protected override void NodePropertiesRequested(object sender, ExplorerNodePropertiesRequestedEventArgs e)
        {
            IExplorerNode pageLayoutNode = e.Node;
            FileNodeInfo pageLayout = pageLayoutNode.Annotations.GetValue<FileNodeInfo>();
            Dictionary<string, string> properties = pageLayoutNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, Dictionary<string, string>>(MasterPageGallerySharePointCommandIds.GetMasterPagesOrPageLayoutPropertiesCommand, pageLayout);
            object propertySource = pageLayoutNode.Context.CreatePropertySourceObject(properties);
            e.PropertySources.Add(propertySource);
        }

        #endregion
    }
}

using System;
using System.ComponentModel.Composition;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Explorer;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using System.Collections.Generic;
using CKS.Dev.VisualStudio.SharePoint.Exploration;
using System.Diagnostics;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    /// <summary>
    /// Represents an extension of SharePoint site nodes in Server Explorer 
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(IExplorerNodeTypeExtension))]
    // Indicates that this class extends SharePoint site nodes in Server Explorer.
    [ExplorerNodeType(ExplorerNodeTypes.SiteNode)]
    internal class ThemeGallerySiteNodeExtension : IExplorerNodeTypeExtension
    {
        #region Fields

        /// <summary>
        /// The site url.
        /// </summary>
        private Uri siteUrl = null;

        #endregion

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
        /// Process the node children request to get the theme nodes.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The ExplorerNodeEventArgs object.</param>
        void nodeType_NodeChildrenRequested(object sender, ExplorerNodeEventArgs e)
        {
            IExplorerNode siteNode = e.Node;
            IExplorerSiteNodeInfo siteInfo = siteNode.Annotations.GetValue<IExplorerSiteNodeInfo>();
            if (siteInfo != null && siteInfo.IsConnectionRoot)
            {
                siteUrl = siteInfo.Url;
            }

            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.ListThemes, true))
            {
                e.Node.ChildNodes.AddFolder(Resources.SiteNodeExtension_ThemeGalleryNodeName, Resources.ThemesNode.ToBitmap(), CreateThemeNodes);
            }
        }

        /// <summary>
        /// Create the themes nodes.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        private void CreateThemeNodes(IExplorerNode parentNode)
        {
            FileNodeInfo[] themes = parentNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo[]>(ThemeGallerySharePointCommandIds.GetThemes);

            if (themes != null)
            {
                foreach (FileNodeInfo theme in themes)
                {
                    var annotations = new Dictionary<object, object>
                    {
                        { typeof(FileNodeInfo), theme }
                    };

                    IExplorerNode webPartNode = parentNode.ChildNodes.Add(ExplorerNodeIds.ThemeNode, theme.Name, annotations);

                    if (theme.IsCheckedOut)
                    {
                        webPartNode.Icon = Resources.ThemeNodeCheckedOut.ToBitmap();
                    }
                }
            }
        }

        #endregion
    }
}

using CKS.Dev11.VisualStudio.SharePoint.Commands;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev11.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev11.VisualStudio.SharePoint.Properties;
using Microsoft.VisualStudio.SharePoint.Explorer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace CKS.Dev11.VisualStudio.SharePoint.Explorer
{
    /// <summary>
    /// Represents an extension of SharePoint site nodes in Server Explorer 
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(IExplorerNodeTypeExtension))]
    // Indicates that this class extends SharePoint site nodes in Server Explorer.
    [ExplorerNodeType(ExplorerNodeTypes.SiteNode)]
    internal class WebTemplateSiteNodeExtension : IExplorerNodeTypeExtension
    {
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
            Uri siteUrl = null;
            IExplorerNode siteNode = e.Node;
            IExplorerSiteNodeInfo siteInfo = siteNode.Annotations.GetValue<IExplorerSiteNodeInfo>();
            if (siteInfo != null && siteInfo.IsConnectionRoot)
            {
                siteUrl = siteInfo.Url;
            }

            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.ListThemes, true))
            {
                e.Node.ChildNodes.AddFolder(Resources.SiteNodeExtension_WebTemplatesNodeName, Resources.WebTemplatesNode.ToBitmap(), CreateWebTemplateCategories);
            }
        }

        /// <summary>
        /// Get the catergories for the web templates.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        private void CreateWebTemplateCategories(IExplorerNode parentNode)
        {
            //Get the categories which
            string[] categories = parentNode.Context.SharePointConnection.ExecuteCommand<string[]>(WebTemplateCollectionSharePointCommandIds.GetWebTemplateCategories);

            foreach (var item in categories)
            {
                parentNode.ChildNodes.AddFolder(item, Resources.WebTemplateCategoryNode.ToBitmap(), CreateWebTemplateNodes);
            }
        }

        /// <summary>
        /// Create the themes nodes.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        private void CreateWebTemplateNodes(IExplorerNode parentNode)
        {
            WebTemplateInfo[] webTemplates = parentNode.Context.SharePointConnection.ExecuteCommand<string, WebTemplateInfo[]>(WebTemplateCollectionSharePointCommandIds.GetAvailableWebTemplatesByCategory, parentNode.Text);

            if (webTemplates != null)
            {
                foreach (WebTemplateInfo webTemplate in webTemplates)
                {
                    var annotations = new Dictionary<object, object>
                    {
                        { typeof(WebTemplateInfo), webTemplate }
                    };

                    IExplorerNode webPartNode = parentNode.ChildNodes.Add(ExplorerNodeIds.WebTemplateNode, webTemplate.Name, annotations);
                }
            }
        }

        #endregion
    }
}

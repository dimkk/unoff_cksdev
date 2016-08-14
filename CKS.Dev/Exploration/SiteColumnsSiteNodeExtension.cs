using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Explorer;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    /// <summary>
    /// The site columns node extensions for the 'Site' node type.
    /// </summary>
    [Export(typeof(IExplorerNodeTypeExtension))]
    [ExplorerNodeType(ExplorerNodeTypes.SiteNode)]
    public class SiteColumnsSiteNodeExtension : IExplorerNodeTypeExtension
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
            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.SiteColumnsGroupedView, true))
            {
                e.Node.ChildNodes.AddFolder(Resources.SiteColumnsSiteNodeExtension_SiteColumnsNode, Resources.SiteColumns.ToBitmap(), AddSiteColumnsGroups);
            }
        }

        /// <summary>
        /// Adds the site columns groups.
        /// </summary>
        /// <param name="siteColumnsNode">The site columns node.</param>
        void AddSiteColumnsGroups(IExplorerNode siteColumnsNode)
        {
            string[] siteColumnsGroups = GetSiteColumnsGroups(siteColumnsNode);
            if (siteColumnsGroups != null)
            {
                foreach (string groupName in siteColumnsGroups)
                {
                    IExplorerNode contentTypeGroup = siteColumnsNode.ChildNodes.Add(ExplorerNodeIds.SiteColumnsGroupNode, groupName, null, -1);
                }
            }
        }

        /// <summary>
        /// Gets the site columns groups.
        /// </summary>
        /// <param name="siteColumnsNode">The site columns node.</param>
        /// <returns></returns>
        private string[] GetSiteColumnsGroups(IExplorerNode siteColumnsNode)
        {
            if (siteColumnsNode == null)
            {
                throw new ArgumentNullException("siteColumnsNode");
            }

            return siteColumnsNode.Context.SharePointConnection.ExecuteCommand<string[]>(SiteColumnsSharePointCommandIds.GetSiteColumnsGroups);
        }
    }
}

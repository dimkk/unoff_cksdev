using CKSProperties = CKS.Dev.Core.Properties.Resources;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Explorer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

#if VS2012Build_SYMBOL
using CKS.Dev11.VisualStudio.SharePoint.Commands;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev11.VisualStudio.SharePoint.Environment;
using CKS.Dev11.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev11.VisualStudio.SharePoint.Explorer.Dialogs;
#elif VS2013Build_SYMBOL
using CKS.Dev12.VisualStudio.SharePoint.Commands;
using CKS.Dev12.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev12.VisualStudio.SharePoint.Environment;
using CKS.Dev12.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev12.VisualStudio.SharePoint.Explorer.Dialogs;
#elif VS2014Build_SYMBOL
using CKS.Dev13.VisualStudio.SharePoint.Commands;
using CKS.Dev13.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev13.VisualStudio.SharePoint.Environment;
using CKS.Dev13.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev13.VisualStudio.SharePoint.Explorer.Dialogs;
#else
using CKS.Dev.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev.VisualStudio.SharePoint.Environment;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev.VisualStudio.SharePoint.Explorer.Dialogs;
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
    /// Represents an extension of SharePoint site nodes in Server Explorer 
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(IExplorerNodeTypeExtension))]
    // Indicates that this class extends SharePoint site nodes in Server Explorer.
    [ExplorerNodeType(ExplorerNodeTypes.SiteNode)]
    internal class SiteNodeExtension : IExplorerNodeTypeExtension
    {
        #region Methods

        /// <summary>
        /// Initializes the node extension.
        /// </summary>
        /// <param name="nodeType">The node type that is being extended.</param>
        public void Initialize(IExplorerNodeType nodeType)
        {
            //Bind the events
            nodeType.NodeChildrenRequested += NodeChildrenRequested;
            nodeType.NodeMenuItemsRequested += NodeMenuItemsRequested;
            nodeType.NodePropertiesRequested += NodePropertiesRequested;
        }


        /// <summary>
        /// Nodes the children requested.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExplorerNodeEventArgs" /> instance containing the event data.</param>
        void NodeChildrenRequested(object sender, ExplorerNodeEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Nodes the menu items requested.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExplorerNodeMenuItemsRequestedEventArgs" /> instance containing the event data.</param>
        void NodeMenuItemsRequested(object sender, ExplorerNodeMenuItemsRequestedEventArgs e)
        {
            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.DeveloperDashboardSettings, true))
            {
                e.MenuItems.Add(CKSProperties.SiteNodeExtension_DeveloperDashboardSettings).Click += SiteNodeExtension_Click;
            }

            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.OpenInSharePointDesigner, true))
            {
                IMenuItem item = e.MenuItems.Add(CKSProperties.SiteNodeExtension_OpenInSPD);
                item.Click += delegate
                {
                    IExplorerSiteNodeInfo nodeInfo = e.Node.Annotations.GetValue<IExplorerSiteNodeInfo>();
                    string url = nodeInfo.Url.ToString();
                    OpenInSharePointDesigner(url);
                };
            }
        }

        /// <summary>
        /// Handles the Click event of the SiteNodeExtension control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MenuItemEventArgs" /> instance containing the event data.</param>
        void SiteNodeExtension_Click(object sender, MenuItemEventArgs e)
        {
            IExplorerNode owner = (IExplorerNode)e.Owner;

            string level = owner.Context.SharePointConnection.ExecuteCommand<string>(DeveloperDashboardCommandIds.GetDeveloperDashBoardDisplayLevelSetting);


            DeveloperDashboardSettingsDialog frm = new DeveloperDashboardSettingsDialog();
            frm.SelectedLevel = level;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                owner.Context.SharePointConnection.ExecuteCommand(DeveloperDashboardCommandIds.SetDeveloperDashBoardDisplayLevelSetting, frm.SelectedLevel);
            }
        }

        /// <summary>
        /// Opens the in SharePoint designer.
        /// </summary>
        /// <param name="url">The URL.</param>
        void OpenInSharePointDesigner(string url)
        {
            string path = String.Empty;
            
            SharePointVersion version = ProjectUtilities.WhichSharePointVersionIsProjectDeployingTo();
            if (version == SharePointVersion.SP2010)
            {
                path = Path.Combine(ProjectUtilities.GetSharePoint14DesignerInstallRoot(),
                    "SPDesign.exe");
            }
            else if (version == SharePointVersion.SP2013)
            {
                path = Path.Combine(ProjectUtilities.GetSharePoint15DesignerInstallRoot(),
                   "SPDesign.exe");
            }

            ProcessUtilities utils = new ProcessUtilities();
            if (!String.IsNullOrEmpty(path))
            {
                if (File.Exists(path))
                {
                    utils.StartProcess(DTEManager.ActiveSharePointProject, path, url);
                }
            }
        }

        /// <summary>
        /// Nodes the properties requested.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExplorerNodePropertiesRequestedEventArgs" /> instance containing the event data.</param>
        void NodePropertiesRequested(object sender, ExplorerNodePropertiesRequestedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
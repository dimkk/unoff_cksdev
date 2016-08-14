using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Explorer;
using Microsoft.VisualStudio.SharePoint;
using CKS.Dev.VisualStudio.SharePoint.Exploration.Dialogs;
using System.Windows.Forms;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using CKS.Dev.VisualStudio.SharePoint.Properties;
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
    internal class SiteNodeExtension : IExplorerNodeTypeExtension
    {
        public void Initialize(IExplorerNodeType nodeType)
        {
            //Bind the events
            nodeType.NodeChildrenRequested += NodeChildrenRequested;
            nodeType.NodeMenuItemsRequested += NodeMenuItemsRequested;
            nodeType.NodePropertiesRequested += NodePropertiesRequested;
        }


        void NodeChildrenRequested(object sender, ExplorerNodeEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void NodeMenuItemsRequested(object sender, ExplorerNodeMenuItemsRequestedEventArgs e)
        {
            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.DeveloperDashboardSettings, true))
            {
                e.MenuItems.Add(Resources.SiteNodeExtension_DeveloperDashboardSettings).Click += SiteNodeExtension_Click;
            }

            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.OpenInSharePointDesigner, true))
            {
                IMenuItem item = e.MenuItems.Add(Resources.SiteNodeExtension_OpenInSPD);
                item.Click += delegate
                {
                    IExplorerSiteNodeInfo nodeInfo = e.Node.Annotations.GetValue<IExplorerSiteNodeInfo>();
                    string url = nodeInfo.Url.ToString();
                    OpenInSharePointDesigner(url);
                };
            }
        }

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
        /// Opens the in share point designer.
        /// </summary>
        /// <param name="url">The URL.</param>
        void OpenInSharePointDesigner(string url)
        {
            string path = Path.Combine(ProjectUtilities.GetSharePointDesignerInstallRoot(),
                "SPDesign.exe");

            if (File.Exists(path))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(
                    path, url);
                Process.Start(startInfo);
            }
        }

        void NodePropertiesRequested(object sender, ExplorerNodePropertiesRequestedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}

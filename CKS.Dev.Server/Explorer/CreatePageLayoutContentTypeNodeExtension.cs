using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Explorer;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;
using Microsoft.VisualStudio.SharePoint;
using System.Globalization;
using System.Text.RegularExpressions;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Exploration.Utilities;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    [Export(typeof(IExplorerNodeTypeExtension))]
    [ExplorerNodeType(ExtensionNodeTypes.ContentTypeNode)]
    public class CreatePageLayoutContentTypeNodeExtension : IExplorerNodeTypeExtension
    {
        public void Initialize(IExplorerNodeType nodeType)
        {
            nodeType.NodeMenuItemsRequested += new EventHandler<ExplorerNodeMenuItemsRequestedEventArgs>(nodeType_NodeMenuItemsRequested);
        }

        void nodeType_NodeMenuItemsRequested(object sender, ExplorerNodeMenuItemsRequestedEventArgs e)
        {
            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.CreatePageLayoutFromContentType, true))
            {
                IMenuItem createPageLayoutMenuItem = e.MenuItems.Add("Create Page Layout");
                createPageLayoutMenuItem.Click += new EventHandler<Microsoft.VisualStudio.SharePoint.MenuItemEventArgs>(CreatePageLayoutContentTypeNodeExtension_Click);

                IContentTypeNodeInfo ctInfo = e.Node.Annotations.GetValue<IContentTypeNodeInfo>();
                createPageLayoutMenuItem.IsEnabled = e.Node.Context.SharePointConnection.ExecuteCommand<string, bool>(ContentTypeSharePointCommandIds.IsPublishingContentTypeCommand, ctInfo.Name);
            }
        }

        void CreatePageLayoutContentTypeNodeExtension_Click(object sender, Microsoft.VisualStudio.SharePoint.MenuItemEventArgs e)
        {
            IExplorerNode ctNode = e.Owner as IExplorerNode;

            if (ctNode != null)
            {
                IContentTypeNodeInfo ctInfo = ctNode.Annotations.GetValue<IContentTypeNodeInfo>();

                string pageLayoutContents = ctNode.Context.SharePointConnection.ExecuteCommand<string, string>(ContentTypeSharePointCommandIds.CreatePageLayoutCommand, ctInfo.Name);
                DTEManager.CreateNewTextFile(SafeContentTypeName(ctInfo.Name) + ".aspx", pageLayoutContents);
            }
        }

        internal static string SafeContentTypeName(string contentTypeName)
        {
            string safeName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(contentTypeName);
            safeName = new Regex(@"[^\w]").Replace(safeName, "");
            return safeName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Explorer;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev.VisualStudio.SharePoint.Exploration.Utilities;
using CKS.Dev.VisualStudio.SharePoint.Commands;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    [Export(typeof(IExplorerNodeTypeProvider))]
    [ExplorerNodeType(PublishingPageNodeTypeProvider.PublishingPageNodeTypeId)]
    public class PublishingPageNodeTypeProvider : IExplorerNodeTypeProvider
    {
        public const string PublishingPageNodeTypeId = "CKS.Dev.SharePoint.Explorer.PublishingPage";

        public void InitializeType(IExplorerNodeTypeDefinition typeDefinition)
        {
            typeDefinition.IsAlwaysLeaf = true;
            typeDefinition.DefaultIcon = Properties.Resources.PageNode.ToBitmap();
            typeDefinition.NodePropertiesRequested += new EventHandler<ExplorerNodePropertiesRequestedEventArgs>(typeDefinition_NodePropertiesRequested);
            typeDefinition.NodeMenuItemsRequested += new EventHandler<ExplorerNodeMenuItemsRequestedEventArgs>(typeDefinition_NodeMenuItemsRequested);
        }

        void typeDefinition_NodeMenuItemsRequested(object sender, ExplorerNodeMenuItemsRequestedEventArgs e)
        {
            IMenuItem exportMenuItem = e.MenuItems.Add("Export");
            exportMenuItem.Click += new EventHandler<MenuItemEventArgs>(exportMenuItem_Click);

            IMenuItem getTemplatePageMenuItem = e.MenuItems.Add("Get Template Page");
            getTemplatePageMenuItem.Click += new EventHandler<MenuItemEventArgs>(getTemplatePageMenuItem_Click);
        }

        void getTemplatePageMenuItem_Click(object sender, MenuItemEventArgs e)
        {
            DTEManager.CreateNewTextFile("TemplatePage.aspx", Properties.Resources.TemplatePage);
        }

        void exportMenuItem_Click(object sender, MenuItemEventArgs e)
        {
            IExplorerNode pageNode = e.Owner as IExplorerNode;
            if (pageNode != null)
            {
                PublishingPageInfo pageInfo = pageNode.Annotations.GetValue<PublishingPageInfo>();
                if (pageInfo != null)
                {
                    string pageXml = pageNode.Context.SharePointConnection.ExecuteCommand<PublishingPageInfo, string>(PublishingPageCommandIds.ExportToXml, pageInfo);
                    DTEManager.CreateNewTextFile(String.Format("{0}.xml", pageInfo.Name), pageXml);
                }
            }
        }

        void typeDefinition_NodePropertiesRequested(object sender, ExplorerNodePropertiesRequestedEventArgs e)
        {
            IExplorerNode pageNode = e.Node;
            if (pageNode != null)
            {
                IDictionary<string, string> publishingPageProperties = pageNode.Context.SharePointConnection.ExecuteCommand<PublishingPageInfo, Dictionary<string, string>>(PublishingPageCommandIds.GetProperties, pageNode.Annotations.GetValue<PublishingPageInfo>());
                object propertySource = e.Node.Context.CreatePropertySourceObject(publishingPageProperties);
                e.PropertySources.Add(propertySource);
            }
        }

        internal static void CreatePublishingPageNodes(IExplorerNode pagesFolder)
        {
            List<PublishingPageInfo> publishingPages = pagesFolder.Context.SharePointConnection.ExecuteCommand<List<PublishingPageInfo>>(SiteCommandIds.GetPublishingPagesCommandId);
            foreach (PublishingPageInfo publishingPage in publishingPages)
            {
                CreateNode(pagesFolder, publishingPage);
            }
        }

        public static IExplorerNode CreateNode(IExplorerNode parentNode, PublishingPageInfo publishingPage)
        {
            return parentNode.ChildNodes.Add(PublishingPageNodeTypeProvider.PublishingPageNodeTypeId,
                String.IsNullOrEmpty(publishingPage.Name) ? publishingPage.Title : publishingPage.Name,
                new Dictionary<object, object> {
                    { typeof(PublishingPageInfo), publishingPage }
                });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Explorer;
using Microsoft.VisualStudio.SharePoint;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
using System.Xml.Linq;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using System.IO;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev11.VisualStudio.SharePoint.Commands;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    [Export(typeof(IExplorerNodeTypeExtension))]
    [ExplorerNodeType(Microsoft.VisualStudio.SharePoint.Explorer.Extensions.ExtensionNodeTypes.FieldNode)]
    internal class FieldNodeExtension : IExplorerNodeTypeExtension
    {
        public void Initialize(IExplorerNodeType nodeType)
        {
            nodeType.NodeMenuItemsRequested += new EventHandler<ExplorerNodeMenuItemsRequestedEventArgs>(nodeType_NodeMenuItemsRequested);

            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.GetFieldProperties, true))
            {
                nodeType.NodePropertiesRequested += new EventHandler<ExplorerNodePropertiesRequestedEventArgs>(nodeType_NodePropertiesRequested);
            }
        }

        void nodeType_NodePropertiesRequested(object sender, ExplorerNodePropertiesRequestedEventArgs e)
        {
            try
            {
                IFieldNodeInfo info = e.Node.Annotations.GetValue<IFieldNodeInfo>();
                if (info.ListId == Guid.Empty && String.IsNullOrEmpty(info.ContentTypeName))
                {
                    IDictionary<string, string> fieldProperties = e.Node.Context.SharePointConnection.ExecuteCommand<FieldNodeInfo, Dictionary<string, string>>(SiteColumnsSharePointCommandIds.GetProperties, info as FieldNodeInfo);
                    object propertySource = e.Node.Context.CreatePropertySourceObject(fieldProperties);
                    e.PropertySources.Add(propertySource);
                }
            }
            catch (Exception ex)
            {
                DTEManager.ProjectService.Logger.WriteLine("EXCEPTION: " + ex.Message, LogCategory.Error);
            }
        }

        void nodeType_NodeMenuItemsRequested(object sender, ExplorerNodeMenuItemsRequestedEventArgs e)
        {
            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.ImportField, true))
            {
                if (DTEManager.ActiveSharePointProject != null)
                {
                    IMenuItem importFieldMenuItem = e.MenuItems.Add(Resources.FieldNodeExtension_ImportNodeName);
                    importFieldMenuItem.Click += new EventHandler<MenuItemEventArgs>(importFieldMenuItem_Click);
                }
            }
        }

        void importFieldMenuItem_Click(object sender, MenuItemEventArgs e)
        {
            IExplorerNode node = e.Owner as IExplorerNode;
            if (node != null)
            {
                ImportField(node);
            }
        }

        public static void ImportField(IExplorerNode fieldNode)
        {
            if (fieldNode == null)
            {
                throw new ArgumentNullException("fieldNode");
            }

            Microsoft.VisualStudio.SharePoint.Explorer.Extensions.IFieldNodeInfo nodeInfo = fieldNode.Annotations.GetValue<Microsoft.VisualStudio.SharePoint.Explorer.Extensions.IFieldNodeInfo>();
            if (nodeInfo != null)
            {
                FieldNodeInfo fieldNodeInfo = new FieldNodeInfo
                {
                    ContentTypeName = nodeInfo.ContentTypeName,
                    Id = nodeInfo.Id,
                    IsHidden = nodeInfo.IsHidden,
                    ListId = nodeInfo.ListId,
                    Title = nodeInfo.Title
                };
                Dictionary<string, string> fieldProperties = null;

                if (String.IsNullOrEmpty(fieldNodeInfo.ContentTypeName) && fieldNodeInfo.ListId == Guid.Empty)
                {
                    fieldProperties = fieldNode.Context.SharePointConnection.ExecuteCommand<FieldNodeInfo, Dictionary<string, string>>(SiteColumnsSharePointCommandIds.GetProperties, fieldNodeInfo);
                }
                else
                {
                    fieldProperties = fieldNode.Context.SharePointConnection.ExecuteCommand<FieldNodeInfo, Dictionary<string, string>>(FieldSharePointCommandIds.GetProperties, fieldNodeInfo);
                }

                if (fieldProperties != null)
                {
                    XNamespace xn = XNamespace.Get("http://schemas.microsoft.com/sharepoint/");
                    XElement xElements = new XElement(xn + "Elements",
                        XElement.Parse(fieldProperties["SchemaXml"]));

                    EnvDTE.Project activeProject = DTEManager.ActiveProject;
                    if (activeProject != null)
                    {
                        ISharePointProjectService projectService = fieldNode.ServiceProvider.GetService(typeof(ISharePointProjectService)) as ISharePointProjectService;
                        ISharePointProject activeSharePointProject = projectService.Projects[activeProject.FullName];
                        if (activeSharePointProject != null)
                        {
                            ISharePointProjectItem fieldProjectItem = activeSharePointProject.ProjectItems.Add(fieldProperties["InternalName"], "Microsoft.VisualStudio.SharePoint.Field");
                            System.IO.File.WriteAllText(Path.Combine(fieldProjectItem.FullPath, "Elements.xml"), xElements.ToString().Replace("xmlns=\"\"", String.Empty));
                            ISharePointProjectItemFile elementsXml = fieldProjectItem.Files.AddFromFile("Elements.xml");
                            elementsXml.DeploymentType = DeploymentType.ElementManifest;
                            elementsXml.DeploymentPath = String.Format(@"{0}\", fieldProperties["InternalName"]);
                            fieldProjectItem.DefaultFile = elementsXml;
                        }
                    }
                }
            }
        }
    }
}

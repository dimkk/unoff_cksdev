using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Commands;
using Microsoft.SharePoint;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Properties;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
using System.Xml;
using CKS.Dev.VisualStudio.SharePoint.Exploration;

namespace CKS.Dev11.VisualStudio.SharePoint.Commands
{
    /// <summary>
    /// The content type commands.
    /// </summary>
    class ContentTypeSharePointCommands
    {
        #region Methods

        /// <summary>
        /// Get the content type Id for the supplied name
        /// </summary>
        /// <param name="context">The ISharePointCommandContext.</param>
        /// <param name="name">The content type name.</param>
        /// <returns>The Id of the content type.</returns>
        [SharePointCommand(ContentTypeSharePointCommandIds.GetContentTypeID)]
        private static string GetContentTypeID(ISharePointCommandContext context, string name)
        {
            SPContentType type = context.Web.AvailableContentTypes[name];
            if (type == null)
            {
                context.Logger.WriteLine(String.Format(Resources.ContentTypeSharePointCommands_GetContentTypeIDException, name), LogCategory.Error);
                return String.Empty;
            }
            return type.Id.ToString();
        }

        /// <summary>
        /// Checks whether the content type is one of the built in ones.
        /// </summary>
        /// <param name="context">The ISharePointCommandContext.</param>
        /// <param name="contentTypeName">The name of the content type.</param>
        /// <returns>True if the content type is a built in one.</returns>
        [SharePointCommand(ContentTypeSharePointCommandIds.IsBuiltInContentType)]
        private static bool IsBuiltInContentType(ISharePointCommandContext context, string contentTypeName)
        {
            SPContentType contentType = context.Web.AvailableContentTypes[contentTypeName];

            if (contentType.Id == SPBuiltInContentTypeId.AdminTask)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Announcement)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.BasicPage)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.BlogComment)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.BlogPost)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.CallTracking)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Contact)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Discussion)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Document)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.DocumentSet)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.DocumentWorkflowItem)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.DomainGroup)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.DublinCoreName)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Event)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.FarEastContact)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Folder)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.GbwCirculationCTName)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.GbwOfficialNoticeCTName)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.HealthReport)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.HealthRuleDefinition)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Holiday)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.IMEDictionaryItem)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Issue)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Item)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Link)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.LinkToDocument)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.MasterPage)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Message)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.ODCDocument)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Person)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Picture)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Resource)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.ResourceGroup)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.ResourceReservation)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.RootOfList)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Schedule)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.ScheduleAndResourceReservation)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.SharePointGroup)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.SummaryTask)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.System)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Task)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Timecard)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.UDCDocument)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.UntypedDocument)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.WebPartPage)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.WhatsNew)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.Whereabouts)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.WikiDocument)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.WorkflowHistory)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.WorkflowTask)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.XMLDocument)
            {
                return true;
            }
            if (contentType.Id == SPBuiltInContentTypeId.XSLStyle)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the import properties for the supplied content type.
        /// </summary>
        /// <param name="context">The ISharePointCommandContext.</param>
        /// <param name="contentTypeName">The name of the content type.</param>
        /// <returns>The import properties.</returns>
        [SharePointCommand(ContentTypeSharePointCommandIds.GetContentTypeImportProperties)]
        private static ContentTypeInfo GetContentTypeImportProperties(ISharePointCommandContext context,
            string contentTypeName)
        {
            SPContentType contentType = context.Web.AvailableContentTypes[contentTypeName];
            if (contentType == null)
            {
                context.Logger.WriteLine(String.Format(Resources.ContentTypeSharePointCommands_GetContentTypePropertiesException, contentTypeName), LogCategory.Error);
                return null;
            }

            ContentTypeInfo info = new ContentTypeInfo();

            if (contentType != null)
            {
                info.Id = contentType.Id.ToString();
                info.Name = contentType.Name;
                info.Description = contentType.Description;
                info.Group = contentType.Group;
                info.Sealed = contentType.Sealed;
                info.Hidden = contentType.Hidden;
                info.ReadOnly = contentType.ReadOnly;

                SPContentType parentContentType = contentType.Parent;

                List<ContentTypeInfo.FieldRef> fields = new List<ContentTypeInfo.FieldRef>();
                foreach (SPField field in contentType.Fields)
                {
                    // check if the field is in the parent content type, skip it...
                    if (parentContentType.Fields.ContainsField(field.StaticName))
                    {
                        continue;
                    }

                    ContentTypeInfo.FieldRef fieldRef = new ContentTypeInfo.FieldRef();
                    fieldRef.Id = field.Id.ToString();
                    fieldRef.Name = field.StaticName;
                    fieldRef.DisplayName = field.Title;

                    fields.Add(fieldRef);
                }

                if (fields.Count > 0)
                {
                    info.FieldRefs = fields.ToArray();
                }

                if (!String.IsNullOrEmpty(contentType.DocumentTemplate))
                {
                    info.DocumentTemplate = contentType.DocumentTemplate;
                }

                if (contentType.XmlDocuments != null && contentType.XmlDocuments.Count > 0)
                {
                    List<string> xmlDocs = new List<string>();
                    foreach (string doc in contentType.XmlDocuments)
                    {
                        xmlDocs.Add(doc);
                    }
                    info.XmlDocuments = xmlDocs.ToArray();
                }
            }

            return info;
        }

        /// <summary>
        /// Gets the content type groups.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The content type groups.</returns>
        [SharePointCommand(ContentTypeSharePointCommandIds.GetContentTypeGroups)]
        private static string[] GetContentTypeGroups(ISharePointCommandContext context)
        {
            SPContentTypeCollection contentTypes = context.Web.AvailableContentTypes;
            IEnumerable<string> allContentTypeGroups = (from SPContentType contentType
                                                        in contentTypes
                                                        select contentType.Group);

            string[] contentTypeGroups = allContentTypeGroups.Distinct().ToArray();

            return contentTypeGroups;
        }

        /// <summary>
        /// Gets the content types from group.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="groupName">Name of the group.</param>
        /// <returns>The content type node infos for the group.</returns>
        [SharePointCommand(ContentTypeSharePointCommandIds.GetContentTypesFromGroup)]
        private static ContentTypeNodeInfo[] GetContentTypesFromGroup(ISharePointCommandContext context, string groupName)
        {
            SPContentTypeCollection contentTypes = context.Web.AvailableContentTypes;
            ContentTypeNodeInfo[] contentTypesFromGroup = (from SPContentType contentType
                                                           in contentTypes
                                                           where contentType.Group == groupName
                                                           select new ContentTypeNodeInfo { Name = contentType.Name }).ToArray();

            return contentTypesFromGroup;
        }

        #endregion
    }
}

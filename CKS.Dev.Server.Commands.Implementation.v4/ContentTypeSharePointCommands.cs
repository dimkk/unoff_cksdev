using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Fields;
using Microsoft.SharePoint.Publishing.WebControls;
using Microsoft.SharePoint.WebControls;
using Microsoft.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Commands.Extensions;

namespace CKS.Dev.VisualStudio.SharePoint.Commands
{
    class ContentTypeSharePointCommands
    {
        [SharePointCommand(ContentTypeSharePointCommandIds.CreatePageLayoutCommand)]
        private static string CreatePageLayout(ISharePointCommandContext context, string contentTypeName)
        {
            string pageLayout = String.Empty;

            try
            {
                SPContentType contentType = context.Web.ContentTypes[contentTypeName];

                Dictionary<string, TagInfo> assemblies = new Dictionary<string, TagInfo>();
                List<FieldInfo> fields = new List<FieldInfo>(contentType.Fields.Count);
                int tagCount = 0; // counter for custom tags

                foreach (SPField field in contentType.Fields)
                {
                    if (field.Hidden)
                    {
                        continue;
                    }

                    Type t = field.GetFieldRenderingControlType();
                    TagInfo tag = null;
                    if (assemblies.ContainsKey(t.Namespace))
                    {
                        tag = assemblies[t.Namespace];
                    }
                    else
                    {
                        if (BuiltInTags.ContainsKey(t.Namespace))
                        {
                            tag = BuiltInTags[t.Namespace];
                        }
                        else
                        {
                            tag = new TagInfo("CustomTag_" + tagCount++, t.Assembly.FullName, t.Namespace);
                        }
                        assemblies.Add(t.Namespace, tag);
                    }

                    fields.Add(new FieldInfo(field.InternalName, field.Title, field.Description, String.Format("<{{0}}:{0} FieldName=\"{1}\" InputFieldLabel=\"{2}\" runat=\"server\"></{{0}}:{0}>", t.Name, field.InternalName, field.Title), tag, field));
                }

                pageLayout = WritePageLayoutContents(assemblies, fields);
            }
            catch { }

            return pageLayout;
        }

        [SharePointCommand(ContentTypeSharePointCommandIds.IsPublishingContentTypeCommand)]
        private static bool IsPublishingContentType(ISharePointCommandContext context, string contentTypeName)
        {
            bool isPublishingContentType = false;

            SPContentType contentType = context.Web.ContentTypes[contentTypeName];
            isPublishingContentType = contentType.Id.IsChildOf(ContentTypeId.Page);

            return isPublishingContentType;
        }

        #region Helper methods
        internal static XNamespace sp = XNamespace.Get("http://schemas.microsoft.com/sharepoint/");

        internal static readonly Dictionary<String, String> TagPrefixMappings = new Dictionary<string, string>
		{
			{ "Microsoft.SharePoint.WebControls", "SharePointWebControls" },
			{ "Microsoft.SharePoint.WebPartPages", "WebPartPages" },
			{ "Microsoft.SharePoint.Publishing.WebControls", "PublishingWebControls" },
			{ "Microsoft.SharePoint.Publishing.Navigation", "PublishingNavigation" }
		};

        internal static readonly Dictionary<string, TagInfo> BuiltInTags = new Dictionary<string, TagInfo>
        {
            { typeof(FormField).Namespace, new TagInfo("wss", typeof(FormField).Assembly.FullName, typeof(FormField).Namespace) },
            { typeof(BaseRichFieldType).Namespace, new TagInfo("cmsf", typeof(BaseRichFieldType).Assembly.FullName, typeof(BaseRichFieldType).Namespace) },
            { typeof(BaseRichField).Namespace, new TagInfo("cmsc", typeof(BaseRichField).Assembly.FullName, typeof(BaseRichField).Namespace) }
        };

        internal static Dictionary<String, String> CustomTagPrefixMappings = new Dictionary<String, String>();

        /// <summary>
        /// Contains a list of Site Columns which values will be excluded of the export.
        /// This is either because the value of the Site Column is read-only or because it cannot be set for another reason
        /// </summary>
        private static readonly List<Guid> skipFields = new List<Guid>()
        {
          SPBuiltInFieldId.FileLeafRef,
          SPBuiltInFieldId.Title
        };


        internal static string WritePageLayoutContents(Dictionary<string, TagInfo> assemblies, List<FieldInfo> fields)
        {
            StringBuilder pageLayout = new StringBuilder();

            pageLayout.AppendLine("<%@ Page language=\"C#\" Inherits=\"Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c\" %>");

            // assembly references
            foreach (KeyValuePair<string, TagInfo> assembly in assemblies)
            {
                pageLayout.AppendLine(assembly.Value.ToString());
            }

            // PlaceHolderAdditionalPageHead
            pageLayout.AppendLine("<asp:Content ContentPlaceholderID=\"PlaceHolderAdditionalPageHead\" runat=\"server\">");
            pageLayout.AppendLine("  <SharePointWebControls:CssRegistration name=\"<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/page-layouts-21.css %>\" runat=\"server\"/>");
            pageLayout.AppendLine("  <PublishingWebControls:EditModePanel runat=\"server\">");
            pageLayout.AppendLine("    <!-- Styles for edit mode only-->");
            pageLayout.AppendLine("    <SharePointWebControls:CssRegistration name=\"<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/edit-mode-21.css %>\"");
            pageLayout.AppendLine("      After=\"<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/page-layouts-21.css %>\" runat=\"server\"/>");
            pageLayout.AppendLine("    </PublishingWebControls:EditModePanel>");
            pageLayout.AppendLine("  <SharePointWebControls:CssRegistration name=\"<% $SPUrl:~sitecollection/Style Library/~language/Core Styles/rca.css %>\" runat=\"server\"/>");
            pageLayout.AppendLine("  <SharePointWebControls:FieldValue id=\"PageStylesField\" FieldName=\"HeaderStyleDefinitions\" runat=\"server\"/>");
            pageLayout.AppendLine("</asp:Content>");

            // put title in the PlaceHolderPageTitle
            pageLayout.AppendLine("<asp:Content ContentPlaceholderID=\"PlaceHolderPageTitle\" runat=\"server\">");
            pageLayout.AppendLine("  <SharePointWebControls:FieldValue id=\"PageTitle\" FieldName=\"Title\" runat=\"server\"/>");
            pageLayout.AppendLine("</asp:Content>");

            // put title in the PlaceHolderPageTitleInTitleArea
            pageLayout.AppendLine("<asp:Content ContentPlaceholderID=\"PlaceHolderPageTitleInTitleArea\" runat=\"server\">");
            pageLayout.AppendLine("  <SharePointWebControls:FieldValue FieldName=\"Title\" runat=\"server\"/>");
            pageLayout.AppendLine("</asp:Content>");

            // Breadcrumbs
            pageLayout.AppendLine("<asp:Content ContentPlaceHolderId=\"PlaceHolderTitleBreadcrumb\" runat=\"server\">");
            pageLayout.AppendLine("  <SharePointWebControls:ListSiteMapPath runat=\"server\" SiteMapProviders=\"CurrentNavigation\" RenderCurrentNodeAsLink=\"false\" PathSeparator=\"\" NodeStyle-CssClass=\"s4-breadcrumbNode\" CurrentNodeStyle-CssClass=\"s4-breadcrumbCurrentNode\" RootNodeStyle-CssClass=\"s4-breadcrumbRootNode\" NodeImageOffsetX=0 NodeImageOffsetY=321 NodeImageWidth=16 NodeImageHeight=16 NodeImageUrl=\"/_layouts/images/fgimg.png\" HideInteriorRootNodes=\"true\" SkipLinkText=\"\" />");
            pageLayout.AppendLine("</asp:Content>");

            // Main content
            pageLayout.AppendLine();
            pageLayout.AppendLine("<asp:Content ContentPlaceHolderId=\"PlaceHolderMain\" runat=\"server\">");

            // view fields
            foreach (FieldInfo field in fields)
            {
                pageLayout.AppendFormat("  <SharePointWebControls:FieldValue FieldName=\"{0}\" runat=\"server\"/>" + Environment.NewLine, field.Name);
            }

            // edit panel
            pageLayout.AppendLine();
            pageLayout.AppendLine("  <PublishingWebControls:EditModePanel runat=\"server\" id=\"editmodepanel\" CssClass=\"editPanel\">");

            // edit fields
            foreach (FieldInfo field in fields)
            {
                if (!field.Field.ReadOnlyField)
                {
                    pageLayout.AppendFormat("    " + field.Template + Environment.NewLine, field.Tag.TagName);
                }
            }

            pageLayout.AppendLine("  </PublishingWebControls:EditModePanel>");

            pageLayout.AppendLine("</asp:Content>"); // end Main content

            return pageLayout.ToString();
        }

        internal static string SafeContentTypeName(string contentTypeName)
        {
            string safeName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(contentTypeName);
            safeName = new Regex(@"[^\w]").Replace(safeName, "");
            return safeName;
        }
        #endregion
    }
}

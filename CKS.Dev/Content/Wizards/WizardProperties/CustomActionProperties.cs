using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using System.Threading;
using System.Xml.Linq;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties
{
    class CustomActionProperties : INotifyPropertyChanged
    {
        #region Fields

        private Uri _sourceUrl;
        private readonly string _uniqueId;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets or sets the assembly of a control that supports the custom action.
        /// </summary>
        public string ControlAssembly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a control class that supports the custom action.
        /// </summary>
        public string ControlClass
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the relative URL of the .ascx file that serves as the source for the custom action, 
        /// for example, "~/_controltemplates/myCustomAction.ascx".
        /// </summary>
        public string ControlSrc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a longer description for the action that is exposed as a tooltip or sub-description 
        /// for the action.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an action group that contains the action, for example, "SiteManagement". If contained 
        /// within a custom action group, the value of the GroupId attribute must equal the group ID of the 
        /// customAction element.
        /// </summary>
        public string GroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a unique identifier for the custom action. The ID may be a GUID, or it may be a unique
        /// term, for example, "HtmlViewer".
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a virtual server relative link to an image that presents an icon for the item.
        /// </summary>
        public string ImageUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the location of this custom action, for example, "Microsoft.SharePoint.SiteSettings".
        /// </summary>
        public string Location
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the identifier of the list or item content type that this action is associated with,
        /// or the file type or programmatic identifier (ProgID).
        /// </summary>
        public string RegistrationId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the registration attachment for a per-item action.
        /// </summary>
        public string RegistrationType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets TRUE to specify that the item be displayed only if the user is a site administrator;
        /// otherwise, FALSE.
        /// </summary>
        public bool? RequireSiteAdministrator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Specifies a set of rights that the user must have in order for the link to be visible, 
        /// for example, "ViewListItems,ManageAlerts". If not specified, then the action always appears in the 
        /// list of actions. To specify multiple rights, separate the values by using commas. The set of rights 
        /// are grouped logically according to AND logic, which means that a user must have all the specified
        /// rights to see an action. 
        /// </summary>
        public string Rights
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ordering priority for actions.
        /// </summary>
        public int? Sequence
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets TRUE if the custom action is only displayed for read-only content types on the page 
        /// for managing content types. The default value is FALSE.
        /// </summary>
        public bool? ShowInReadOnlyContentTypes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets TRUE if the custom action is only displayed for sealed content types on the page for
        /// managing content types. The default value is FALSE.
        /// </summary>
        public bool? ShowInSealedContentTypes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the end user description for this action.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the url action for this action.
        /// </summary>
        public string UrlAction
        {
            get;
            set;
        }

        /// <summary>
        /// SourceUrl
        /// </summary>
        public Uri SourceUrl
        {
            get
            {
                return _sourceUrl;
            }
            private set
            {
                _sourceUrl = value;
                PropertyChangedEventHandler propertyChanged = PropertyChanged;
                if (propertyChanged != null)
                {
                    propertyChanged(this, new PropertyChangedEventArgs("SourceUrl"));
                }
            }
        }

        #endregion

        #region Methods

        public CustomActionProperties()
        {

        }

        public CustomActionProperties(Guid uniqueId, ISourceUrlSource urlSource)
        {
            _uniqueId = ConvertToId(uniqueId);
            _sourceUrl = urlSource.SourceUrl;
            urlSource.PropertyChanged += new PropertyChangedEventHandler(source_PropertyChanged);
        }

        private static string ConvertToId(Guid value)
        {
            return value.ToString().Replace("-", "");
        }

        private void source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SourceUrl")
            {
                SourceUrl = ((ISourceUrlSource)sender).SourceUrl;
            }
        }

        protected XElement BuildEntireElement()
        {
            XElement customAction = new XElement("CustomAction");

            if (!String.IsNullOrEmpty(ControlAssembly))
            {
                XAttribute controlAssembly = new XAttribute("ControlAssembly", ControlAssembly);
                customAction.Add(controlAssembly);
            }

            if (!String.IsNullOrEmpty(ControlClass))
            {
                XAttribute controlClass = new XAttribute("ControlClass", ControlClass);
                customAction.Add(controlClass);
            }

            if (!String.IsNullOrEmpty(ControlSrc))
            {
                XAttribute controlSrc = new XAttribute("ControlSrc", ControlSrc);
                customAction.Add(controlSrc);
            }

            if (!String.IsNullOrEmpty(Description))
            {
                XAttribute description = new XAttribute("Description", Description);
                customAction.Add(description);
            }

            if (!String.IsNullOrEmpty(GroupId))
            {
                XAttribute groupId = new XAttribute("GroupId", GroupId);
                customAction.Add(groupId);
            }

            if (!String.IsNullOrEmpty(Id))
            {
                XAttribute id = new XAttribute("Id", Id);
                customAction.Add(id);
            }

            if (!String.IsNullOrEmpty(ImageUrl))
            {
                XAttribute imageUrl = new XAttribute("ImageUrl", ImageUrl);
                customAction.Add(imageUrl);
            }

            if (!String.IsNullOrEmpty(Location))
            {
                XAttribute location = new XAttribute("Location", Location);
                customAction.Add(location);
            }

            if (!String.IsNullOrEmpty(RegistrationId))
            {
                XAttribute registrationId = new XAttribute("RegistrationId", RegistrationId);
                customAction.Add(registrationId);
            }

            if (!String.IsNullOrEmpty(RegistrationType))
            {
                XAttribute registrationType = new XAttribute("RegistrationType", RegistrationType);
                customAction.Add(registrationType);
            }

            if (RequireSiteAdministrator != null)
            {
                XAttribute requireSiteAdministrator = new XAttribute("RequireSiteAdministrator", RequireSiteAdministrator);
                customAction.Add(requireSiteAdministrator);
            }

            if (!String.IsNullOrEmpty(Rights))
            {
                XAttribute rights = new XAttribute("Rights", Rights);
                customAction.Add(rights);
            }

            if (Sequence != null)
            {
                XAttribute sequence = new XAttribute("Sequence", Sequence);
                customAction.Add(sequence);
            }

            if (ShowInReadOnlyContentTypes != null)
            {
                XAttribute showInReadOnlyContentTypes = new XAttribute("ShowInReadOnlyContentTypes", ShowInReadOnlyContentTypes);
                customAction.Add(showInReadOnlyContentTypes);
            }

            if (ShowInSealedContentTypes != null)
            {
                XAttribute showInSealedContentTypes = new XAttribute("ShowInSealedContentTypes", ShowInSealedContentTypes);
                customAction.Add(showInSealedContentTypes);
            }

            if (!String.IsNullOrEmpty(Title))
            {
                XAttribute title = new XAttribute("Title", Title);
                customAction.Add(title);
            }

            if (!String.IsNullOrEmpty(UrlAction))
            {
                XElement urlAction = new XElement("UrlAction", new XAttribute("Url", UrlAction));
                customAction.Add(urlAction);
            }

            return customAction;
        }

        /// <summary>
        /// ToString returns the xml representation of the element
        /// </summary>
        /// <returns>The xml</returns>
        public override string ToString()
        {
            return BuildEntireElement().ToString();
        }

        #endregion
    }
}


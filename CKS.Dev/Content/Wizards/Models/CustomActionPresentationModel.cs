using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using EnvDTE;
using System.ComponentModel;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models
{
    class CustomActionPresentationModel : BasePresentationModel
    {
        #region Fields

        private CustomActionProperties _customActionProperties;

        private DTE _designTimeEnvironment;

        private Uri _sourceurl;

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
        /// Gets the result of validating the ControlAssembly
        /// </summary>
        public bool IsControlAssemblyValid
        {
            get { return ValidateControlAssembly(); }
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
        /// Gets the result of validating the ControlClass
        /// </summary>
        public bool IsControlClassValid
        {
            get { return ValidateControlClass(); }
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
        /// Gets the result of validating the ControlSrc
        /// </summary>
        public bool IsControlSrcValid
        {
            get { return ValidateControlSrc(); }
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
        /// Gets the result of validating the Description
        /// </summary>
        public bool IsDescriptionValid
        {
            get { return ValidateDescription(); }
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
        /// Gets the result of validating the GroupId
        /// </summary>
        public bool IsGroupIdValid
        {
            get { return ValidateGroupId(); }
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
        /// Gets the result of validating the Id
        /// </summary>
        public bool IsIdValid
        {
            get { return ValidateId(); }
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
        /// Gets the result of validating the ImageUrl
        /// </summary>
        public bool IsImageUrlValid
        {
            get { return ValidateImageUrl(); }
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
        /// Gets the result of validating the Location
        /// </summary>
        public bool IsLocationValid
        {
            get { return ValidateLocation(); }
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
        /// Gets the result of validating the RegistrationId
        /// </summary>
        public bool IsRegistrationIdValid
        {
            get { return ValidateRegistrationId(); }
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
        /// Gets the result of validating the RegistrationType
        /// </summary>
        public bool IsRegistrationTypeValid
        {
            get { return ValidateRegistrationType(); }
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
        /// Gets the result of validating the RequireSiteAdministrator
        /// </summary>
        public bool IsRequireSiteAdministratorValid
        {
            get { return ValidateRequireSiteAdministrator(); }
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
        /// Gets the result of validating the Rights
        /// </summary>
        public bool IsRightsValid
        {
            get { return ValidateRights(); }
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
        /// Gets the result of validating the Sequence
        /// </summary>
        public bool IsSequenceValid
        {
            get { return ValidateSequence(); }
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
        /// Gets the result of validating the ShowInReadOnlyContentTypes
        /// </summary>
        public bool IsShowInReadOnlyContentTypesValid
        {
            get { return ValidateShowInReadOnlyContentTypes(); }
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
        /// Gets the result of validating the ShowInSealedContentTypes
        /// </summary>
        public bool IsShowInSealedContentTypesValid
        {
            get { return ValidateShowInSealedContentTypes(); }
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
        /// Gets the result of validating the Title
        /// </summary>
        public bool IsTitleValid
        {
            get { return ValidateTitle(); }
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
        /// Gets the result of validating the UrlAction
        /// </summary>
        public bool IsUrlActionValid
        {
            get { return ValidateUrlAction(); }
        }

        /// <summary>
        /// Gets the available registration types
        /// </summary>
        public BindingList<BasicListItem> AvailableRegistrationTypes
        {
            get
            {
                return GetAvailableRegistrationTypes();
            }
        }

        /// <summary>
        /// Gets the requires site admin values
        /// </summary>
        public BindingList<BasicListItem> RequireSiteAdministratorValues
        {
            get
            {
                return GetRequireSiteAdministratorValues();
            }
        }

        /// <summary>
        /// Gets the show in readonly content types values
        /// </summary>
        public BindingList<BasicListItem> ShowInReadOnlyContentTypesValues
        {
            get
            {
                return GetShowInReadOnlyContentTypes();
            }
        }

        /// <summary>
        /// Gets the show in sealed content types values
        /// </summary>
        public BindingList<BasicListItem> ShowInSealedContentTypesValues
        {
            get
            {
                return GetShowInSealedContentTypes();
            }
        }

        #endregion

        #region Methods

        public CustomActionPresentationModel(CustomActionProperties customActionProperties,
            bool isOptional,
            DTE designTimeEnvironment)
            : base(isOptional)
        {
            if (customActionProperties == null)
            {
                throw new ArgumentNullException("customActionProperties");
            }
            _customActionProperties = customActionProperties;
            _designTimeEnvironment = designTimeEnvironment;
            _sourceurl = _customActionProperties.SourceUrl;
            _customActionProperties.PropertyChanged += new PropertyChangedEventHandler(_customActionProperties_PropertyChanged);
        }

        void _customActionProperties_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "SourceUrl") && !this._sourceurl.Equals(((CustomActionProperties)sender).SourceUrl))
            {
                this._sourceurl = ((CustomActionProperties)sender).SourceUrl;
            }
        }

        /// <summary>
        /// Gets the available registration types
        /// </summary>
        /// <returns>A bindinglist</returns>
        protected virtual BindingList<BasicListItem> GetAvailableRegistrationTypes()
        {
            BindingList<BasicListItem> items = new BindingList<BasicListItem>();

            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "Not specified",
                    ValueMember = "-1"
                });
            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "ContentType",
                    ValueMember = "ContentType"
                });
            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "FileType",
                    ValueMember = "FileType"
                });
            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "List",
                    ValueMember = "List"
                });
            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "ProgId",
                    ValueMember = "ProgId"
                });

            return items;
        }

        /// <summary>
        /// Gets the requires site admin values
        /// </summary>
        /// <returns>A bindinglist</returns>
        protected virtual BindingList<BasicListItem> GetRequireSiteAdministratorValues()
        {
            BindingList<BasicListItem> items = new BindingList<BasicListItem>();

            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "Not specified",
                    ValueMember = "-1"
                });
            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "True",
                    ValueMember = "true"
                });
            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "False",
                    ValueMember = "false"
                });

            return items;
        }

        /// <summary>
        /// Gets the show in readonly content types values
        /// </summary>
        /// <returns>A bindinglist</returns>
        protected virtual BindingList<BasicListItem> GetShowInReadOnlyContentTypes()
        {
            BindingList<BasicListItem> items = new BindingList<BasicListItem>();

            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "Not specified",
                    ValueMember = "-1"
                });
            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "True",
                    ValueMember = "true"
                });
            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "False",
                    ValueMember = "false"
                });

            return items;
        }

        /// <summary>
        /// Gets the show in sealed content types values
        /// </summary>
        /// <returns>A bindinglist</returns>
        protected virtual BindingList<BasicListItem> GetShowInSealedContentTypes()
        {
            BindingList<BasicListItem> items = new BindingList<BasicListItem>();

            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "Not specified",
                    ValueMember = "-1"
                });
            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "True",
                    ValueMember = "true"
                });
            items.Add(
                new BasicListItem()
                {
                    DisplayMember = "False",
                    ValueMember = "false"
                });

            return items;
        }
        
        /// <summary>
        /// Save the changes to the properties
        /// </summary>
        public override void SaveChanges()
        {
            if (string.IsNullOrEmpty(this.Id))
            {
                throw new NullReferenceException("WizardResources.InvalidContentTypeIdErrorMessage");
            }
            _customActionProperties.ControlAssembly = ControlAssembly;
            _customActionProperties.ControlClass = ControlClass;
            _customActionProperties.ControlSrc = ControlSrc;
            _customActionProperties.Description = Description;
            _customActionProperties.GroupId = GroupId;
            _customActionProperties.Id = Id;
            _customActionProperties.ImageUrl = ImageUrl;
            _customActionProperties.Location = Location;
            _customActionProperties.RegistrationId = RegistrationId;
            _customActionProperties.RegistrationType = RegistrationType;
            _customActionProperties.RequireSiteAdministrator = RequireSiteAdministrator;
            _customActionProperties.Rights = Rights;
            _customActionProperties.Sequence = Sequence;
            _customActionProperties.ShowInReadOnlyContentTypes = ShowInReadOnlyContentTypes;
            _customActionProperties.ShowInSealedContentTypes = ShowInSealedContentTypes;
            _customActionProperties.Title = Title;
            _customActionProperties.UrlAction = UrlAction;
        }

        /// <summary>
        /// Validate the Id
        /// </summary>
        /// <returns>True if the Id is valid</returns>
        protected virtual bool ValidateId()
        {
            return true;
        }

        /// <summary>
        /// Validate the Title
        /// </summary>
        /// <returns>True if the Title is valid</returns>
        protected virtual bool ValidateTitle()
        {
            return !String.IsNullOrEmpty(Title);
        }

        /// <summary>
        /// Validate the Description
        /// </summary>
        /// <returns>True if the Description is valid</returns>
        protected virtual bool ValidateDescription()
        {
            return true;
        }

        /// <summary>
        /// Validate the Location
        /// </summary>
        /// <returns>True if the Location is valid</returns>
        protected virtual bool ValidateLocation()
        {
            return !String.IsNullOrEmpty(Location);
        }

        /// <summary>
        /// Validate the Sequence
        /// </summary>
        /// <returns>True if the Sequence is valid</returns>
        protected virtual bool ValidateSequence()
        {
            return true;
        }

        /// <summary>
        /// Validate the Group Id
        /// </summary>
        /// <returns>True if the Title is valid</returns>
        protected virtual bool ValidateGroupId()
        {
            return true;
        }

        /// <summary>
        /// Validate the Image Url
        /// </summary>
        /// <returns>True if the ImageUrl is valid</returns>
        protected virtual bool ValidateImageUrl()
        {
            return true;
        }

        /// <summary>
        /// Validate the RegistrationId
        /// </summary>
        /// <returns>True if the RegistrationId is valid</returns>
        protected virtual bool ValidateRegistrationId()
        {
            return true;
        }

        /// <summary>
        /// Validate the RegistrationType
        /// </summary>
        /// <returns>True if the RegistrationType is valid</returns>
        protected virtual bool ValidateRegistrationType()
        {
            return true;
        }

        /// <summary>
        /// Validate the RequireSiteAdministrator
        /// </summary>
        /// <returns>True if the RequireSiteAdministrator is valid</returns>
        protected virtual bool ValidateRequireSiteAdministrator()
        {
            return true;
        }

        /// <summary>
        /// Validate the Rights
        /// </summary>
        /// <returns>True if the Rights is valid</returns>
        protected virtual bool ValidateRights()
        {
            return true;
        }

        /// <summary>
        /// Validate the ShowInReadOnlyContentTypes
        /// </summary>
        /// <returns>True if the ShowInReadOnlyContentTypes is valid</returns>
        protected virtual bool ValidateShowInReadOnlyContentTypes()
        {
            return true;
        }

        /// <summary>
        /// Validate the ShowInSealedContentTypes
        /// </summary>
        /// <returns>True if the ShowInSealedContentTypes is valid</returns>
        protected virtual bool ValidateShowInSealedContentTypes()
        {
            return true;
        }

        /// <summary>
        /// Validate the ControlAssembly
        /// </summary>
        /// <returns>True if the ControlAssembly is valid</returns>
        protected virtual bool ValidateControlAssembly()
        {
            return true;
        }

        /// <summary>
        /// Validate the ControlClass
        /// </summary>
        /// <returns>True if the ControlClass is valid</returns>
        protected virtual bool ValidateControlClass()
        {
            return true;
        }

        /// <summary>
        /// Validate the ControlSrc
        /// </summary>
        /// <returns>True if the ControlSrc is valid</returns>
        protected virtual bool ValidateControlSrc()
        {
            return true;
        }

        /// <summary>
        /// Validate the UrlAction
        /// </summary>
        /// <returns>True if the UrlAction is valid</returns>
        protected virtual bool ValidateUrlAction()
        {
            return true;
        }

        #endregion
    }
}

using System;
using System.ComponentModel;
using EnvDTE;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models
{
    /// <summary>
    /// The presentation model for the custom action group wizard
    /// </summary>
    class CustomActionGroupPresentationModel : BasePresentationModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the current CustomActionGroupProperties
        /// </summary>
        private CustomActionGroupProperties CurrentCustomActionGroupProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current DTE
        /// </summary>
        private DTE CurrentDesignTimeEnvironment
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current source url
        /// </summary>
        private Uri CurrentSourceurl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the element. The ID
        /// may be a GUID, or it may be a unique term, for example, "SiteManagement".
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
        /// Gets or sets the end user description for the action group.
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
        /// Gets or sets the longer description that is exposed as a sub-description for the action group.
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
        /// Gets or sets the value for where the action lives. 
        /// This string is a name that is declared on the LinkSectionTable control within a page.
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
        /// Gets or sets the ordering priority for the action group. 
        /// </summary>
        public int? Sequence
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the result of validating the Id
        /// </summary>
        public bool IsSequenceValid
        {
            get { return ValidateSequence(); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new instance of CustomActionGroupPresentationModel
        /// </summary>
        /// <param name="customActionGroupProperties">The custom action group properties</param>
        /// <param name="isOptional">The is optional flag</param>
        /// <param name="designTimeEnvironment">The DTE</param>
        public CustomActionGroupPresentationModel(CustomActionGroupProperties customActionGroupProperties,
            bool isOptional,
            DTE designTimeEnvironment)
            : base(isOptional)
        {
            if (customActionGroupProperties == null)
            {
                throw new ArgumentNullException("customActionGroupProperties");
            }
            CurrentCustomActionGroupProperties = customActionGroupProperties;
            CurrentDesignTimeEnvironment = designTimeEnvironment;
            CurrentSourceurl = customActionGroupProperties.SourceUrl;
            CurrentCustomActionGroupProperties.PropertyChanged += new PropertyChangedEventHandler(CurrentCustomActionGroupProperties_PropertyChanged);
        }


        /// <summary>
        /// The properties changed event handler
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The PropertyChangedEventArgs object</param>
        private void CurrentCustomActionGroupProperties_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "SourceUrl") && !CurrentSourceurl.Equals(((CustomActionGroupProperties)sender).SourceUrl))
            {
                CurrentSourceurl = ((CustomActionGroupProperties)sender).SourceUrl;
            }
        }

        /// <summary>
        /// Save the changes to the properties object
        /// </summary>
        public override void SaveChanges()
        {
            CurrentCustomActionGroupProperties.Id = Id;
            CurrentCustomActionGroupProperties.Title = Title;
            CurrentCustomActionGroupProperties.Description = Description;
            CurrentCustomActionGroupProperties.Location = Location;
            CurrentCustomActionGroupProperties.Sequence = Sequence;
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

        #endregion
    }
}

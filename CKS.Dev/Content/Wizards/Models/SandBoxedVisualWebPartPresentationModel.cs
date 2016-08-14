using System;
using System.ComponentModel;
using EnvDTE;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models
{
    /// <summary>
    /// The presentation model for the sandbox visual web part wizard
    /// </summary>
    class SandBoxedVisualWebPartPresentationModel : BasePresentationModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the current SandBoxVisualWebPartProperties
        /// </summary>
        private SandBoxedVisualWebPartProperties CurrentSandBoxVisualWebPartProperties
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
        /// Gets or sets the ID of this hide custom action element, for example, "HideDeleteWeb".
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
        /// Gets or sets the action group that contains the action, for example, "SiteAdministration".
        /// </summary>
        public string GroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the result of validating the Group Id
        /// </summary>
        public bool IsGroupIdValid
        {
            get { return ValidateGroupId(); }
        }

        /// <summary>
        /// Gets or sets the ID of the custom action to hide, for example, "DeleteWeb". 
        /// See Default Custom Action Locations and IDs for a list of the default custom action 
        /// IDs that are used in SharePoint Foundation.
        /// </summary>
        public string HideActionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the result of validating the Hide Action Id
        /// </summary>
        public bool IsHideActionIdValid
        {
            get { return ValidateHideActionId(); }
        }

        /// <summary>
        /// Gets or sets the location of the custom action to hide, for example, "Microsoft.SharePoint.SiteSettings".
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

        #endregion

        #region Methods

        /// <summary>
        /// Create a new instance of SandBoxVisualWebPartPresentationModel
        /// </summary>
        /// <param name="sandBoxVisualWebPartProperties">The properties</param>
        /// <param name="isOptional">Is optional flag</param>
        /// <param name="designTimeEnvironment">The DTE</param>
        public SandBoxedVisualWebPartPresentationModel(SandBoxedVisualWebPartProperties sandBoxVisualWebPartProperties,
            bool isOptional,
            DTE designTimeEnvironment)
            : base(isOptional)
        {
            if (sandBoxVisualWebPartProperties == null)
            {
                throw new ArgumentNullException(Resources.SandBoxVisualWebPartPresentationModel_CtorException);
            }
            CurrentSandBoxVisualWebPartProperties = sandBoxVisualWebPartProperties;
            CurrentDesignTimeEnvironment = designTimeEnvironment;
            CurrentSourceurl = CurrentSandBoxVisualWebPartProperties.SourceUrl;
            CurrentSandBoxVisualWebPartProperties.PropertyChanged += new PropertyChangedEventHandler(CurrentSandBoxVisualWebPartProperties_PropertyChanged);
        }

        /// <summary>
        /// The properties changed event handler
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The PropertyChangedEventArgs object</param>
        private void CurrentSandBoxVisualWebPartProperties_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "SourceUrl") && !CurrentSourceurl.Equals(((HideCustomActionProperties)sender).SourceUrl))
            {
                CurrentSourceurl = ((HideCustomActionProperties)sender).SourceUrl;
            }
        }

        /// <summary>
        /// Save the changes to the properties object
        /// </summary>
        public override void SaveChanges()
        {
            CurrentSandBoxVisualWebPartProperties.Id = Id;
            CurrentSandBoxVisualWebPartProperties.GroupId = GroupId;
            CurrentSandBoxVisualWebPartProperties.HideActionId = HideActionId;
            CurrentSandBoxVisualWebPartProperties.Location = Location;
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
        /// Validate the Group Id
        /// </summary>
        /// <returns>True if the Group Id is valid</returns>
        protected virtual bool ValidateGroupId()
        {
            return true;
        }

        /// <summary>
        /// Validate the Hide Action Id
        /// </summary>
        /// <returns>True if the Hide Action Id is valid</returns>
        protected virtual bool ValidateHideActionId()
        {
            return true;
        }

        /// <summary>
        /// Validate the Location
        /// </summary>
        /// <returns>True if the Location is valid</returns>
        protected virtual bool ValidateLocation()
        {
            return true;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties;
using EnvDTE;
using System.Management.Automation;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using System.ComponentModel;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models
{
    /// <summary>
    /// The powershell cmdlet model.
    /// </summary>
    class SPPowerShellCmdLetModel : BasePresentationModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the current SPPowerShellCmdLetProperties
        /// </summary>
        private SPPowerShellCmdLetProperties CurrentSPPowerShellCmdLetProperties
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
        /// Gets or sets the verb.
        /// </summary>
        /// <value>The verb.</value>
        public string Verb { get; set; }

        /// <summary>
        /// Gets the result of validating the Verb
        /// </summary>
        public bool IsVerbValid
        {
            get { return ValidateVerb(); }
        }

        /// <summary>
        /// Gets or sets the noun.
        /// </summary>
        /// <value>The noun.</value>
        public string Noun { get; set; }

        /// <summary>
        /// Gets the result of validating the Noun
        /// </summary>
        public bool IsNounValid
        {
            get { return ValidateNoun(); }
        }

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        public string Object { get; set; }

        /// <summary>
        /// Gets the result of validating the Object
        /// </summary>
        public bool IsObjectValid
        {
            get { return ValidateObject(); }
        }

        /// <summary>
        /// Gets or sets a Boolean value specifying whether the cmdlet requires that a farm exist on the local machine in order to execute.
        /// </summary>
        public bool RequireLocalFarmExist { get; set; }

        /// <summary>
        /// Gets the result of validating the RequireLocalFarmExist
        /// </summary>
        public bool IsRequireLocalFarmExistValid
        {
            get { return ValidateRequireLocalFarmExist(); }
        }

        /// <summary>
        /// Gets or sets a Boolean value specifying whether the cmdlet requires that a user have administrative privleges on the farm in order to invoke the cmdlet.
        /// </summary>
        public bool RequireUserFarmAdmin { get; set; }

        /// <summary>
        /// Gets the result of validating the RequireUserFarmAdmin
        /// </summary>
        public bool IsRequireUserFarmAdminValid
        {
            get { return ValidateRequireUserFarmAdmin(); }
        }

        /// <summary>
        /// Gets or sets a Boolean value specifying whether the cmdlet requires that a user have administrative privileges on the computer on which the cmdlet is invoked.
        /// </summary>
        public bool RequireUserMachineAdmin { get; set; }

        /// <summary>
        /// Gets the result of validating the RequireUserFarmAdmin
        /// </summary>
        public bool IsRequireUserMachineAdmin
        {
            get { return ValidateRequireUserMachineAdmin(); }
        }

        /// <summary>
        /// Gets the available verbs
        /// </summary>
        public BindingList<BasicListItem> AvailableVerbs
        {
            get
            {
                return GetVerbs();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new instance of HideCustomActionPresentationModel
        /// </summary>
        /// <param name="spPowerShellCmdLetProperties">The properties</param>
        /// <param name="isOptional">Is optional flag</param>
        /// <param name="designTimeEnvironment">The DTE</param>
        public SPPowerShellCmdLetModel(SPPowerShellCmdLetProperties spPowerShellCmdLetProperties,
            bool isOptional,
            DTE designTimeEnvironment)
            : base(isOptional)
        {
            if (spPowerShellCmdLetProperties == null)
            {
                throw new ArgumentNullException(Resources.HideCustomActionPresentationModel_CtorException);
            }
            CurrentSPPowerShellCmdLetProperties = spPowerShellCmdLetProperties;
            CurrentDesignTimeEnvironment = designTimeEnvironment;
            CurrentSourceurl = CurrentSPPowerShellCmdLetProperties.SourceUrl;
            CurrentSPPowerShellCmdLetProperties.PropertyChanged += new PropertyChangedEventHandler(SPPowerShellCmdLetProperties_PropertyChanged);
        }

        /// <summary>
        /// The properties changed event handler
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The PropertyChangedEventArgs object</param>
        private void SPPowerShellCmdLetProperties_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "SourceUrl") && !CurrentSourceurl.Equals(((SPPowerShellCmdLetProperties)sender).SourceUrl))
            {
                CurrentSourceurl = ((SPPowerShellCmdLetProperties)sender).SourceUrl;
            }
        }

        /// <summary>
        /// Save the changes to the properties object
        /// </summary>
        public override void SaveChanges()
        {
            CurrentSPPowerShellCmdLetProperties.Verb = Verb;
            CurrentSPPowerShellCmdLetProperties.Noun = Noun;
            CurrentSPPowerShellCmdLetProperties.Object = Object;
            CurrentSPPowerShellCmdLetProperties.RequireLocalFarmExist = RequireLocalFarmExist;
            CurrentSPPowerShellCmdLetProperties.RequireUserFarmAdmin = RequireUserFarmAdmin;
            CurrentSPPowerShellCmdLetProperties.RequireUserMachineAdmin = RequireUserMachineAdmin;
        }

        /// <summary>
        /// Validate the Verb
        /// </summary>
        /// <returns>True if the Verb is valid</returns>
        protected virtual bool ValidateVerb()
        {
            return true;
        }

        /// <summary>
        /// Validate the Noun
        /// </summary>
        /// <returns>True if the Noun is valid</returns>
        protected virtual bool ValidateNoun()
        {
            return true;
        }

        /// <summary>
        /// Validate the Object
        /// </summary>
        /// <returns>True if the Object is valid</returns>
        protected virtual bool ValidateObject()
        {
            return true;
        }

        /// <summary>
        /// Validate the RequireLocalFarmExist
        /// </summary>
        /// <returns>True if the RequireLocalFarmExist is valid</returns>
        protected virtual bool ValidateRequireLocalFarmExist()
        {
            return true;
        }

        /// <summary>
        /// Validate the RequireUserFarmAdmin
        /// </summary>
        /// <returns>True if the RequireUserFarmAdmin is valid</returns>
        protected virtual bool ValidateRequireUserFarmAdmin()
        {
            return true;
        }

        /// <summary>
        /// Validate the RequireUserMachineAdmin
        /// </summary>
        /// <returns>True if the RequireUserMachineAdmin is valid</returns>
        protected virtual bool ValidateRequireUserMachineAdmin()
        {
            return true;
        }

        /// <summary>
        /// Gets the Verbs values
        /// </summary>
        /// <returns>A bindinglist</returns>
        protected virtual BindingList<BasicListItem> GetVerbs()
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
                    DisplayMember = VerbsCommon.Get,
                    ValueMember = VerbsCommon.Get
                });
            items.Add(
                new BasicListItem()
                {
                    DisplayMember = VerbsCommon.New,
                    ValueMember = VerbsCommon.New
                });
            items.Add(
                new BasicListItem()
                {
                    DisplayMember = VerbsCommon.Remove,
                    ValueMember = VerbsCommon.Remove
                });
            items.Add(
                new BasicListItem()
                {
                    DisplayMember = VerbsCommon.Set,
                    ValueMember = VerbsCommon.Set
                });

            return items;
        }

        #endregion
    }
}

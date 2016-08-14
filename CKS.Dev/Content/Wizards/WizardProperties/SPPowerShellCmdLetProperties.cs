using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Management.Automation;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties
{
    /// <summary>
    /// The SPPowerShellCmdLet properties.
    /// </summary>
    class SPPowerShellCmdLetProperties : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Backing field for the SourceUrl
        /// </summary>
        private Uri sourceUrl;
        
        #endregion

        #region Events

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// SourceUrl
        /// </summary>
        public Uri SourceUrl
        {
            get
            {
                return sourceUrl;
            }
            private set
            {
                sourceUrl = value;
                PropertyChangedEventHandler propertyChanged = PropertyChanged;
                if (propertyChanged != null)
                {
                    propertyChanged(this, new PropertyChangedEventArgs("SourceUrl"));
                }
            }
        }

        /// <summary>
        /// Gets or sets the verb.
        /// </summary>
        /// <value>The verb.</value>
        public string Verb { get; set; }

        /// <summary>
        /// Gets or sets the noun.
        /// </summary>
        /// <value>The noun.</value>
        public string Noun { get; set; }

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        public string Object { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value specifying whether the cmdlet requires that a farm exist on the local machine in order to execute.
        /// </summary>
        public bool RequireLocalFarmExist { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value specifying whether the cmdlet requires that a user have administrative privleges on the farm in order to invoke the cmdlet.
        /// </summary>
        public bool RequireUserFarmAdmin { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value specifying whether the cmdlet requires that a user have administrative privileges on the computer on which the cmdlet is invoked.
        /// </summary>
        public bool RequireUserMachineAdmin { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initialise a new HideCustomActionProperties
        /// </summary>
        public SPPowerShellCmdLetProperties()
        {
            this.RequireLocalFarmExist = false;
            this.RequireUserFarmAdmin = false;
            this.RequireUserMachineAdmin = false;
        }

        /// <summary>
        /// Initialise a new SPPowerShellCmdLetProperties
        /// </summary>
        /// <param name="urlSource">The source url</param>
        public SPPowerShellCmdLetProperties(ISourceUrlSource urlSource)
        {
            sourceUrl = urlSource.SourceUrl;
            urlSource.PropertyChanged += new PropertyChangedEventHandler(source_PropertyChanged);
        }

        /// <summary>
        /// The source url has changed
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The PropertyChangedEventArgs object</param>
        private void source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SourceUrl")
            {
                SourceUrl = ((ISourceUrlSource)sender).SourceUrl;
            }
        }

        #endregion
    }
}

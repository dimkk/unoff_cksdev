using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using System.Xml.Linq;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties
{
    /// <summary>
    /// The Custom Action Group properties
    /// </summary>
    class CustomActionGroupProperties : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Backing field for the SourceUrl
        /// </summary>
        private Uri sourceUrl;

        /// <summary>
        /// Backing string for the uniqueId
        /// </summary>
        private readonly string uniqueId;

        #endregion

        #region Events

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

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
        /// Gets or sets the end user description for the action group.
        /// </summary>
        public string Title
        {
            get;
            set;
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
        /// Gets or sets the value for where the action lives. 
        /// This string is a name that is declared on the LinkSectionTable control within a page.
        /// </summary>
        public string Location
        {
            get;
            set;
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

        #endregion

        #region Methods

        /// <summary>
        /// Initialise a new CustomActionGroupProperties
        /// </summary>
        public CustomActionGroupProperties()
        {

        }

        /// <summary>
        /// Initialise a new CustomActionGroupProperties
        /// </summary>
        /// <param name="uniqueId">The unique Id></param>
        /// <param name="urlSource">The source url</param>
        public CustomActionGroupProperties(Guid uniqueId, ISourceUrlSource urlSource)
        {
            this.uniqueId = ConvertToId(uniqueId);
            sourceUrl = urlSource.SourceUrl;
            urlSource.PropertyChanged += new PropertyChangedEventHandler(source_PropertyChanged);
        }

        /// <summary>
        /// Convert the id from a guid
        /// </summary>
        /// <param name="value">The id as a guid</param>
        /// <returns>The string value</returns>
        private static string ConvertToId(Guid value)
        {
            return value.ToString().Replace("-", "");
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

        /// <summary>
        /// Construct the xml for the Custom Action Group
        /// </summary>
        /// <returns>The xml</returns>
        protected XElement BuildEntireElement()
        {
            XElement customActionGroup = new XElement("CustomActionGroup");

            if(!String.IsNullOrEmpty(Id))
            {
                XAttribute id = new XAttribute("Id", Id);
                customActionGroup.Add(id);
            }

            if (!String.IsNullOrEmpty(Title))
            {
                XAttribute title = new XAttribute("Title", Title);
                customActionGroup.Add(title);
            }

            if (!String.IsNullOrEmpty(Description))
            {
                XAttribute description = new XAttribute("Description", Description);
                customActionGroup.Add(description);
            }

            if (!String.IsNullOrEmpty(Location))
            {
                XAttribute location = new XAttribute("Location", Location);
                customActionGroup.Add(location);
            }

            if (Sequence != null)
            {
                XAttribute sequence = new XAttribute("Sequence", Sequence);
                customActionGroup.Add(sequence);
            }

            return customActionGroup;
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


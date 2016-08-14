using System;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties
{
    /// <summary>
    /// The Hide Custom Action properties
    /// </summary>
    class HideCustomActionProperties : INotifyPropertyChanged
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
        /// Gets or sets the ID of this hide custom action element, for example, "HideDeleteWeb".
        /// </summary>
        public string Id
        {
            get;
            set;
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
        /// Gets or sets the location of the custom action to hide, for example, "Microsoft.SharePoint.SiteSettings".
        /// </summary>
        public string Location
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
        /// Initialise a new HideCustomActionProperties
        /// </summary>
        public HideCustomActionProperties()
        {

        }

        /// <summary>
        /// Initialise a new HideCustomActionProperties
        /// </summary>
        /// <param name="uniqueId">The unique Id></param>
        /// <param name="urlSource">The source url</param>
        public HideCustomActionProperties(Guid uniqueId, ISourceUrlSource urlSource)
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
        /// Construct the xml for the Hide Custom Action
        /// </summary>
        /// <returns>The xml</returns>
        protected XElement BuildEntireElement()
        {
            XElement hideCustomAction = new XElement("HideCustomAction");

            if(!String.IsNullOrEmpty(Id))
            {
                XAttribute id = new XAttribute("Id", Id);
                hideCustomAction.Add(id);
            }

            if (!String.IsNullOrEmpty(GroupId))
            {
                XAttribute groupId = new XAttribute("GroupId", GroupId);
                hideCustomAction.Add(groupId);
            }

            if (!String.IsNullOrEmpty(HideActionId))
            {
                XAttribute hideActionId = new XAttribute("HideActionId", HideActionId);
                hideCustomAction.Add(hideActionId);
            }

            if (!String.IsNullOrEmpty(Location))
            {
                XAttribute location = new XAttribute("Location", Location);
                hideCustomAction.Add(location);
            }

            return hideCustomAction;
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

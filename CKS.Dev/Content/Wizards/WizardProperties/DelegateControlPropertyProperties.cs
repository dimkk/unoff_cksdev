using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using System.Xml.Linq;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties
{
    class DelegateControlPropertyProperties : INotifyPropertyChanged
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
        /// Gets or sets the name of the property.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the property as an XElement
        /// </summary>
        public XElement PropertyElement
        {
            get
            {
                return BuildEntireElement();
            }
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

        public DelegateControlPropertyProperties()
        {

        }

        public DelegateControlPropertyProperties(Guid uniqueId, ISourceUrlSource urlSource)
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
            XElement property = new XElement("Property", Value);

            if(!String.IsNullOrEmpty(Name))
            {
                XAttribute name = new XAttribute("Name", Name);
                property.Add(name);
            }

            return property;
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
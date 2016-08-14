using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using System.Xml.Linq;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties
{
    class DelegateControlProperties : INotifyPropertyChanged
    {
        #region Fields

        private Uri _sourceUrl;
        private readonly string _uniqueId;
        private List<DelegateControlPropertyProperties> _controlProperties;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ID of the control, for example, "SmallSearchInputBox".
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the strong name of the assembly for the control.
        /// </summary>
        public string ControlAssembly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the class for the control.
        /// </summary>
        public string ControlClass
        {
            get;
            set;
        }
               
        /// <summary>
        /// Gets or sets the  relative URL of the .ascx file that serves as the
        /// source for the control, for example, "~/_controltemplates/mySearch.ascx".
        /// </summary>
        public string ControlSrc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sequence number for the control, which determines whether
        /// the control is added to the control tree for a page. The control with the 
        /// lowest sequence number is added to the tree. 
        /// </summary>
        public int? Sequence
        {
            get;
            set;
        }

        public List<DelegateControlPropertyProperties> ControlProperties
        {
            get
            {
                 if(_controlProperties == null)
                {
                    _controlProperties = new List<DelegateControlPropertyProperties>();
                }
                return _controlProperties;
            }
            set
            {
                if(_controlProperties == null)
                {
                    _controlProperties = new List<DelegateControlPropertyProperties>();
                }
                _controlProperties = value;
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

        public DelegateControlProperties()
        {

        }

        public DelegateControlProperties(Guid uniqueId, ISourceUrlSource urlSource)
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
            XElement control = new XElement("Control");

            if(!String.IsNullOrEmpty(Id))
            {
                XAttribute id = new XAttribute("Id", Id);
                control.Add(id);
            }

            if (Sequence != null)
            {
                XAttribute sequence = new XAttribute("Sequence", Sequence);
                control.Add(sequence);
            }

            if (!String.IsNullOrEmpty(ControlAssembly))
            {
                XAttribute controlAssembly = new XAttribute("ControlAssembly", ControlAssembly);
                control.Add(controlAssembly);
            }

            if (!String.IsNullOrEmpty(ControlClass))
            {
                XAttribute controlClass = new XAttribute("ControlClass", ControlClass);
                control.Add(controlClass);
            }

            if (!String.IsNullOrEmpty(ControlSrc))
            {
                XAttribute controlSrc = new XAttribute("ControlSrc", ControlSrc);
                control.Add(controlSrc);
            }

            //Process the child property nodes
            if(ControlProperties != null && ControlProperties.Count > 0)
            {
                foreach (DelegateControlPropertyProperties property in ControlProperties)
            	{
                    control.Add(property.PropertyElement);
            	}
            }

            return control;
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


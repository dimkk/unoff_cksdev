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
    class DelegateControlPresentationModel : BasePresentationModel
    {
        #region Fields

        private DelegateControlProperties _delegateControlProperties;

        private DTE _designTimeEnvironment;

        private Uri _sourceurl;
   
        private List<DelegateControlPropertyProperties> _controlProperties;

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
        /// Gets the result of validating the Id
        /// </summary>
        public bool IsIdValid
        {
            get { return ValidateId(); }
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
        /// Gets the result of validating the ControlAssembly
        /// </summary>
        public bool IsControlAssemblyValid
        {
            get { return ValidateControlAssembly(); }
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
        /// Gets the result of validating the ControlClass
        /// </summary>
        public bool IsControlClassValid
        {
            get { return ValidateControlClass(); }
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
        /// Gets the result of validating the ControlSrc
        /// </summary>
        public bool IsControlSrcValid
        {
            get { return ValidateControlSrc(); }
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

        /// <summary>
        /// Gets the result of validating the Id
        /// </summary>
        public bool IsSequenceValid
        {
            get { return ValidateSequence(); }
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

        #endregion

        #region Methods

        public DelegateControlPresentationModel(DelegateControlProperties delegateControlProperties,
            bool isOptional,
            DTE designTimeEnvironment)
            : base(isOptional)
        {
            if (delegateControlProperties == null)
            {
                throw new ArgumentNullException("delegateControlProperties");
            }
            _delegateControlProperties = delegateControlProperties;
            _designTimeEnvironment = designTimeEnvironment;
            _sourceurl = _delegateControlProperties.SourceUrl;
            _delegateControlProperties.PropertyChanged += new PropertyChangedEventHandler(_delegateControlProperties_PropertyChanged);
        }

        void _delegateControlProperties_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "SourceUrl") && !this._sourceurl.Equals(((DelegateControlProperties)sender).SourceUrl))
            {
                this._sourceurl = ((DelegateControlProperties)sender).SourceUrl;
            }
        }


        /// <summary>
        /// Saves the changes.
        /// </summary>
        public override void SaveChanges()
        {
            _delegateControlProperties.Id = Id;
            _delegateControlProperties.ControlAssembly = ControlAssembly;
            _delegateControlProperties.ControlClass = ControlClass;
            _delegateControlProperties.ControlSrc = ControlSrc;
            _delegateControlProperties.Sequence = Sequence;
            _delegateControlProperties.ControlProperties = ControlProperties;
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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.SharePoint;
using System.Threading;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Content.Wizards.WizardProperties
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Content.Wizards.WizardProperties
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Content.Wizards.WizardProperties
#else
namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties
#endif
{
    class DeploymentProperties : INotifyPropertyChanged, ISourceUrlSource
    {
        #region Fields

        private MRUHelper _mruHelper = new MRUHelper();
        private Uri _url;
        private readonly string _uniqueId;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public Uri SourceUrl
        {
            get
            {
                return this.Url;
            }
        }
        
        /// <summary>
        /// SourceUrl
        /// </summary>
        public Uri Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                PropertyChangedEventHandler propertyChanged = PropertyChanged;
                if (propertyChanged != null)
                {
                    propertyChanged(this, new PropertyChangedEventArgs("SourceUrl"));
                }
            }
        }

        public List<string> MruUrlList
        {
            get
            {
                return this._mruHelper.MruUrlList;
            }
        }

        public bool IsSandboxedSolution
        {
            set;
            get;
        }


        #endregion

        #region Methods

        public DeploymentProperties()
        {
            this._url = new Uri(this._mruHelper.GetDefaultUrl());
            this.IsSandboxedSolution = true;
        }

        public DeploymentProperties(Guid uniqueId, ISourceUrlSource urlSource)
        {
            _uniqueId = ConvertToId(uniqueId);
            _url = urlSource.SourceUrl;
            urlSource.PropertyChanged += new PropertyChangedEventHandler(source_PropertyChanged);
        }

        private static string ConvertToId(Guid value)
        {
            return value.ToString().Replace("-", "");
        }

        internal void SaveProjectProperties(ISharePointProject sharePointProject)
        {
            sharePointProject.SiteUrl = this.Url;
            sharePointProject.IsSandboxedSolution = this.IsSandboxedSolution;
            this._mruHelper.SaveUrlToMruList(this.Url);
        }

        private void source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SourceUrl")
            {
                Url = ((ISourceUrlSource)sender).SourceUrl;
            }
        }

        #endregion
    }
}


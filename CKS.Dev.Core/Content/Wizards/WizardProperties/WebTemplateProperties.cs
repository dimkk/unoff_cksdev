using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Xml.Linq;

#if VS2012Build_SYMBOL
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
#elif VS2013Build_SYMBOL
using CKS.Dev12.VisualStudio.SharePoint.Commands.Info;
#elif VS2014Build_SYMBOL
using CKS.Dev13.VisualStudio.SharePoint.Commands.Info;
#else
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
#endif

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
    class WebTemplateProperties
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
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the web template info.
        /// </summary>
        /// <value>The web template info.</value>
        public WebTemplateInfo WebTemplateInfo { get; set; }

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

        public WebTemplateProperties()
        {

        }

        public WebTemplateProperties(Guid uniqueId, ISourceUrlSource urlSource)
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

        #endregion
    }
}


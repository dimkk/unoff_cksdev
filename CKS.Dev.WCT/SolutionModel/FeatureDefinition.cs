using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Features;
using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Extensions;
using System.IO;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class FeatureDefinition
    {
        private string _name = null;
        [XmlIgnore]
        public string Name 
        {
            get
            {
                if (String.IsNullOrEmpty(_name))
                {
                    _name = this.Title;
                }
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        [XmlIgnore]
        public FileInfo SourceFileInfo { get; set; }


        private string _sourceFolder = null;
        [XmlIgnore]
        public string SourceFolder
        {
            get 
            {
                if (_sourceFolder == null)
                {
                    _sourceFolder = Path.GetDirectoryName(this.SourceFileInfo.FullName);
                }
                return _sourceFolder; 
            }
            set { _sourceFolder = value; }
        }


        private IList<ElementManifestReference> _elementManifestList = new List<ElementManifestReference>();
        [XmlIgnore]
        public IList<ElementManifestReference> ElementManifestList
        {
            get
            {
                return _elementManifestList;
            }
        }

        private IList<ElementManifestReference> _elementFileList = new List<ElementManifestReference>();
        [XmlIgnore]
        public IList<ElementManifestReference> ElementFileList
        {
            get
            {
                return _elementFileList;
            }
        }

        private VSSharePointItemCollection _sharePointItems = new VSSharePointItemCollection();
        [XmlIgnore]
        public VSSharePointItemCollection SharePointItems
        {
            get { return _sharePointItems; }
            set { _sharePointItems = value; }
        }

        [XmlIgnore]
        public ClassInformationCollection Classes { get; set; }
        
    }
}

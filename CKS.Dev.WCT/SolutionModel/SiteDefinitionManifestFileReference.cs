using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class SiteDefinitionManifestFileReference
    {
        [XmlIgnore]
        public DirectoryInfo SourceDirectory { get; set; }

        [XmlIgnore]
        public VSSiteDinitionItem VSItem { get; set; }

        //private ShadowList<WebTempFileDefinition> _webTempFileCollection = null;
        //[XmlIgnore]
        //public ShadowList<WebTempFileDefinition> WebTempFileCollection
        //{
        //    get 
        //    {
        //        if (_webTempFileCollection == null)
        //        {
        //            _webTempFileCollection = new ShadowList<WebTempFileDefinition>(ref this.webTempFileField);
        //        }
        //        return _webTempFileCollection;
        //    }
        //    set 
        //    { 
        //        _webTempFileCollection = value;
        //    }
        //}
    }
}

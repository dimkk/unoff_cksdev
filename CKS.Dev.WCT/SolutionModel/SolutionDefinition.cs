using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class SolutionDefinition
    {
        [XmlIgnore]
        public string Name { get; set; }


        private List<FeatureDefinition> _featureList = new List<FeatureDefinition>();
        [XmlIgnore]
        public List<FeatureDefinition> FeatureList
        {
            get { return _featureList; }
        }

        private List<TemplateFileReference> _templateFileList = new List<TemplateFileReference>();
        [XmlIgnore]
        public List<TemplateFileReference> TemplateFileList
        {
            get { return _templateFileList; }
        }

        private List<RootFileReference> _rootFileList = new List<RootFileReference>();
        [XmlIgnore]
        public List<RootFileReference> RootFileList
        {
            get { return _rootFileList; }
        }

        [XmlIgnore]
        public VSWPCatalogItem VSWPCatalogItem { get; set; }


        //private List<SiteDefinitionManifestFileReference> _siteDefinitionList = new List<SiteDefinitionManifestFileReference>();
        //[XmlIgnore]
        //public List<SiteDefinitionManifestFileReference> SiteDefinitionManifestList
        //{
        //    get { return _siteDefinitionList; }
        //    set { _siteDefinitionList = value; }
        //}


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class ApplicationResourceFileDefinitions
    {
        [XmlIgnore]
        public VSAppGlobalResourcesItem VSGlobalItem { get; set; }

        [XmlIgnore]
        public VSApplicationResourcesItem VSApplicationItem { get; set; }
    }
}

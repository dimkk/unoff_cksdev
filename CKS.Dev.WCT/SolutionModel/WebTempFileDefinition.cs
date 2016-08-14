using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class WebTempFileDefinition
    {
        [XmlIgnore]
        public FileInfo SourceFileInfo { get; set; }
    }
}

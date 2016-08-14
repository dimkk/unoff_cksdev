using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class TemplateFileReference
    {
        [XmlIgnore]
        public FileInfo SourceFileInfo { get; set; }
    }
}

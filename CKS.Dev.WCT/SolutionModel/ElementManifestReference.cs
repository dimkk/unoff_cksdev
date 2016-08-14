using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Features;
using System.Xml.Serialization;
using System.IO;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class ElementManifestReference
    {
        [XmlIgnore]
        public FileInfo SourceFileInfo { get; set; }


        private string _name = null;
        [XmlIgnore]
        public string Name
        {
            get
            {
                if (String.IsNullOrEmpty(_name))
                {
                    _name = this.Location;
                    if (!string.IsNullOrEmpty(_name) && _name.Contains(@"\"))
                    {
                        _name = _name.Substring(0, _name.IndexOf(@"\"));
                    }
                }

                return _name;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Common;
using System.Xml.Serialization;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class FileDefinition
    {
        [XmlIgnore]
        public string Filename { get; set; }
        
        [XmlIgnore]
        public string LocalPath { get; set; }

        [XmlIgnore]
        public ClassInformationCollection Classes { get; set; }


        public void ParseWebPart(VSProject project)
        {

            if (this.Url.EnsureString().EndsWithIgnoreCase(".webpart"))
            {
                try
                {

                    XmlDocument doc = new XmlDocument();
                    doc.Load(this.Filename);

                    XmlElement node = XmlHelper.SelectSingleElement(doc.DocumentElement, "webPart/metaData/type");
                    if (node != null)
                    {
                        string assemblyName = node.GetAttribute("name");
                        string className = assemblyName.SubStringBefore(",");

                        this.Classes = project.Classes.GetValue(className);
                    }

                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                }
            }

            if (this.Url.EnsureString().EndsWithIgnoreCase(".dwp"))
            {
                try
                {

                    XmlDocument doc = new XmlDocument();
                    doc.Load(this.Filename);

                    XmlElement node = XmlHelper.SelectSingleElement(doc.DocumentElement, "TypeName");
                    if (node != null)
                    {
                        string className = node.InnerText;
                        this.Classes = project.Classes.GetValue(className);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                }
            }

        }

    }
}

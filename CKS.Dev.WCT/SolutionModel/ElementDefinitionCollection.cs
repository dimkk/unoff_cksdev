using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Framework.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class ElementDefinitionCollection
    {
        [XmlIgnore]
        public FileInfo SourceFileInfo { get; set; }

        [XmlIgnore]
        public Guid InternalID = Guid.NewGuid();

        [XmlIgnore]
        public Dictionary<string, List<object>> GenericItems = new Dictionary<string, List<object>>();


        public static T CreateItem<T>(string xml)
        {
            ElementDefinitionCollection elementDef = new ElementDefinitionCollection();
            string elementXml = Serializer.ObjectToXML(elementDef);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(elementXml);
            xmlDoc.DocumentElement.InnerXml = xml;

            elementDef = Serializer.XmlToObject<ElementDefinitionCollection>(xmlDoc.DocumentElement.OuterXml);
            T result = (T)elementDef.Items[0];
            return result;
        }

    }
}

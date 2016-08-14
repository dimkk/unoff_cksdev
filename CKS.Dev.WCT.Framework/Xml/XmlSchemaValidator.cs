using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace CKS.Dev.WCT.Framework.Xml
{
    public class XMLValidator
    {
        public static string[] Validate(XmlSchema schema, Stream source)
        {
            List<string> errors = new List<string>();

            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(schema);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationEventHandler += new ValidationEventHandler(
                delegate(object sender, ValidationEventArgs args)
                {
                    string message = "{0}. Line {1} - Position {2}";
                    errors.Add(String.Format(message, args.Exception.Message, args.Exception.LineNumber, args.Exception.LinePosition));
                    if (errors.Count > 10)
                    {
                        message = string.Join(", ", errors.ToArray());
                        throw new ApplicationException("To many errors in xml. Validation terminated! " + message);
                    }
                });

            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = schemaSet;

            XmlReader reader = XmlReader.Create(source, settings);
            
            while (reader.Read())
            {
            }

            return errors.ToArray();
        }
    }
}
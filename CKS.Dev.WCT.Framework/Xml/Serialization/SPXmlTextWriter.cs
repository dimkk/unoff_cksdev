using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Schema;
using System.Reflection;
using System.Collections.Specialized;

namespace CKS.Dev.WCT.Framework.Serialization
{
    public class SPXmlTextWriter : XmlTextWriter
    {
        private bool _attributeNewline = false;
        private TextWriter _internalWriter = null;
        private List<string> _newlineTags = null;


        public bool AttributeNewline
        {
            get { return _attributeNewline; }
            set { _attributeNewline = value; }
        }

        public TextWriter InternalWriter
        {
            get 
            {
                if (_internalWriter == null)
                {
                    FieldInfo TextWriterField = typeof(XmlTextWriter).GetField("textWriter", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic);
                    _internalWriter = (TextWriter)TextWriterField.GetValue(this);
                }
                return _internalWriter; 
            }
        }

        public List<string> NewlineTags
        {
            get { return _newlineTags; }
            set { _newlineTags = value; }
        }
        

        public SPXmlTextWriter(TextWriter w) : base(w)
        {
        }
        public SPXmlTextWriter(Stream w, Encoding encoding) : base(w, encoding)
        {
        }
        public SPXmlTextWriter(string filename, Encoding encoding) : base(filename, encoding)
        {
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            if (NewlineTags != null)
            {
                AttributeNewline = NewlineTags.Contains(localName, StringComparer.OrdinalIgnoreCase);
            }
            base.WriteStartElement(prefix, localName, ns);
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            if (AttributeNewline)
            {
                InternalWriter.Write(Environment.NewLine + "\t");
            }
            base.WriteStartAttribute(prefix, localName, ns);
        }
    }
}

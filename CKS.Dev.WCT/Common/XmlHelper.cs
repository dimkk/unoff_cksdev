//--------------------------------------------------------------------------------
// This file is a "Sample" from the SharePoint Foundation 2010
// Samples
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// This source code is intended only as a supplement to Microsoft
// Development Tools and/or on-line documentation.  See these other
// materials for detailed information regarding Microsoft code samples.
// 
// THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//--------------------------------------------------------------------------------

namespace CKS.Dev.WCT.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    public static class XmlHelper
    {
        public static XmlNodeList GetMatchingTags(this XmlDocument doc, string tagName)
        {
            return doc.GetElementsByTagName(tagName);
        }

        public static XmlNodeList GetMatchingTags(this XmlNode node, string tagName)
        {
            return node.SelectNodes(tagName);
        }

        public static XmlElement GetFirstMatchingTag(this XmlDocument doc, string tagName)
        {
            XmlNodeList items = doc.GetMatchingTags(tagName);

            return items.OfType<XmlElement>().FirstOrDefault();
        }

        public static void SetAttributeValueSafe(this XmlNode node, string attrib, string value)
        {
            XmlElement element = node as XmlElement;
            if (element != null)
            {
                element.SetAttribute(attrib, value);
            }
        }

        public static string GetAttributeValueSafe(this XmlNode node, string name)
        {
            XmlElement element = node as XmlElement;
            if (element != null)
            {
                return element.GetAttribute(name);
            }

            return String.Empty;
        }

        public static XmlDocument Load(string path, bool preserverWhiteSpace)
        {
            if (String.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return null;
            }

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.PreserveWhitespace = preserverWhiteSpace;
                doc.Load(path);
            }
            catch (Exception)
            {
                doc = null;
            }

            return doc;
        }

        public static XmlDocument LoadXml(string xml, bool preserverWhiteSpace)
        {
            if (String.IsNullOrEmpty(xml))
            {
                return null;
            }

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.PreserveWhitespace = preserverWhiteSpace;
                doc.LoadXml(xml);
            }
            catch (Exception)
            {
                doc = null;
            }

            return doc;
        }

        public static XmlDocument RetrieveOwnerDocument(XmlNode element)
        {
            if (element == null)
            {
                return null;
            }

            XmlDocument doc = element.OwnerDocument;
            if (doc == null)
            {
                doc = element as XmlDocument;
            }

            return doc;
        }

        public static XmlNamespaceManager CreateNamespaceManager(XmlNode element, string namespaceUri, string prefix)
        {
            if (element == null || String.IsNullOrEmpty(prefix))
            {
                return null;
            }

            string actualNamespaceUri = namespaceUri;
            if (String.IsNullOrEmpty(actualNamespaceUri))
            {
                actualNamespaceUri = RetrieveNamespaceUri(element);
            }

            if (String.IsNullOrEmpty(actualNamespaceUri))
            {
                return null;
            }

            XmlDocument doc = RetrieveOwnerDocument(element);
            if (doc == null)
            {
                return null;
            }

            XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace(prefix, actualNamespaceUri);

            return manager;
        }

        public static string RetrieveNamespaceUri(XmlNode element)
        {
            if (element == null)
            {
                return null;
            }

            XmlDocument doc = RetrieveOwnerDocument(element);
            if (doc == null)
            {
                return null;
            }

            XmlNode root = doc.DocumentElement;
            if (root == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(root.NamespaceURI))
            {
                return root.NamespaceURI;
            }
            else
            {
                if (root.FirstChild != null)
                {
                    return root.FirstChild.NamespaceURI;
                }
            }
            return null;
        }

        public static XmlElement GetDocumentElement(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return null;
            }

            XmlDocument doc = Load(path, false);
            if (doc == null)
            {
                return null;
            }
            return doc.DocumentElement;
        }

        public static XmlElement SelectSingleElement(XmlElement element, string xpath)
        {
            return SelectSingleElement(element, xpath, null);
        }

        public static XmlElement SelectSingleElement(XmlElement element, string xpath, string namespaceUri)
        {
            if (element == null || String.IsNullOrEmpty(xpath))
            {
                return null;
            }

            string prefix = "ns";
            XmlNamespaceManager manager = CreateNamespaceManager(element, namespaceUri, prefix);

            string actualXpath = xpath;
            if (manager != null)
            {
                actualXpath = actualXpath.Replace("/", "/ns:");
                if (!actualXpath.StartsWith("/"))
                {
                    actualXpath = "ns:" + actualXpath;
                }
            }

            XmlNode node = null;
            if (manager != null)
            {
                node = element.SelectSingleNode(actualXpath, manager);
            }
            else
            {
                node = element.SelectSingleNode(actualXpath);
            }
            return node as XmlElement;
        }

        public static XmlNodeList SelectNodes(XmlNode element, string xpath)
        {
            return SelectNodes(element, xpath, null);
        }

        public static XmlNodeList SelectNodes(XmlNode element, string xpath, string namespaceUri)
        {
            if (element == null || String.IsNullOrEmpty(xpath))
            {
                return null;
            }

            string prefix = "ns";
            XmlNamespaceManager manager = CreateNamespaceManager(element, namespaceUri, prefix);

            string actualXpath = xpath;
            if (manager != null)
            {
                actualXpath = actualXpath.Replace("/", "/ns:");
                if (!actualXpath.StartsWith("/"))
                {
                    actualXpath = "ns:" + actualXpath;
                }
            }

            XmlNodeList resultNodes = null;
            if (manager != null)
            {
                resultNodes = element.SelectNodes(actualXpath, manager);
            }
            else
            {
                resultNodes = element.SelectNodes(actualXpath);
            }

            return resultNodes;
        }

        public static XmlElement AppendChild(XmlElement element, string name)
        {
            if (element == null || String.IsNullOrEmpty(name))
            {
                return null;
            }

            return AppendChild(element, name, null);
        }

        public static XmlElement AppendChild(XmlElement element, string name, string namespaceUri)
        {
            if (element == null || String.IsNullOrEmpty(name))
            {
                return null;
            }

            string actualNamespaceUri = namespaceUri;
            if (String.IsNullOrEmpty(actualNamespaceUri))
            {
                actualNamespaceUri = RetrieveNamespaceUri(element);
            }

            XmlDocument doc = RetrieveOwnerDocument(element);
            if (doc == null)
            {
                return null;
            }

            XmlNode child;
            if (String.IsNullOrEmpty(actualNamespaceUri))
            {
                child = doc.CreateElement(name);
            }
            else
            {
                child = doc.CreateElement(name, actualNamespaceUri);
            }

            XmlElement newElement = null;
            if (child != null)
            {
                newElement = element.AppendChild(child) as XmlElement;
            }

            return newElement;
        }
    }
}
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
    using System.Xml;
    using System.IO;
    using System;

    public class SolutionFileChecker
    {
        public static bool IsElementManifest(string path)
        {
            return SolutionFileChecker.IsMatch(path, "Elements");
        }

        public static bool IsFeatureManifest(string path)
        {
            return SolutionFileChecker.IsMatch(path, "Feature");
        }

        public static bool IsMatch(string path, string rootNodeName)
        {
            bool isMatch = false;
            if(!string.IsNullOrEmpty(rootNodeName))
            {

                using (StreamReader sr = new StreamReader(path))
                {
                    XmlTextReader xtr = new XmlTextReader(sr);
                    
                    xtr.MoveToContent();
                    if (rootNodeName.Equals(xtr.Name, StringComparison.Ordinal) && xtr.NamespaceURI == Constants.SPDocumentNamespaceUri)
                    {
                        isMatch = true;
                    }
                }
            }

            return isMatch;
        }

        public static bool IsElementManifest(XmlDocument doc)
        {
            return IsMatch(doc, "Elements");
        }

        public static bool IsFeatureManifest(XmlDocument doc)
        {
            return IsMatch(doc, "Feature");
        }

        private static bool IsMatch(XmlDocument doc, string rootNodeName)
        {
            bool isMatch = false;

            if (doc != null)
            {
                XmlNode xRoot = doc.DocumentElement;
                if (xRoot.Name == rootNodeName && xRoot.NamespaceURI == Constants.SPDocumentNamespaceUri)
                {
                    isMatch = true;
                }
            }

            return isMatch;
        }

    }
}
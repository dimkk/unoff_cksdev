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
    using System.Xml;

    class ElementFinder
    {
        public EnvDTE.Project DTEProject { get; set; }

        public SolutionFileChecker Checker { get; set; }

        public ElementFinder(EnvDTE.Project dteProject)
        {
            this.DTEProject = dteProject;
            this.Checker = new SolutionFileChecker();
        }

        public string FindByElementId(Guid elementId)
        {
            string elementManifestPath = string.Empty;

            ICollection<EnvDTE.ProjectItem> items = this.FindElements(this.DTEProject.ProjectItems);
            foreach (EnvDTE.ProjectItem item in items)
            {
                if (this.GetElementManifestId(item.get_FileNames(1)) == elementId)
                {
                    elementManifestPath = item.get_FileNames(1);
                    break;
                }
            }

            return elementManifestPath;
        }

        public ICollection<EnvDTE.ProjectItem> FindElements(EnvDTE.ProjectItems items)
        {
            if (items == null)
            {
                throw new ArgumentException();
            }

            List<EnvDTE.ProjectItem> foundItems = new List<EnvDTE.ProjectItem>();

            foreach (EnvDTE.ProjectItem item in items)
            {
                string fileName = item.get_FileNames(1);
                if (Path.GetExtension(fileName).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    if (SolutionFileChecker.IsElementManifest(item.get_FileNames(1)))
                    {
                        foundItems.Add(item);
                    }
                }

                if (item.ProjectItems != null)
                {
                    ICollection<EnvDTE.ProjectItem> childItems = this.FindElements(item.ProjectItems);
                    foundItems.AddRange(childItems);
                }
            }

            return foundItems;
        }

        private Guid GetElementManifestId(string path)
        {
            XmlElement xRoot = XmlHelper.GetDocumentElement(path);

            if (xRoot != null)
            {
                string sid = String.Empty;
                string id = xRoot.GetAttribute("Id");

                Guid guid;
                if (Guid.TryParse(sid, out guid))
                {
                    return guid;
                }
            }

            return Guid.Empty;
        }
    }
}
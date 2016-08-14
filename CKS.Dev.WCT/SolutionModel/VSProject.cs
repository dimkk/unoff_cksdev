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

namespace CKS.Dev.WCT.SolutionModel
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.IO;
    using System.Linq;
    using CKS.Dev.WCT.Common;
    using System.Text.RegularExpressions;

    public class VSProject
    {
        public bool IsCSharp { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string Folder { get; set; }

        public string AssemblyFileName { get; set; }

        public string OutputPath { get; set; }

        public Guid ProjectGuid { get; set; }

        public string RootNamespace { get; set; }

        private string _siteUrl = null;
        public string SiteURL 
        {
            get
            {
                return _siteUrl;
            }
            set
            {
                _siteUrl = value;
                if (!String.IsNullOrEmpty(_siteUrl) && !_siteUrl.EndsWith("/"))
                {
                    _siteUrl += "/";
                }
            }
        }

        public bool IsSandboxedSolution { get; set; }

        public string AssemblyOriginatorKeyFile { get; set; }

        private XmlDocument _projDocument = null;

        public XmlDocument ProjDocument
        {
            get 
            {
                if (_projDocument == null)
                {
                    if (!String.IsNullOrEmpty(this.FileName))
                    {
                        _projDocument = XmlHelper.Load(this.FileName, false);
                    }
                }
                return _projDocument; 
            }
            set { _projDocument = value; }
        }


        public IList<ProjectBuildConfiguration> BuildConfigurations { get; set; }

        private ICollection<ReferenceInformation> _references = new List<ReferenceInformation>();
        public ICollection<ReferenceInformation> References
        {
            get
            {
                return this._references;
            }
        }

        private ICollection<string> _webReferences = new List<string>();
        public ICollection<string> WebReferences
        {
            get
            {
                return this._webReferences;
            }
        }

        private ICollection<string> _serviceReferences = new List<string>();
        public ICollection<string> ServiceReferences
        {
            get
            {
                return this._serviceReferences;
            }
        }

        public IList<ProjectFile> ProjectFiles { get; set; }

        public ProjectFileDictionary Files { get; set; }


        private ClassMapDictionary _classes = new ClassMapDictionary();
        public ClassMapDictionary Classes
        {
            get { return _classes; }
            set { _classes = value; }
        }


        public VSProject()
        {
        }


        public VSProject(string name, string fileName)
        {
            Name = name;
            FileName = fileName;
        }

        public string GetRelativePath(string fullPath)
        {
            string relPath = fullPath;

            relPath = relPath.Replace(this.Folder + Path.DirectorySeparatorChar, "");
            relPath = relPath.Replace(this.Folder, "");

            return relPath;
        }


    }
}

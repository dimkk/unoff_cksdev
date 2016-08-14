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

    public class ClassInformation
    {
        private Guid _id = Guid.NewGuid();
        public Guid Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        public string Name { get; set; }

        public string NameSpace { get; set; }

        public string ProjectRelativePath { get; set; }

        public string FullAssemblyName { get; set; }

        public bool IsFeatureReceiver { get; set; }

        public ProjectFile File { get; set; }

        public ClassInformation()
        {
        }

        public ClassInformation(Guid id, string name, string nameSpace, string fullAssemblyName, string projectRelativeLocation, bool isFeatureReceiver)
        {
            this._id = id;
            this.Name = name;
            this.NameSpace = nameSpace;
            this.FullAssemblyName = fullAssemblyName;
            this.ProjectRelativePath = projectRelativeLocation;
            this.IsFeatureReceiver = isFeatureReceiver;
        }

        public string FullClassNameWithoutAssembly
        {
            get
            {
                string fullClassName = string.Empty;
                if (!string.IsNullOrEmpty(this.NameSpace))
                {
                    fullClassName = String.Format("{0}.{1}", this.NameSpace, this.Name);
                }
                else
                {
                    fullClassName = this.Name;
                }
                return fullClassName;
            }
        }
    }
}
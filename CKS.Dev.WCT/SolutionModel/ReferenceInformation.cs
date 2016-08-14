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
    public class ReferenceInformation
    {
        public string Include { get; set; }

        public bool IsCopyLocal { get; set; }

        public bool IsProjectReference { get; set; }

        public ReferenceInformation(string include, bool isCopyLocal, bool isProjectReference)
        {
            this.Include = include;
            this.IsCopyLocal = isCopyLocal;
            this.IsProjectReference = isProjectReference;
        }
    }
}
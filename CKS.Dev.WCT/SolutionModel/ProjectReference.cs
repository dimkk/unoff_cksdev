using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev.WCT.SolutionModel
{
    public class ProjectReference
    {
        public string ReferenceName { get; set; }

        public bool IsCopyLocal { get; set; }

        public bool IsProjectReference { get; set; }

        public ProjectReference(string referenceName, bool isCopyLocal, bool isProjectReference)
        {
            this.ReferenceName = referenceName;
            this.IsCopyLocal = isCopyLocal;
            this.IsProjectReference = isProjectReference;
        }
    }
}

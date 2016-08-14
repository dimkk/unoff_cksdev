using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev.WCT.SolutionModel
{
    public class ProjectFileDictionary : Dictionary<string, ProjectFile>
    {
        public ProjectFileDictionary() : base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}

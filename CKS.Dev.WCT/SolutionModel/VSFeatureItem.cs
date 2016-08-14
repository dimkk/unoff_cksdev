using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev.WCT.SolutionModel
{
    public class VSFeatureItem : VSSharePointItem
    {
        public FeatureDefinition Feature { get; set; }

        public virtual ElementDefinitionCollection GetElementManifest()
        {
            return null;
        }


    }
}

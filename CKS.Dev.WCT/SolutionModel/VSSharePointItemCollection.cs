using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev.WCT.SolutionModel
{
    public class VSSharePointItemCollection : List<VSSharePointItem>
    {

        public VSSharePointItemCollection()
        {
        }

        public VSSharePointItemCollection(IEnumerable<VSSharePointItem> collection) : base(collection)
        {
        }

    }
}

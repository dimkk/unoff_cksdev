using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.SolutionModel;

namespace CKS.Dev.WCT.Mappers
{
    public class PackageMapper
    {
        public WCTContext Context { get; set; }


        public PackageMapper(WCTContext context)
        {
            this.Context = context;
        }

        public void Map()
        {

        }

    }
}

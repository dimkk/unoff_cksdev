﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.SolutionModel;

namespace CKS.Dev.WCT.ModelCreators
{
    public class PackageCreator
    {
        public WCTContext Context { get; set; }


        public PackageCreator(WCTContext context)
        {
            this.Context = context;
        }

        public void Load()
        {


        }
    }
}
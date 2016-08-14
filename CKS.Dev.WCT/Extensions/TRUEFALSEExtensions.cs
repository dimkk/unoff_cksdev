using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.SolutionModel;

namespace CKS.Dev.WCT.Extensions
{
    public static class TRUEFALSEExtensions
    {
        public static bool IsTrue(TRUEFALSE value)
        {
            bool result = false;

            result = (value == TRUEFALSE.True || value == TRUEFALSE.TRUE || value == TRUEFALSE.@true);

            return result;
        }
    }
}

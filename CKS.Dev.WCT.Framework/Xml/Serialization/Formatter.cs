using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev.WCT.Framework.Serialization
{
    public class Formatter
    {
        public static string GetCDATA(object text)
        {
            return string.Format("<![CDATA[{0}]]>", text as string);
        }

    }
}

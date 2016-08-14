using System;
using Microsoft.SharePoint.UserCode;

namespace $rootnamespace$
{
    public class $safeitemrootname$ : SPProxyOperation
    {
        public static string AssemblyName
        {
            get
            {
                return typeof($safeitemrootname$).Assembly.FullName;
            }
        }

        public static string TypeName
        {
            get
            {
                return typeof($safeitemrootname$).FullName;
            }
        }

        public override object Execute(SPProxyOperationArgs args)
        {
            $safeitemrootname$Args $strongTypedArgs$ = args as $safeitemrootname$Args;
           
            return String.Format("The input was: {0}", $strongTypedArgs$);
        }
    }
}


using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace $rootnamespace$
{
    [Guid("$guid1$")]
    class $subnamespace$ServiceProxy
        : SPServiceProxy
    {
        public $subnamespace$ServiceProxy()
	    {
	    }

        public $subnamespace$ServiceProxy(SPFarm farm)
            : base(String.Empty, farm)
	    {
	    }
    }
}

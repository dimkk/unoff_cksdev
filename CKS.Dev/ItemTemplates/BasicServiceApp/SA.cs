using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace $rootnamespace$
{
    [Guid("$guid3$")]
    class $subnamespace$ServiceApplication
        : SPServiceApplication
    {
        public override string TypeName
        {
            get { return "$fileinputname$ Application"; }
        }

        public $subnamespace$ServiceApplication()
	    {
	    }

        public $subnamespace$ServiceApplication(string name, $subnamespace$Service service)
            : base(name, service)
	    {
	    }

        public static $subnamespace$ServiceApplication Create(
            string name, $subnamespace$Service service)
        {
            $subnamespace$ServiceApplication serviceApplication = new $subnamespace$ServiceApplication(name, service);
            serviceApplication.Update();
            return serviceApplication;
        }

        public string SampleMethod()
        {
            return "Hello Service Application World";
        }
    }
}

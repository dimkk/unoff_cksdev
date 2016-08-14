using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace $rootnamespace$
{
    [Guid("$guid4$")]
    class $subnamespace$ServiceApplicationProxy
        : SPServiceApplicationProxy
    {
        [Persisted]
        Guid _serviceApplicationID;
        
        public override string TypeName
        {
            get { return "$fileinputname$ Application Proxy"; }
        }

        public $subnamespace$ServiceApplication ServiceApplication 
        {
            get{ return ($subnamespace$ServiceApplication)Farm.GetObject(_serviceApplicationID);}
        }
            
        public $subnamespace$ServiceApplicationProxy()
	    {
	    }

        public $subnamespace$ServiceApplicationProxy(string name, $subnamespace$ServiceProxy proxy, Guid serviceApplicationID)
            : base(name, proxy)
	    {
            _serviceApplicationID = serviceApplicationID;
	    }

        public static $subnamespace$ServiceApplicationProxy Create(string name, $subnamespace$ServiceProxy proxy, Guid serviceApplicationID)
        {
            $subnamespace$ServiceApplicationProxy applicationProxy = new $subnamespace$ServiceApplicationProxy(
                name, proxy, serviceApplicationID);
            applicationProxy.Update();
            return applicationProxy;
        }

        public string SampleMethod()
        {
            return ServiceApplication.SampleMethod();
        }
    }
}
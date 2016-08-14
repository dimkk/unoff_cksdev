using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace $rootnamespace$
{
    [Guid("$guid2$")]
    class $subnamespace$Service
        : SPService, IServiceAdministration
    {
        public $subnamespace$Service()
	    {
	    }

        public $subnamespace$Service(SPFarm farm)
            : base(String.Empty, farm)
	    {
	    }

        public SPServiceApplication CreateApplication(string name, 
            Type serviceApplicationType, 
            SPServiceProvisioningContext provisioningContext)
        {
            ValidateApplicationType(serviceApplicationType);
            return $subnamespace$ServiceApplication.Create(name, this);
        }

        public SPServiceApplicationProxy CreateProxy(string name, 
            SPServiceApplication serviceApplication, 
            SPServiceProvisioningContext provisioningContext)
        {
            ValidateApplicationType(serviceApplication.GetType());
            $subnamespace$ServiceProxy serviceProxy = Farm.ServiceProxies.GetValue<$subnamespace$ServiceProxy>();
            return new $subnamespace$ServiceApplicationProxy(name, serviceProxy, serviceApplication.Id);
        }

        public SPPersistedTypeDescription GetApplicationTypeDescription(
            Type serviceApplicationType)
        {
            ValidateApplicationType(serviceApplicationType);
            return new SPPersistedTypeDescription("$fileinputname$ Application", "");
        }

        public override SPAdministrationLink GetCreateApplicationLink(
            Type serviceApplicationType)
        {
            ValidateApplicationType(serviceApplicationType);
            return new SPAdministrationLink("/_admin/$rootnamespace$/$subnamespace$/NewApplication.aspx");
        }

        public override SPCreateApplicationOptions GetCreateApplicationOptions(
            Type serviceApplicationType)
        {
            ValidateApplicationType(serviceApplicationType);
            return SPCreateApplicationOptions.None;
        }

        public Type[] GetApplicationTypes()
        {
            return new Type[] { typeof($subnamespace$ServiceApplication) };
        }

        void ValidateApplicationType(Type serviceApplicationType)
        {
            if (serviceApplicationType != typeof($subnamespace$ServiceApplication))
            {
                throw new NotSupportedException();
            }
        }
    }
}

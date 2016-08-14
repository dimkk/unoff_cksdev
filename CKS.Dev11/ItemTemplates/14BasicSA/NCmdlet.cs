using System;
using Microsoft.SharePoint.PowerShell;
using Microsoft.SharePoint.Administration;
using System.Management.Automation;
using System.Security.Permissions;

namespace $rootnamespace$.PowerShell
{
    [Cmdlet("New", "$subnamespace$ServiceApplication",
        SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Low,
        DefaultParameterSetName = "Default"),
    PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    class New$subnamespace$ServiceApplication
        : SPCmdlet
    {
        [Parameter(Mandatory = false)]
        public string Name { get; set; }

        static $subnamespace$Service $subnamespace$Service
        {
            get { return SPFarm.Local.Services.GetValue<$subnamespace$Service>(); }
        }

        protected override void InternalProcessRecord()
        {
            string name = String.IsNullOrEmpty(Name) == false ?
                Name :
                "$subnamespace$ServiceApplication" + Guid.NewGuid();
            if (ShouldProcess(name))
            {
                $subnamespace$ServiceApplication serviceApplication =
                    $subnamespace$ServiceApplication.Create(name, $subnamespace$Service);
                try
                {
                    serviceApplication.Provision();
                }
                catch
                {
                    try
                    {
                        serviceApplication.Unprovision();
                    }
                    catch (Exception)
                    {
                    }
                    serviceApplication.Delete();
                    throw;
                }
                WriteObject(serviceApplication);
            }
        }
    }
}

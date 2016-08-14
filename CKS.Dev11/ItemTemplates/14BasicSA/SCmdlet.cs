using System;
using Microsoft.SharePoint.PowerShell;
using Microsoft.SharePoint.Administration;
using System.Management.Automation;
using System.Security.Permissions;

namespace $rootnamespace$.PowerShell
{
    [Cmdlet("Set", "$subnamespace$ServiceApplication",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Low,
        DefaultParameterSetName = "Default"),
    PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    class Set$subnamespace$ServiceApplication
        : SPCmdlet
    {
        [Parameter(Mandatory = false)]
        public string Name { get; set; }

        [Parameter(Mandatory = false)]
        public int SampleSetting { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0), ValidateNotNull]
        public SPServiceApplicationPipeBind Identity { get; set; }

        static $subnamespace$Service $subnamespace$Service
        {
            get { return SPFarm.Local.Services.GetValue<$subnamespace$Service>(); }
        }

        protected override void InternalProcessRecord()
        {
            $subnamespace$ServiceApplication serviceApplication = Identity.Read() as $subnamespace$ServiceApplication;
            string name = (this.Name != null) ? this.Name : serviceApplication.Name;
            if (ShouldProcess(serviceApplication.ToString()))
            {
                serviceApplication.Update(name, SampleSetting);
            }
        }
    }
}

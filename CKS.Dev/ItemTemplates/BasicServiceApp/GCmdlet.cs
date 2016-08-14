using System;
using Microsoft.SharePoint.PowerShell;
using Microsoft.SharePoint.Administration;
using System.Management.Automation;
using System.Security.Permissions;

namespace $rootnamespace$.PowerShell
{
    [Cmdlet("Get", "$subnamespace$ServiceApplication")]
    class Get$subnamespace$ServiceApplication
        : SPCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0)]
        public $subnamespace$ServiceApplicationPipeBind Identity { get; set; }

        static $subnamespace$Service $subnamespace$Service
        {
            get { return SPFarm.Local.Services.GetValue<$subnamespace$Service>(); }
        }

        protected override void InternalProcessRecord()
        {
            $subnamespace$ServiceApplicationPipeBind identity = Identity;
            if (identity != null)
            {
                $subnamespace$ServiceApplication sendToPipeline = identity.Read();
                if (null == sendToPipeline)
                {
                    base.WriteError(
                        new InvalidOperationException("Object not found."),
                        ErrorCategory.ObjectNotFound, this);
                }
                WriteObject(sendToPipeline);
            }
            else
            {
                WriteObject(
                    SPFarm.Local.Services.GetValue<$subnamespace$Service>().Applications, true);
            }
        }
    }
}

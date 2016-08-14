using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.PowerShell;
using System.Management.Automation;

namespace $rootnamespace$
{ 
    [SPCmdlet(RequireLocalFarmExist = true, RequireUserFarmAdmin = true)]
    public class $fileinputname$ : SPCmdlet
    {
        protected override void InternalProcessRecord()
        {
            base.InternalProcessRecord();
        }
    }
}
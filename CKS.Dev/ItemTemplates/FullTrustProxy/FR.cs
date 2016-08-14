using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.UserCode;

namespace $rootnamespace$
{
    [Guid("$frGuid$")]
    public class $safeitemname$ : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPUserCodeService service = SPUserCodeService.Local;
            if (GetOperation(service) == null)
            { 
                SPProxyOperationType operationType = new SPProxyOperationType($OperationItemName$.AssemblyName, 
                    $OperationItemName$.TypeName);
                service.ProxyOperationTypes.Add(operationType);
                service.Update();
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPUserCodeService service = SPUserCodeService.Local;
            SPProxyOperationType operationType = GetOperation(service);
            if (operationType != null)
            {
                service.ProxyOperationTypes.Remove(operationType);
                service.Update();
            }
        }

        static SPProxyOperationType GetOperation(SPUserCodeService service)
        {
            return service.ProxyOperationTypes.FirstOrDefault(
                            searchItem => searchItem.AssemblyName == $OperationItemName$.AssemblyName &&
                                searchItem.TypeName == $OperationItemName$.TypeName);
        }
    }
}

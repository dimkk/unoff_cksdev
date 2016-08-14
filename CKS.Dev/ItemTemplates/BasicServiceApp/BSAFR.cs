using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace $rootnamespace$
{
    public class $subnamespace$FeatureReceiver
        : SPFeatureReceiver
    {
        public override void FeatureActivated(
            SPFeatureReceiverProperties properties)
        {
            SPFarm farm = SPFarm.Local;
            $subnamespace$Service service = farm.Services.GetValue<$subnamespace$Service>();
            if(service == null)
            {
                service = new $subnamespace$Service(farm);
                service.Update();
            }
            $subnamespace$ServiceProxy proxy = farm.ServiceProxies.GetValue<$subnamespace$ServiceProxy>();
            if(proxy == null)
            {
                proxy = new $subnamespace$ServiceProxy(farm);
                proxy.Update();
            }
        }
        
        public override void FeatureDeactivating(
            SPFeatureReceiverProperties properties)
        {
            SPFarm farm = SPFarm.Local;
            $subnamespace$ServiceProxy proxy = farm.ServiceProxies.GetValue<$subnamespace$ServiceProxy>();
            if(proxy != null)
            {
                proxy.Delete();
            }
            $subnamespace$Service service = farm.Services.GetValue<$subnamespace$Service>();
            if(service != null)
            {
                service.Delete();
            }
        }
    }
}

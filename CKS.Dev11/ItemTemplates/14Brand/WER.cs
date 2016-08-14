using System;
using Microsoft.SharePoint;

namespace $rootnamespace$.$subnamespace$
{
    public class WebEventReceiver
        : SPWebEventReceiver
    {
        public override void WebProvisioned(SPWebEventProperties properties)
        {
            SPWeb site = properties.Web;
            SPWeb rootSite = site.Site.RootWeb;
            site.MasterUrl = rootSite.MasterUrl;
            site.CustomMasterUrl = rootSite.CustomMasterUrl;
            site.Update();
        }
    }
}

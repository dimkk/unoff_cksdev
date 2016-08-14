using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace $rootnamespace$.$subnamespace$
{
    public class SiteFeatureReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite siteCollection = (SPSite)properties.Feature.Parent;
            SPFeatureProperty masterUrlProperty = properties.Feature.Properties["MasterPage"];
            string masterUrl = masterUrlProperty.Value;

            if(String.IsNullOrEmpty(masterUrl) == false)
            {
                masterUrl = SPUrlUtility.CombineUrl(siteCollection.ServerRelativeUrl,
                    "_catalogs/masterpage/" + masterUrl);

                foreach(SPWeb site in siteCollection.AllWebs)
                {
                    site.MasterUrl = masterUrl;
                    site.CustomMasterUrl = masterUrl;
                    site.Update();
                }
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite siteCollection = (SPSite)properties.Feature.Parent;
            string masterUrl = SPUrlUtility.CombineUrl(siteCollection.ServerRelativeUrl,
                "_catalogs/masterpage/v4.master");

            foreach (SPWeb site in siteCollection.AllWebs)
            {
                site.MasterUrl = masterUrl;
                site.CustomMasterUrl = masterUrl;
                site.Update();
            }
        }
    }
}

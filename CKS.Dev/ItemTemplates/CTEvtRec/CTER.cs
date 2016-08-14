using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;

namespace $rootnamespace$ 
{
    [Guid("$guid1$")]
    public class $safeitemrootname$EventReceiver : SPItemEventReceiver 
    {
        /// <summary>
        /// Asynchronous After event that occurs after a new item has been added to its containing object.
        /// </summary>
        /// <param name="properties"></param>
        public override void ItemAdded(SPItemEventProperties properties) 
        {
            base.ItemAdded(properties);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Commands;
using Microsoft.SharePoint;

#if VS2012Build_SYMBOL
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
#elif VS2013Build_SYMBOL
    using CKS.Dev12.VisualStudio.SharePoint.Commands.Info;
#elif VS2014Build_SYMBOL
    using CKS.Dev13.VisualStudio.SharePoint.Commands.Info;
#else
    using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
#endif

#if VS2012Build_SYMBOL
namespace CKS.Dev11.VisualStudio.SharePoint.Commands
#elif VS2013Build_SYMBOL
    namespace CKS.Dev12.VisualStudio.SharePoint.Commands
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Commands
#else
    namespace CKS.Dev.VisualStudio.SharePoint.Commands
#endif
{
    /// <summary>
    /// List commands.
    /// </summary>
    public class ListSharePointCommands
    {
        /// <summary>
        /// Gets the list event receivers.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="listId">The list id.</param>
        /// <returns></returns>
        [SharePointCommand(ListEventReceiversCommandIds.GetListEventReceivers)]
        private EventReceiverInfo[] GetListEventReceivers(ISharePointCommandContext context, Guid listId)
        {
            SPList list = context.Web.Lists[listId];

            List<EventReceiverInfo> listEventReceivers = (from SPEventReceiverDefinition eventReceiver
                                                         in list.EventReceivers
                                                          select new EventReceiverInfo
                                                          {
                                                              ListId = listId,
                                                              Assembly = eventReceiver.Assembly,
                                                              Class = eventReceiver.Class,
                                                              EventType = (int)eventReceiver.Type,
                                                              Id = eventReceiver.Id,
                                                              Name = eventReceiver.Name
                                                          }).ToList();

            return listEventReceivers.ToArray();
        }

        /// <summary>
        /// Gets the list event receiver properties.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="eventReceiverInfo">The event receiver info.</param>
        /// <returns></returns>
        [SharePointCommand(ListEventReceiversCommandIds.GetListEventReceiverProperties)]
        private Dictionary<string, string> GetListEventReceiverProperties(ISharePointCommandContext context, EventReceiverInfo eventReceiverInfo)
        {
            SPEventReceiverDefinition eventReceiver = null;
            Dictionary<string, string> eventReceiverProperties = new Dictionary<string, string>();

            SPList list = context.Web.Lists[eventReceiverInfo.ListId];

            if (eventReceiverInfo.Id != Guid.Empty)
            {
                eventReceiver = list.EventReceivers[eventReceiverInfo.Id];
            }
            else if (!String.IsNullOrEmpty(eventReceiverInfo.Name))
            {
                eventReceiver = (from SPEventReceiverDefinition er in list.EventReceivers
                                 where er.Name.Equals(eventReceiverInfo.Name)
                                 select er).FirstOrDefault();
            }
            else
            {
                eventReceiver = (from SPEventReceiverDefinition er in list.EventReceivers
                                 where er.Assembly == eventReceiverInfo.Assembly &&
                                 er.Class == eventReceiverInfo.Class &&
                                 (int)er.Type == eventReceiverInfo.EventType
                                 select er).FirstOrDefault();
            }

            if (eventReceiver != null)
            {
                eventReceiverProperties = SharePointCommandServices.GetProperties(eventReceiver);
            }

            return eventReceiverProperties;
        }
    }
}

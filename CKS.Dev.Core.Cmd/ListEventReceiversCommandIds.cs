using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    /// SharePoint Command Ids for list event receivers.
    /// </summary>
    public static class ListEventReceiversCommandIds
    {
        #region Constants

        /// <summary>
        /// Gets properties of List Event Receiver.
        /// </summary>
        public const string GetListEventReceiverProperties = "ListEventReceivers.GetListEventReceiverProperties";

        /// <summary>
        /// Get Event Receivers associated with the List.
        /// </summary>
        public const string GetListEventReceivers = "ListEventReceivers.GetListEventReceivers";

        #endregion
    }
}

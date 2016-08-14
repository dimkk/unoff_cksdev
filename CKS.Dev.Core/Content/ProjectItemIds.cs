using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Content
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Content
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Content
#else
namespace CKS.Dev.VisualStudio.SharePoint.Content
#endif
{
    /// <summary>
    /// The SharePoint project item names
    /// </summary>
    public static class ProjectItemIds
    {
        /// <summary>
        /// The CustomActionGroup project item name
        /// </summary>
        public const string CustomActionGroup = "CKS.Dev.SharePoint.CustomActionGroup";

        /// <summary>
        /// The CustomAction project item name
        /// </summary>
        public const string CustomAction = "CKS.Dev.SharePoint.CustomAction";

        /// <summary>
        /// The DelegateControl project item name
        /// </summary>
        public const string DelegateControl = "CKS.Dev.SharePoint.DelegateControl";

        /// <summary>
        /// The HideCustomAction project item name
        /// </summary>
        public const string HideCustomAction = "CKS.Dev.SharePoint.HideCustomAction";

        /// <summary>
        /// The SandBoxedVisualWebPart project item name
        /// </summary>
        public const string SandBoxedVisualWebPart = "CKS.Dev.SharePoint.SandBoxedVisualWebPart";

        /// <summary>
        /// The FullTrustProxy project item name
        /// </summary>
        public const string FullTrustProxy = "CKS.Dev.SharePoint.FullTrustProxy";

        /// <summary>
        /// The SPMetalDefinition project item name
        /// </summary>
        public const string SPMetalDefinition = "CKS.Dev.SharePoint.SPMetalDefinition";

        /// <summary>
        /// The MasterPage project item name
        /// </summary>
        public const string MasterPage = "CKS.Dev.SharePoint.MasterPage";

        /// <summary>
        /// The PowerShellCmdLet project item name
        /// </summary>
        public const string PowerShellCmdLet = "CKS.Dev.SharePoint.PowerShellCmdLet";

        /// <summary>
        /// The WcfService project item name
        /// </summary>
        public const string WcfService = "CKS.Dev.SharePoint.WcfService";

        /// <summary>
        /// The Branding project item name
        /// </summary>
        public const string Branding = "CKS.Dev.SharePoint.Branding";

        /// <summary>
        /// The ServiceApplication project item name
        /// </summary>
        public const string ServiceApplication = "CKS.Dev.SharePoint.ServiceApplication";

        /// <summary>
        /// The CentralAdministrationPage project item name
        /// </summary>
        public const string CentralAdministrationPage = "CKS.Dev.SharePoint.CentralAdministrationPage";

        /// <summary>
        /// The AshxHandler project item name
        /// </summary>
        public const string AshxHandler = "CKS.Dev.SharePoint.AshxHandler";

        /// <summary>
        /// The Web template project item name
        /// </summary>
        public const string WebTemplate = "CKS.Dev.SharePoint.WebTemplate";

        /// <summary>
        /// The Content Type Binding project item name
        /// </summary>
        public const string ContentTypeBinding = "CKS.Dev.SharePoint.ContentTypeBinding";
    }
}

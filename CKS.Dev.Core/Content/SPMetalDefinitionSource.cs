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
    /// SPMetal definition sources.
    /// </summary>
    public enum SPMetalDefinitionSource
    {
        /// <summary>
        /// The current deployment site.
        /// </summary>
        CurrentDeploymentSite,
        /// <summary>
        /// A custom site.
        /// </summary>
        CustomSite
    }
}

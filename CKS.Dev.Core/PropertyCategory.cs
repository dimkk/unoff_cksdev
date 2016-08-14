using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint
#else
namespace CKS.Dev.VisualStudio.SharePoint
#endif
{
    /// <summary>
    /// The property category.
    /// </summary>
    public static class PropertyCategory
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        public const string DevTools = "CKS Developer Edition";
    }
}
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
    /// The available SharePoint versions
    /// </summary>
    enum SharePointVersion
    {
        /// <summary>
        /// The SP2010 version
        /// </summary>
        SP2010,
        /// <summary>
        /// The SP2013 version
        /// </summary>
        SP2013
    }
}

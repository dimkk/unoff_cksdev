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
    /// FullTrustProxy Operation Defintion.
    /// </summary>
    class FullTrustProxyOperationDefinition
    {
        #region Properties

        /// <summary>
        /// Gets or sets the full name of the class.
        /// </summary>
        /// <value>The full name of the class.</value>
        public string FullClassName { get; set; }

        #endregion
    }
}
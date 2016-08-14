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
    /// SPMetal defintion serialisation options.
    /// </summary>
    public enum SPMetalDefinitionSerialization
    {
        /// <summary>
        /// None.
        /// </summary>
        None,
        /// <summary>
        /// Uni directional.
        /// </summary>
        Unidirectional
    }
}
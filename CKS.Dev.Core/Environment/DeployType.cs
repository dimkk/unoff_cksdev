using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Environment
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Environment
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Environment
#else
namespace CKS.Dev.VisualStudio.SharePoint.Environment
#endif
{
    /// <summary>
    /// The deploy types.
    /// </summary>
    public enum DeployType
    {
        /// <summary>
        /// Copy to the SharePoint root.
        /// </summary>
        CopySharePointRoot = 1,
        /// <summary>
        /// Copy the binary.
        /// </summary>
        CopyBinary = 2,
        /// <summary>
        /// Copy both to the SharePoint root and the binary.
        /// </summary>
        CopyBoth = 3
    };
}
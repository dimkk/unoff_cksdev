using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Commands.Info
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Commands.Info
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Commands.Info
#else
    namespace CKS.Dev.VisualStudio.SharePoint.Commands.Info
#endif
    {
    /// <summary>
    /// The content type node information.
    /// </summary>
    [Serializable]
    public class ContentTypeNodeInfo : IContentTypeNodeInfo
    {
        /// <summary>
        /// Gets the name of the content type.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the content type.</returns>
        public string Name { get; set; }

    }
}

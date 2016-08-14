using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;

namespace CKS.Dev11.VisualStudio.SharePoint.Commands.Info
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Deployment.QuickDeployment
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Deployment.QuickDeployment
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Deployment.QuickDeployment
#else
namespace CKS.Dev.VisualStudio.SharePoint.Deployment.QuickDeployment
#endif
{
    /// <summary>
    /// A result container to allow multiple things to cross the marshaling boundary of the reflection.
    /// </summary>
    [Serializable]
    public class AssemblyInspectorResult
    {
        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>The messages.</value>
        public List<String> Messages { get; set; }

        /// <summary>
        /// Gets or sets the tokens.
        /// </summary>
        /// <value>The tokens.</value>
        public Dictionary<string, string> Tokens { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInspectorResult"/> class.
        /// </summary>
        public AssemblyInspectorResult()
        {
            Tokens = new Dictionary<string, string>();
            Messages = new List<string>();
        }

    }
}

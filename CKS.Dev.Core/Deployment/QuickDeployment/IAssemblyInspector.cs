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
    /// The AssemblyInspector interface.
    /// </summary>
    public interface IAssemblyInspector
    {
        /// <summary>
        /// Gets the replaceable GUID tokens.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <returns>The AssemblyInspectorResult</returns>
        AssemblyInspectorResult GetReplaceableGuidTokens(string assemblyPath);
    }
}


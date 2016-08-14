using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.QuickDeployment
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

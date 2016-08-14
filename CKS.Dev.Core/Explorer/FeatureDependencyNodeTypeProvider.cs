using CKSProperties = CKS.Dev.Core.Properties.Resources;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Explorer;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if VS2012Build_SYMBOL
using CKS.Dev11.VisualStudio.SharePoint.Commands;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev11.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev11.VisualStudio.SharePoint.Explorer.Dialogs;
#elif VS2013Build_SYMBOL
using CKS.Dev12.VisualStudio.SharePoint.Commands;
using CKS.Dev12.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev12.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev12.VisualStudio.SharePoint.Explorer.Dialogs;
#elif VS2014Build_SYMBOL
using CKS.Dev13.VisualStudio.SharePoint.Commands;
using CKS.Dev13.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev13.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev13.VisualStudio.SharePoint.Explorer.Dialogs;
#else
using CKS.Dev.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev.VisualStudio.SharePoint.Explorer.Dialogs;
#endif

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Explorer
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Explorer
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Explorer
#else
namespace CKS.Dev.VisualStudio.SharePoint.Explorer
#endif
{
    /// <summary>
    /// The feature dependency node type provider.
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(IExplorerNodeTypeProvider))]
    // Indicates that this class extends SharePoint nodes in Server Explorer.
    [ExplorerNodeType(ExplorerNodeIds.FeatureDependencyNode)]
    public class FeatureDependencyNodeTypeProvider : IExplorerNodeTypeProvider
    {
        #region Methods

        /// <summary>
        /// Creates the feature dependency nodes.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        internal static void CreateFeatureDependencyNodes(IExplorerNode parentNode)
        {
            IFeatureNodeInfo info = parentNode.ParentNode.Annotations.GetValue<IFeatureNodeInfo>();
            FeatureInfo featureDetails = new FeatureInfo()
            {
                FeatureID = info.Id
            };
            FeatureDependencyInfo[] dependencies =
                parentNode.Context.SharePointConnection.ExecuteCommand<FeatureInfo, FeatureDependencyInfo[]>(FeatureSharePointCommandIds.GetFeatureDependencies, featureDetails);
            foreach (FeatureDependencyInfo dependency in dependencies)
            {
                CreateNode(parentNode, dependency);
            }
        }

        /// <summary>
        /// Creates the node.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="dependency">The dependency.</param>
        /// <returns></returns>
        internal static IExplorerNode CreateNode(IExplorerNode parentNode, FeatureDependencyInfo dependency)
        {
            Dictionary<object, object> annotations = new Dictionary<object, object>();
            annotations.Add(typeof(FeatureDependencyInfo), dependency);
            return parentNode.ChildNodes.Add(ExplorerNodeIds.FeatureDependencyNode, String.Format("{0} ({1})", dependency.Title, dependency.MinimumVersion), annotations);
        }

        /// <summary>
        /// Initializes the new node type.
        /// </summary>
        /// <param name="typeDefinition">The definition of the new node type.</param>
        public void InitializeType(IExplorerNodeTypeDefinition typeDefinition)
        {
            typeDefinition.IsAlwaysLeaf = true;
        }

        #endregion
    }
}

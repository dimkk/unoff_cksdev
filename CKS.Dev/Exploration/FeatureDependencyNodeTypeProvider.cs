using System;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Explorer;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;
using CKS.Dev.VisualStudio.SharePoint.Exploration;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev.VisualStudio.SharePoint.Commands;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    /// <summary>
    /// Feature dependancy node extension.
    /// </summary>
    [Export(typeof(IExplorerNodeTypeProvider))]
    [ExplorerNodeType(ExplorerNodeIds.FeatureDependencyNode)]
    public class FeatureDependencyNodeTypeProvider : IExplorerNodeTypeProvider
    {
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
    }
}

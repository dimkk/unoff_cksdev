using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint;
using System.ComponentModel.Composition;

namespace CKS.Dev.VisualStudio.SharePoint.Content
{
    /// <summary>
    /// Branding item provider.
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(ISharePointProjectItemTypeProvider))]
    // Specifies the ID for this new project item type. This string must match the value of the 
    // Type attribute of the ProjectItem element in the .spdata file for the project item.
    [SharePointProjectItemType(ProjectItemIds.Branding)]
    // Specifies the icon to display with this project item in Solution Explorer.
    [SharePointProjectItemIcon("CKS.Dev.VisualStudio.SharePoint.Resources.SolutionExplorerIcons.Branding_SolutionExplorer.ico")]
    partial class BrandingTypeProvider : ISharePointProjectItemTypeProvider
    {
        /// <summary>
        /// Called by projects to initialize an instance of a SharePoint project item type.
        /// </summary>
        /// <param name="typeDefinition">A project item type definition to initialize.</param>
        public void InitializeType(ISharePointProjectItemTypeDefinition typeDefinition)
        {
            typeDefinition.Name = "Branding";
            typeDefinition.SupportedDeploymentScopes = SupportedDeploymentScopes.Site;
            typeDefinition.SupportedTrustLevels = SupportedTrustLevels.All;
            typeDefinition.SupportedAssemblyDeploymentTargets = SupportedAssemblyDeploymentTargets.All;
        }
    }
}
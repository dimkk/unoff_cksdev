using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint;
using System.ComponentModel.Composition;
using CKS.Dev11.VisualStudio.SharePoint.Properties;

namespace CKS.Dev11.VisualStudio.SharePoint.Content
{
    /// <summary>
    /// Central Administration Page item provider.
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(ISharePointProjectItemTypeProvider))]
    // Specifies the ID for this new project item type. This string must match the value of the 
    // Type attribute of the ProjectItem element in the .spdata file for the project item.
    [SharePointProjectItemType(ProjectItemIds.CentralAdministrationPage)]
    // Specifies the icon to display with this project item in Solution Explorer.
    [SharePointProjectItemIcon("CKS.Dev11.VisualStudio.SharePoint.Resources.SolutionExplorerIcons.CentralAdministrationPage_SolutionExplorer.ico")]
    partial class CentralAdminPageTypeProvider : ISharePointProjectItemTypeProvider
    {
        #region Methods

        /// <summary>
        /// Called by projects to initialize an instance of a SharePoint project item type.
        /// </summary>
        /// <param name="typeDefinition">A project item type definition to initialize.</param>
        public void InitializeType(ISharePointProjectItemTypeDefinition typeDefinition)
        {
            typeDefinition.Name = Resources.CentralAdminPageTypeProvider_TypeDefinitionName;
            typeDefinition.SupportedDeploymentScopes = SupportedDeploymentScopes.Farm;
            typeDefinition.SupportedTrustLevels = SupportedTrustLevels.FullTrust;
            typeDefinition.SupportedAssemblyDeploymentTargets = SupportedAssemblyDeploymentTargets.All;
        }

        #endregion
    }
}
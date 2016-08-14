using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint;
using CKSProperties = CKS.Dev.Core.Properties.Resources;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Content
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Content
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Content
#else
namespace CKS.Dev.VisualStudio.SharePoint.Content
#endif
{
    /// <summary>
    /// Full Trust Proxy item provider
    /// </summary>
    [Export(typeof(ISharePointProjectItemTypeProvider))]
    [SharePointProjectItemType(ProjectItemIds.FullTrustProxy)]
    // Specifies the icon to display with this project item in Solution Explorer.
    [SharePointProjectItemIcon("CKS.Dev.VisualStudio.SharePoint.Resources.SolutionExplorerIcons.FullTrustProxy_SolutionExplorer.ico")]
    class FullTrustProxyTypeProvider : ISharePointProjectItemTypeProvider
    {
        #region Fields

        IServiceProvider _serviceProvider;

        #endregion

        #region Methods

        /// <summary>
        /// Called by projects to initialize an instance of a SharePoint project item type.
        /// </summary>
        /// <param name="typeDefinition">A project item type definition to initialize.</param>
        public void InitializeType(ISharePointProjectItemTypeDefinition typeDefinition)
        {
            _serviceProvider = typeDefinition.ProjectService.ServiceProvider;
            typeDefinition.Name = CKSProperties.FullTrustProxyTypeProvider_TypeDefinitionName;
            typeDefinition.SupportedDeploymentScopes = SupportedDeploymentScopes.Farm;
            typeDefinition.SupportedTrustLevels = SupportedTrustLevels.FullTrust;
            typeDefinition.SupportedAssemblyDeploymentTargets = SupportedAssemblyDeploymentTargets.GlobalAssemblyCache;
            typeDefinition.ProjectItemInitialized += new EventHandler<SharePointProjectItemEventArgs>(typeDefinition_ProjectItemInitialized);
            typeDefinition.ProjectItemAdded += new EventHandler<SharePointProjectItemEventArgs>(TypeDefinition_ProjectItemAdded);
            typeDefinition.ProjectItemPropertiesRequested += new EventHandler<SharePointProjectItemPropertiesRequestedEventArgs>(TypeDefinition_ProjectItemPropertiesRequested);
            FullTrustProxyRefactoring.StartListening(typeDefinition);
        }

        /// <summary>
        /// Handles the ProjectItemPropertiesRequested event of the TypeDefinition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.SharePoint.SharePointProjectItemPropertiesRequestedEventArgs"/> instance containing the event data.</param>
        void TypeDefinition_ProjectItemPropertiesRequested(object sender, SharePointProjectItemPropertiesRequestedEventArgs e)
        {
            e.PropertySources.Add(new FullTrustProxyProperties(this, e.ProjectItem));
        }

        /// <summary>
        /// Handles the ProjectItemAdded event of the TypeDefinition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.SharePoint.SharePointProjectItemEventArgs"/> instance containing the event data.</param>
        void TypeDefinition_ProjectItemAdded(object sender, SharePointProjectItemEventArgs e)
        {

        }

        /// <summary>
        /// Handles the ProjectItemInitialized event of the typeDefinition control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.SharePoint.SharePointProjectItemEventArgs"/> instance containing the event data.</param>
        void typeDefinition_ProjectItemInitialized(object sender, SharePointProjectItemEventArgs e)
        {

        }

        #endregion
    }
}
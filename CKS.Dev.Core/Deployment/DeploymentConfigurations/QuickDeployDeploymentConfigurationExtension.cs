using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKSProperties = CKS.Dev.Core.Properties.Resources;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;

#if VS2012Build_SYMBOL
using CKS.Dev11.VisualStudio.SharePoint.Deployment.DeploymentSteps;
#elif VS2013Build_SYMBOL
using CKS.Dev12.VisualStudio.SharePoint.Deployment.DeploymentSteps;
#elif VS2014Build_SYMBOL
using CKS.Dev13.VisualStudio.SharePoint.Deployment.DeploymentSteps;
#else
using CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps;
#endif

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Deployment.DeploymentConfigurations
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Deployment.DeploymentConfigurations
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Deployment.DeploymentConfigurations
#else
namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentConfigurations
#endif
{
    /// <summary>
    /// Deployment configuration for quick deploy.
    /// </summary>
    [Export(typeof(ISharePointProjectExtension))]
    internal class QuickDeployDeploymentConfigurationExtension : ISharePointProjectExtension
    {
        private const string name = "Quick Deploy (CKSDev)";

        /// <summary>
        /// Initializes the SharePoint project extension.
        /// </summary>
        /// <param name="projectService">An instance of SharePoint project service.</param>
        public void Initialize(ISharePointProjectService projectService)
        {
            projectService.ProjectInitialized += ProjectInitialized;
        }

        /// <summary>
        /// Creates the new deployment configuration.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.SharePoint.SharePointProjectEventArgs"/> instance containing the event data.</param>
        private void ProjectInitialized(object sender, SharePointProjectEventArgs e)
        {
            //Add the new configuration.
            if (!e.Project.DeploymentConfigurations.ContainsKey(CKSProperties.UpgradeDeploymentConfigurationExtension_Name))
            {
                string[] deploymentSteps = new string[] 
                {
                    DeploymentStepIds.PreDeploymentCommand,
                    DeploymentStepIds.RecycleApplicationPool,
                    CustomDeploymentStepIds.CopyBinaries,
                    CustomDeploymentStepIds.CopyToSharePointRoot,
                    DeploymentStepIds.PostDeploymentCommand 
                };

                string[] retractionSteps = new string[] 
                {
                    DeploymentStepIds.RecycleApplicationPool            
                };

                IDeploymentConfiguration configuration = e.Project.DeploymentConfigurations.Add(
                    CKSProperties.QuickDeployDeploymentConfigurationExtension_Name, deploymentSteps, retractionSteps);
                configuration.Description = CKSProperties.QuickDeployDeploymentConfigurationExtension_Description;
            }
        }
    }
}
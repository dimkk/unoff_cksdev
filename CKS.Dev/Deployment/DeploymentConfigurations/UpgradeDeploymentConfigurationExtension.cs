using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;
using System.ComponentModel.Composition;
using CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentConfigurations
{
    /// <summary>
    /// Deployment configuration for upgrade.
    /// </summary>
    [Export(typeof(ISharePointProjectExtension))]
    internal class UpgradeDeploymentConfigurationExtension : ISharePointProjectExtension
    {
        /// <summary>
        /// The old name of the configuration. This is so that early deployment configs are still supported.
        /// </summary>
        private const string nameOld = "Upgrade (CKSDev)";

        /// <summary>
        /// The name of the configuration.
        /// </summary>
        private const string name = "Upgrade Solution (CKSDev)";

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
            //Processing of the old deployment config
            if (e.Project.DeploymentConfigurations.ContainsKey(nameOld))
            {
                e.Project.DeploymentConfigurations.Remove(nameOld);
            }
                  
            //Add the new configuration.
            if (!e.Project.DeploymentConfigurations.ContainsKey(name))
            {
                string[] deploymentSteps = new string[] 
                {
                    DeploymentStepIds.PreDeploymentCommand,
                    DeploymentStepIds.RecycleApplicationPool,
                    CustomDeploymentStepIds.UpgradeSolution,
                    DeploymentStepIds.PostDeploymentCommand 
                };

                string[] retractionSteps = new string[] 
                {
                    DeploymentStepIds.RecycleApplicationPool,
                    DeploymentStepIds.RetractSolution                
                };

                IDeploymentConfiguration configuration = e.Project.DeploymentConfigurations.Add(
                    name, deploymentSteps, retractionSteps);
                configuration.Description = "This is the Upgrade Solution deployment configuration";
            }

            //Update the active configuration if it was the old one.
            if (e.Project.ActiveDeploymentConfiguration == nameOld)
            {
                e.Project.ActiveDeploymentConfiguration = name;
            }
        }
    }
}
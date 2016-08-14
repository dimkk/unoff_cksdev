using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;
using System.ComponentModel.Composition;
using CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentConfigurations
{
    /// <summary>
    /// Deployment configuration to quick deploy assemblies
    /// </summary>
    [Export(typeof(ISharePointProjectExtension))]
    internal class QuickDeployAssembliesDeploymentConfigurationExtension : ISharePointProjectExtension
    {
        private const string name = "Quick Deploy (GAC/BIN Only) (CKSDev)";

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
            if (e.Project.DeploymentConfigurations.ContainsKey(name))
            {
                //Check the steps for the old references
                if (e.Project.DeploymentConfigurations[name].DeploymentSteps.Contains("CKSDEV.DeploymentSteps.CopyBinaries"))
                {
                    //If found remove this config so it gets refreshed below
                    e.Project.DeploymentConfigurations.Remove(name);
                }
            }

            if (!e.Project.DeploymentConfigurations.ContainsKey(name))
            {
                string[] deploymentSteps = new string[] 
                {
                    DeploymentStepIds.RecycleApplicationPool,
                    CustomDeploymentStepIds.CopyBinaries
                };

                string[] retractionSteps = new string[] 
                {
                };

                IDeploymentConfiguration configuration = e.Project.DeploymentConfigurations.Add(
                    name, deploymentSteps, retractionSteps);
                configuration.Description = "This is the Quick Deploy (GAC/BIN Only) deployment configuration";
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKS.Dev11.VisualStudio.SharePoint.Deployment.DeploymentSteps;
using CKS.Dev11.VisualStudio.SharePoint.Properties;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;

namespace CKS.Dev11.VisualStudio.SharePoint.Deployment.DeploymentConfigurations
{
    /// <summary>
    /// Deployment configuration for upgrade.
    /// </summary>
    [Export(typeof(ISharePointProjectExtension))]
    internal class UpgradeDeploymentConfigurationExtension : ISharePointProjectExtension
    {
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
            if (!e.Project.DeploymentConfigurations.ContainsKey(Resources.UpgradeDeploymentConfigurationExtension_Name))
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
                    Resources.UpgradeDeploymentConfigurationExtension_Name, deploymentSteps, retractionSteps);
                configuration.Description = Resources.UpgradeDeploymentConfigurationExtension_Description;
            }
        }
    }
}

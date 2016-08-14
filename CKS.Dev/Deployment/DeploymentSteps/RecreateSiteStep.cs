using System;
using Microsoft.VisualStudio.SharePoint;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Deployment;
using CKS.Dev.VisualStudio.SharePoint.Commands;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps
{
    /// <summary>
    /// Recreate the site deployment step.
    /// </summary>
    [Export(typeof(IDeploymentStep))]
    [DeploymentStep(CustomDeploymentStepIds.RecreateSite)]
    public class RecreateSiteStep
        : IDeploymentStep
    {
        /// <summary>
        /// Initializes the deployment step.
        /// </summary>
        /// <param name="stepInfo">An object that contains information about the deployment step.</param>
        public void Initialize(IDeploymentStepInfo stepInfo)
        {
            stepInfo.Name = "Recreate site (CKSDev)";
            stepInfo.StatusBarMessage = "Recreating site...";
            stepInfo.Description = "Recreates the deployment site";
        }

        /// <summary>
        /// Determines whether the deployment step can be executed in the current context.
        /// </summary>
        /// <param name="context">An object that provides information you can use to determine the context in which the deployment step is executing.</param>
        /// <returns>
        /// true if the deployment step can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(IDeploymentContext context)
        {
            return context.Project.SharePointConnection.ExecuteCommand<bool>(DeploymentSharePointCommandIds.CanCreateSite);
        }

        /// <summary>
        /// Executes the deployment step.
        /// </summary>
        /// <param name="context">An object that provides information you can use to determine the context in which the deployment step is executing.</param>
        public void Execute(IDeploymentContext context)
        {
            ProjectProperties properties = context.Project.Annotations.GetValue<ProjectProperties>();
            context.Project.SharePointConnection.ExecuteCommand<string>(DeploymentSharePointCommandIds.RecreateSite,
                properties != null ? properties.SiteDefinition : null);
        }
    }
}
using System;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;
using System.ComponentModel.Composition;
using CKS.Dev.VisualStudio.SharePoint.Deployment.QuickDeployment;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps
{
    /// <summary>
    /// Defines a new deployment step that can be used to "Restart IIS".
    /// </summary> 
    // Enables Visual Studio to discover and load this deployment step.
    [Export(typeof(IDeploymentStep))]
    // Specifies the ID for this new deployment step.
    [DeploymentStep(CustomDeploymentStepIds.RestartIis)]
    internal class RestartIisStep : IDeploymentStep
    {
        // Implements IDeploymentStep.Initialize.
        public void Initialize(IDeploymentStepInfo stepInfo)
        {
            stepInfo.Name = "Restart IIS (CKSDev)";
            stepInfo.StatusBarMessage = "Restarting IIS...";
            stepInfo.Description = "Restarts IIS on the local machine.";
        }

        // Implements IDeploymentStep.CanExecute. Specifies whether the solution can be upgraded.
        public bool CanExecute(IDeploymentContext context)
        {
            if (context.IsRetracting)
            {
                string sandboxMessage = "Restart IIS cannot Retract.";
                context.Logger.WriteLine(sandboxMessage, LogCategory.Error);
                throw new InvalidOperationException(sandboxMessage);
            }

            return true;
        }

        // Implements IDeploymentStep.Execute.
        public void Execute(IDeploymentContext context)
        {
            if (context.IsDeploying)
            {
                RecycleUtilities.RestartIIS();
            }
        }
    }
}
using System;
using System.Linq;
using System.ServiceProcess;
using Microsoft.VisualStudio.SharePoint;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Deployment;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps
{
    /// <summary>
    /// Reset timer service deployment step.
    /// </summary>
    [Export(typeof(IDeploymentStep))]
    [DeploymentStep(CustomDeploymentStepIds.ResetTimerService)]
    public class ResetTimerServiceStep
        : IDeploymentStep
    {
        const string TimerServiceName = "SPTimerV4";
        bool? _canExecute;

        /// <summary>
        /// Initializes the deployment step.
        /// </summary>
        /// <param name="stepInfo">An object that contains information about the deployment step.</param>
        public void Initialize(IDeploymentStepInfo stepInfo)
        {
            stepInfo.Name = "Reset Timer Service (CKSDev)";
            stepInfo.StatusBarMessage = "Resetting Timer Service...";
            stepInfo.Description = "Resets the SharePoint Timer Service.";
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
            if (_canExecute == null)
            {
                _canExecute = ServiceController.GetServices().Where(
                    sc => sc.ServiceName == TimerServiceName).FirstOrDefault() != null;
            }
            if (_canExecute == false)
            {
                context.Logger.WriteLine("Skipping step because the timer service is not installed on the local machine.", LogCategory.Status);
            }
            return _canExecute.Value;
        }

        /// <summary>
        /// Executes the deployment step.
        /// </summary>
        /// <param name="context">An object that provides information you can use to determine the context in which the deployment step is executing.</param>
        public void Execute(IDeploymentContext context)
        {
            ServiceController controller = new ServiceController("SPTimerV4");
            try
            {
                controller.Stop();
                controller.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 1, 0));
            }
            finally
            {
                if (controller.Status == ServiceControllerStatus.Stopped)
                {
                    controller.Start();
                }
            }
        }
    }
}

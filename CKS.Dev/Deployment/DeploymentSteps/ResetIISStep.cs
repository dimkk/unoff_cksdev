using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.SharePoint.Deployment;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps
{
    /// <summary>
    /// Reset IIS deployment step.
    /// </summary>
    [Export(typeof(IDeploymentStep))]
    [DeploymentStep(CustomDeploymentStepIds.ResetIIS)]
    public class ResetIISStep : IDeploymentStep
    {
        /// <summary>
        /// Gets the IIS reset path.
        /// </summary>
        /// <value>The IIS reset path.</value>
        string IISResetPath
        {
            get
            {
                return Path.Combine(
                    System.Environment.SystemDirectory, "iisreset.exe");
            }
        }

        /// <summary>
        /// Initializes the deployment step.
        /// </summary>
        /// <param name="stepInfo">An object that contains information about the deployment step.</param>
        public void Initialize(IDeploymentStepInfo stepInfo)
        {
            stepInfo.Name = "Reset IIS (CKSDev)";
            stepInfo.StatusBarMessage = "Resetting IIS...";
            stepInfo.Description = "Resets the Internet Information Server.";
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
            bool canExecute = File.Exists(IISResetPath);
            if (canExecute == false)
            {
                context.Logger.WriteLine("Skipping step because IISReset.exe can not be found.", LogCategory.Status);
            }
            return canExecute;
        }

        /// <summary>
        /// Executes the deployment step.
        /// </summary>
        /// <param name="context">An object that provides information you can use to determine the context in which the deployment step is executing.</param>
        public void Execute(IDeploymentContext context)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo(IISResetPath);
            Process process = Process.Start(processInfo);
            process.WaitForExit();
        }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKS.Dev11.VisualStudio.SharePoint.Environment;
using CKS.Dev11.VisualStudio.SharePoint.Properties;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;

namespace CKS.Dev11.VisualStudio.SharePoint.Deployment.DeploymentSteps
{
    /// <summary>
    /// Defines a new deployment step that can be used to "Restart IIS".
    /// </summary> 
    // Enables Visual Studio to discover and load this deployment step.
    [Export(typeof(IDeploymentStep))]
    // Specifies the ID for this new deployment step.
    [DeploymentStep(CustomDeploymentStepIds.RestartIis)]
    internal class RestartIisStep 
        : IDeploymentStep
    {
        /// <summary>
        /// Invoked when the step is first initialized. This happens only once per VS instance.
        /// </summary>
        /// <param name="stepInfo">Represents the information about the step - The step is expected to set
        /// the properties in <paramref name="stepInfo" /> on initialization.</param>
        /// <remarks>
        /// This method is executed in the UI thread.
        /// </remarks>
        public void Initialize(IDeploymentStepInfo stepInfo)
        {
            stepInfo.Name = Resources.RestartIisStep_Name;
            stepInfo.StatusBarMessage = Resources.RestartIisStep_StatusBarMessage;
            stepInfo.Description = Resources.RestartIisStep_Description;
        }

        /// <summary>
        /// Determines if the step can be executed in the current context.
        /// </summary>
        /// <param name="context">The step context that can be used to access project related properties.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <remarks>
        /// This method is executed in a background thread.
        /// </remarks>
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

        /// <summary>
        /// Entry point for execution of the step.
        /// </summary>
        /// <param name="context">The step context that can be used to access project related properties.</param>
        /// <remarks>
        /// This method is executed in a background thread.
        /// </remarks>
        public void Execute(IDeploymentContext context)
        {
            if (context.IsDeploying)
            {
                new ProcessUtilities().RestartIIS(context.Project);
            }
        }
    }
}
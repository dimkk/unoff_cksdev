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
    /// Attach to the User Code Process.
    /// </summary>
    [Export(typeof(IDeploymentStep))]
    [DeploymentStep(CustomDeploymentStepIds.AttachToSPUCWorkerProcess)]
    public class AttachToSPUCWorkerProcessStep 
        : IDeploymentStep
    {
        /// <summary>
        /// Initializes the deployment step.
        /// </summary>
        /// <param name="stepInfo">An object that contains information about the deployment step.</param>
        public void Initialize(IDeploymentStepInfo stepInfo)
        {
            stepInfo.Name = Resources.AttachToSPUCWorkerProcessStep_Name;
            stepInfo.StatusBarMessage = Resources.AttachToSPUCWorkerProcessStep_StatusBarMessage;
            stepInfo.Description = Resources.AttachToSPUCWorkerProcessStep_Description;
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
            bool? _canExecute = null;

            if (_canExecute == null)
            {
                _canExecute = new ProcessUtilities(context.Project.ProjectService.Convert<ISharePointProject, EnvDTE.Project>(context.Project).DTE).IsProcessAvailableByName(ProcessConstants.SPUCWorkerProcess);
            }
            if (_canExecute == false)
            {
                context.Logger.WriteLine("Skipping step because the User Code Service process is not running on the local machine.", LogCategory.Status);
            }
            return _canExecute.Value;
        }

        /// <summary>
        /// Executes the deployment step.
        /// </summary>
        /// <param name="context">An object that provides information you can use to determine the context in which the deployment step is executing.</param>
        public void Execute(IDeploymentContext context)
        {
            new ProcessUtilities(context.Project.ProjectService.Convert<ISharePointProject, EnvDTE.Project>(context.Project).DTE).AttachToProcessByName(ProcessConstants.SPUCWorkerProcess);
        }
    }
}


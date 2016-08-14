using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint.Deployment;
using Microsoft.VisualStudio.SharePoint;
using CKS.Dev.VisualStudio.SharePoint.Environment;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps
{
    /// <summary>
    /// Attach to the IIS Worker Processes.
    /// </summary>
    [Export(typeof(IDeploymentStep))]
    [DeploymentStep(CustomDeploymentStepIds.AttachToIISWorkerProcesses)]
    public class AttachToIISWorkerProcessesStep : IDeploymentStep
    {
        /// <summary>
        /// Initializes the deployment step.
        /// </summary>
        /// <param name="stepInfo">An object that contains information about the deployment step.</param>
        public void Initialize(IDeploymentStepInfo stepInfo)
        {
            stepInfo.Name = "Attach to IIS worker processes (CKSDev)";
            stepInfo.StatusBarMessage = "Attaching to the IIS worker processes...";
            stepInfo.Description = "Attachs the debugger to the IIS worker processes.";
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
            return true;
        }

        /// <summary>
        /// Executes the deployment step.
        /// </summary>
        /// <param name="context">An object that provides information you can use to determine the context in which the deployment step is executing.</param>
        public void Execute(IDeploymentContext context)
        {
            ISharePointProject project = context.Project;
            EnvDTE.Project dteProj = project.ProjectService.Convert<ISharePointProject, EnvDTE.Project>(project);

            ProcessUtilities utils = new ProcessUtilities(dteProj.DTE);
            
            utils.AttachToProcessByName(project, ProcessConstants.IISWorkerProcess);
        }
    }
}

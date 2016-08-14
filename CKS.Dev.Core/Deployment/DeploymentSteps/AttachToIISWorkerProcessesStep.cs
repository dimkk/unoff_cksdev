using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKSProperties = CKS.Dev.Core.Properties.Resources;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;

#if VS2012Build_SYMBOL
using CKS.Dev11.VisualStudio.SharePoint.Environment;
#elif VS2013Build_SYMBOL
using CKS.Dev12.VisualStudio.SharePoint.Environment;
#elif VS2014Build_SYMBOL
using CKS.Dev13.VisualStudio.SharePoint.Environment;
#else
using CKS.Dev.VisualStudio.SharePoint.Environment;
#endif

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Deployment.DeploymentSteps
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Deployment.DeploymentSteps
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Deployment.DeploymentSteps
#else
namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps
#endif
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
            stepInfo.Name = CKSProperties.AttachToIISWorkerProcessesStep_Name;
            stepInfo.StatusBarMessage = CKSProperties.AttachToIISWorkerProcessesStep_StatusBarMessage;
            stepInfo.Description = CKSProperties.AttachToIISWorkerProcessesStep_Description;
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
                _canExecute = new ProcessUtilities(context.Project.ProjectService.Convert<ISharePointProject, EnvDTE.Project>(context.Project).DTE).IsProcessAvailableByName(ProcessConstants.IISWorkerProcess);
            }
            if (_canExecute == false)
            {
                context.Logger.WriteLine("Skipping step because the IIS Worker process is not running on the local machine.", LogCategory.Status);
            }
            return _canExecute.Value;
        }

        /// <summary>
        /// Executes the deployment step.
        /// </summary>
        /// <param name="context">An object that provides information you can use to determine the context in which the deployment step is executing.</param>
        public void Execute(IDeploymentContext context)
        {
            new ProcessUtilities(context.Project.ProjectService.Convert<ISharePointProject, EnvDTE.Project>(context.Project).DTE).AttachToProcessByName(ProcessConstants.IISWorkerProcess);
        }
    }
}

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
            stepInfo.Name = CKSProperties.RestartIisStep_Name;
            stepInfo.StatusBarMessage = CKSProperties.RestartIisStep_StatusBarMessage;
            stepInfo.Description = CKSProperties.RestartIisStep_Description;
        }

        /// <summary>
        /// Determines if the step can be executed in the current context.
        /// </summary>
        /// <param name="context">The step context that can be used to access project related CKSProperties.</param>
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
        /// <param name="context">The step context that can be used to access project related CKSProperties.</param>
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
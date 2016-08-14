using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using Microsoft.Build.Evaluation;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using System.IO;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps.RunPowerShellScript
{
    /// <summary>
    /// Run powershell deployment step
    /// </summary>
    [Export(typeof(IDeploymentStep))]
    [DeploymentStep(CustomDeploymentStepIds.RunPowerShellScriptDeploymentStep)]
    public class RunPowerShellScriptDeploymentStep : IDeploymentStep
    {
        private const string DeploymentStepName = "Run PowerShell Script (CKSDev)";
        private ISharePointProjectLogger logger;

        /// <summary>
        /// Determines whether the deployment step can be executed in the current context.
        /// </summary>
        /// <param name="context">An object that provides information you can use to determine the context in which the deployment step is executing.</param>
        /// <returns>
        /// true if the deployment step can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(IDeploymentContext context)
        {
            return File.Exists(Path.Combine(System.Environment.GetEnvironmentVariable("windir"), @"sysnative\WindowsPowerShell\v1.0\powershell.exe"));
        }

        /// <summary>
        /// Executes the deployment step.
        /// </summary>
        /// <param name="context">An object that provides information you can use to determine the context in which the deployment step is executing.</param>
        public void Execute(IDeploymentContext context)
        {
            logger = context.Logger;

            int numPSTasks = context.Project.DeploymentConfigurations[context.Project.ActiveDeploymentConfiguration].DeploymentSteps.Count(p => p.Equals(CustomDeploymentStepIds.RunPowerShellScriptDeploymentStep));
            int currentPSTask = 0;
            context.Project.Annotations.TryGetValue<int>(CustomDeploymentStepIds.RunPowerShellScriptDeploymentStep, out currentPSTask);
            currentPSTask++;

            string key = GetSettingKey(context.Project, currentPSTask);
            Microsoft.Build.Evaluation.Project project = ProjectFileDataPropertyDescriptor.GetCurrentProject(context.Project.FullPath);
            if (project != null)
            {
                string script = project.GetPropertyValue(ProjectFileDataPropertyDescriptor.EscapeMsBuildString(key));
                if (String.IsNullOrEmpty(script))
                {
                    logger.WriteLine(String.Format(Resources.RunPowerShellScriptDeploymentStep_NoPSScriptFound, currentPSTask), LogCategory.Error);
                }
                else
                {
                    Process cmd = new Process();
                    cmd.StartInfo = new ProcessStartInfo
                    {
                        Arguments = String.Format("{0}", script),
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        FileName = Path.Combine(System.Environment.GetEnvironmentVariable("windir"), @"sysnative\WindowsPowerShell\v1.0\powershell.exe")
                    };
                    logger.WriteLine(String.Format(Resources.RunPowerShellScriptDeploymentStep_ExecutingPSScript, currentPSTask, script), LogCategory.Message);
                    cmd.Start();
                    cmd.WaitForExit();
                    if (cmd.ExitCode != 0)
                    {
                        logger.WriteLine(cmd.StandardError.ReadToEnd(), LogCategory.Error);
                    }
                    else
                    {
                        logger.WriteLine(cmd.StandardOutput.ReadToEnd(), LogCategory.Message);
                    }
                    logger.WriteLine(String.Format(Resources.RunPowerShellScriptDeploymentStep_PSScriptSuccessfullyExecuted, script), LogCategory.Status);
                }
            }
            else
            {
                logger.WriteLine(String.Format(Resources.RunPowerShellScriptDeploymentStep_ErrorCouldNotLoadProject, context.Project.FullPath), LogCategory.Error);
            }

            context.Project.Annotations[CustomDeploymentStepIds.RunPowerShellScriptDeploymentStep] = currentPSTask;

            if (currentPSTask == numPSTasks)
            {
                context.Project.Annotations.Remove(CustomDeploymentStepIds.RunPowerShellScriptDeploymentStep);
            }
        }

        /// <summary>
        /// Initialise the step.
        /// </summary>
        /// <param name="stepInfo">The deployment step information.</param>
        public void Initialize(IDeploymentStepInfo stepInfo)
        {
            stepInfo.Name = DeploymentStepName;
            stepInfo.StatusBarMessage = Resources.RunPowerShellScriptDeploymentStep_StatusBarMessage;
            stepInfo.Description = Resources.RunPowerShellScriptDeploymentStep_StepDescription;
        }

        /// <summary>
        /// Get the setting key.
        /// </summary>
        /// <param name="project">The SahrePoint project.</param>
        /// <param name="scriptNo">The script number.</param>
        /// <returns>The setting key.</returns>
        public static string GetSettingKey(ISharePointProject project, int scriptNo)
        {
            return String.Format("CKSDEV{0}PowerShell{1}", project.ActiveDeploymentConfiguration, scriptNo);
        }
    }
}

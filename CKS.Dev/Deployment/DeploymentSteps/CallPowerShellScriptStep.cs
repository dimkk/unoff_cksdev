﻿using System;
using System.ComponentModel.Composition;
using System.Security;
using CKS.Dev.VisualStudio.SharePoint.Exploration;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;
using Microsoft.Win32;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps
{
    /// <summary>
    /// Call powershell script deployment step.
    /// </summary>
    [Export(typeof(IDeploymentStep))]
    [DeploymentStep(CustomDeploymentStepIds.CallPowerShellScript)]
    public class CallPowerShellScriptStep
        : IDeploymentStep
    {
        /// <summary>
        /// Initializes the deployment step.
        /// </summary>
        /// <param name="stepInfo">An object that contains information about the deployment step.</param>
        public void Initialize(IDeploymentStepInfo stepInfo)
        {
            stepInfo.Name = "Call PowerShell Script (CKSDev)";
            stepInfo.StatusBarMessage = "Calling script...";
            stepInfo.Description = "Calls a PowerShell script";
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
            bool canExecute = false;
            ProjectProperties properties = context.Project.Annotations.GetValue<ProjectProperties>();
            bool hasScript = properties != null && String.IsNullOrEmpty(properties.ScriptName) == false;
            if (hasScript)
            {
                string powershellKeyPath = @"SOFTWARE\Microsoft\PowerShell\1";
                try
                {
                    RegistryKey powerShellKey = Registry.LocalMachine.OpenSubKey(powershellKeyPath);
                    if (powerShellKey != null)
                    {
                        object installValueObject = powerShellKey.GetValue("Install");
                        try
                        {
                            int installValue = Convert.ToInt32(installValueObject);
                            canExecute = installValue == 1;
                        }
                        catch (FormatException)
                        {
                            context.Logger.WriteLine(
                                @"Unexpected data in PowerShell registry subkey.",
                                LogCategory.Error);
                        }
                    }
                    else
                    {
                        context.Logger.WriteLine(
                            @"PowerShell not installed.",
                            LogCategory.Warning);
                    }
                }
                catch (SecurityException)
                {
                    context.Logger.WriteLine(
                        @"Access to registry key denied: HKLM\" + powershellKeyPath,
                        LogCategory.Error);
                }
            }
            else
            {
                context.Logger.WriteLine(
                    @"No PowerShell script configured for execution.",
                    LogCategory.Warning);
            }
            if (canExecute == false)
            {
                context.Logger.WriteLine(
                    "Skipping PowerShell script deployment step.",
                    LogCategory.Warning);
            }
            return canExecute;
        }

        /// <summary>
        /// Executes the deployment step.
        /// </summary>
        /// <param name="context">An object that provides information you can use to determine the context in which the deployment step is executing.</param>
        public void Execute(IDeploymentContext context)
        {
            ProjectProperties properties = context.Project.Annotations.GetValue<ProjectProperties>();
            string script = properties.ScriptName;
            context.Logger.WriteLine("Executing script " + script, LogCategory.Status);
            //    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell");
            //powershell c:\pathtoscript\SingFile.ps1
        }
    }
}

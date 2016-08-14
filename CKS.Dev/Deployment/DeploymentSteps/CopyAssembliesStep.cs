using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;
using Microsoft.VisualStudio.SharePoint.Packages;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Exploration;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps
{
    /// <summary>
    /// Copy assemblies deployment step.
    /// </summary>
    [Export(typeof(IDeploymentStep))]
    [DeploymentStep(CustomDeploymentStepIds.CopyAssemblies)]
    public class CopyAssembliesStep
        : IDeploymentStep
    {
        /// <summary>
        /// Initializes the deployment step.
        /// </summary>
        /// <param name="stepInfo">An object that contains information about the deployment step.</param>
        public void Initialize(IDeploymentStepInfo stepInfo)
        {
            stepInfo.Name = "Copy Assemblies (CKSDev)";
            stepInfo.StatusBarMessage = "Copying Assemblies...";
            stepInfo.Description = "Copies packaged assemblies to all Web Application folders and to the Global Assembly Cache depending on the assembly deployment type.";
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
            bool canExecute = context.Project.IsSandboxedSolution == false;
            if (canExecute == false)
            {
                context.Logger.WriteLine("Skipping step because the project is configured to deploy into the solution sandbox.", LogCategory.Status);
            }
            return canExecute;
        }

        /// <summary>
        /// Executes the deployment step.
        /// </summary>
        /// <param name="context">An object that provides information you can use to determine the context in which the deployment step is executing.</param>
        public void Execute(IDeploymentContext context)
        {
            string[] binPaths = context.Project.SharePointConnection.ExecuteCommand<string[]>(DeploymentSharePointCommandIds.GetWebApplicationPhysicalPaths);

            IPackage package = context.Project.Package.Model;

            foreach (ISharePointProject project in context.Project.Package.ProjectItems
                .Select(item => item.Project)
                .Distinct())
            {
                TryInstallProjectAssembly(binPaths, project);
            }
            foreach (IAssembly assembly in package.Assemblies)
            {
                string sourcePath = null;
                AssemblyDeploymentTarget deploymentTarget = AssemblyDeploymentTarget.GlobalAssemblyCache;
                ICustomAssembly customAssembly = assembly as ICustomAssembly;
                if (customAssembly != null)
                {
                    sourcePath = customAssembly.Location;
                    deploymentTarget = (AssemblyDeploymentTarget)Enum.Parse(typeof(AssemblyDeploymentTarget), customAssembly.DeploymentTarget.ToString());
                }
                else
                {
                    sourcePath = context.Project.OutputFullPath;
                    deploymentTarget = context.Project.AssemblyDeploymentTarget;
                }
                InstallAssemblyItem(binPaths, sourcePath, deploymentTarget);
            }
        }

        /// <summary>
        /// Tries the install project assembly.
        /// </summary>
        /// <param name="binPaths">The bin paths.</param>
        /// <param name="project">The project.</param>
        void TryInstallProjectAssembly(string[] binPaths, ISharePointProject project)
        {
            if (project.IncludeAssemblyInPackage)
            {
                InstallAssemblyItem(binPaths,
                    project.OutputFullPath,
                    project.AssemblyDeploymentTarget);
            }
        }

        /// <summary>
        /// Installs the assembly item.
        /// </summary>
        /// <param name="binPaths">The bin paths.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="deploymentTarget">The deployment target.</param>
        void InstallAssemblyItem(string[] binPaths, string sourcePath, AssemblyDeploymentTarget deploymentTarget)
        {
            if (deploymentTarget == AssemblyDeploymentTarget.GlobalAssemblyCache)
            {
                AssemblyCache.InstallAssembly(sourcePath, null, AssemblyCache.AssemblyCommitFlags.Force);
            }
            else
            {
                foreach (string binPath in binPaths)
                {
                    string targetPath = Path.Combine(
                        binPath, "bin");
                    if (Directory.Exists(targetPath) == false)
                    {
                        Directory.CreateDirectory(targetPath);
                    }
                    targetPath = Path.Combine(targetPath,
                        Path.GetFileName(sourcePath));
                    File.Copy(sourcePath, targetPath, true);
                }
            }
        }
    }
}

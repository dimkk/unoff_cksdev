using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;
using Microsoft.VisualStudio.Shell.Interop;
using CKS.Dev.VisualStudio.SharePoint.Deployment.QuickDeployment;
using CKS.Dev.VisualStudio.SharePoint.Commands;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps
{
    /// <summary>
    /// Copy files deployment step.
    /// </summary>
    [Export(typeof(IDeploymentStep))]
    [DeploymentStep(CustomDeploymentStepIds.CopyFiles)]
    public class CopyFilesStep
        : IDeploymentStep
    {
        /// <summary>
        /// Initializes the deployment step.
        /// </summary>
        /// <param name="stepInfo">An object that contains information about the deployment step.</param>
        public void Initialize(IDeploymentStepInfo stepInfo)
        {
            stepInfo.Name = "Copy Files (CKSDev)";
            stepInfo.StatusBarMessage = "Copying files...";
            stepInfo.Description = "Copies the project files locally.";
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
            string sharePointPath = context.Project.ProjectService.SharePointInstallPath;
            ISharePointProjectPackage package = context.Project.Package;
            IVsBuildPropertyStorage propertyStorage = context.Project.ProjectService.Convert<ISharePointProject, IVsBuildPropertyStorage>(context.Project);
            string layoutPath = null;

            propertyStorage.GetPropertyValue("LayoutPath", "", (uint)_PersistStorageType.PST_PROJECT_FILE, out layoutPath);
            EnvDTE.Project dteProject = context.Project.ProjectService.Convert<ISharePointProject, EnvDTE.Project>(context.Project);
            string configuration = dteProject.ConfigurationManager.ActiveConfiguration.ConfigurationName;

            string sourceRootPath = Path.Combine(
                Path.GetDirectoryName(context.Project.FullPath),
                layoutPath,
                configuration,
                context.Project.Name);
            string sharePointInstallDir = context.Project.ProjectService.SharePointInstallPath;

            foreach (ISharePointProjectItem item in package.ProjectItems)
            {
                foreach (ISharePointProjectItemFile file in item.Files)
                {
                    TryDeployFile(context, sourceRootPath, file, null);
                }
            }
            foreach (ISharePointProjectFeature feature in package.Features)
            {
                //Call the extension message
                string featureFolderName = feature.UnTokenize(feature.Model.DeploymentPath);
                string featureRootPath = Path.Combine(sourceRootPath, featureFolderName);
                foreach (ISharePointProjectItem item in feature.ProjectItems)
                {
                    foreach (ISharePointProjectItemFile file in item.Files)
                    {
                        if (file.DeploymentType == DeploymentType.ElementFile || file.DeploymentType == DeploymentType.ElementManifest)
                        {
                            TryDeployFile(context, sourceRootPath, file, featureFolderName);
                        }
                        else
                        {
                            TryDeployFile(context, sourceRootPath, file, null);
                        }
                    }
                }
                string featureFileName = "Feature.xml";

                DeployFile(context, Path.Combine(sourceRootPath, featureFolderName, featureFileName),
                    Path.Combine(sharePointInstallDir, @"TEMPLATE\FEATURES", featureFolderName, featureFileName));
            }

            // project output references
            // featureresource

            //ISharePointProjectOutputReference
            //foreach (ISharePointProjectOutputReference reference in
            //    package.ProjectItems.SelectMany(item => item.ProjectOutputReferences).Concat(package.Features
            //    .SelectMany(feature => feature.ProjectItems)
            //    .SelectMany(item => item.ProjectOutputReferences)))
            //{
            //    context.Logger.WriteLine("Project Output Reference: " + reference.DeploymentRoot + reference.DeploymentPath, LogCategory.Message);
            //}
            //foreach (ISharePointProjectFeatureResourceFile resource in package.Features.SelectMany(feature => feature.ResourceFiles))
            //{
            //    context.Logger.WriteLine("Feature Resource: " + resource.FullPath, LogCategory.Message);
            //}
        }

        /// <summary>
        /// Tries the deploy file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sourceRootPath">The source root path.</param>
        /// <param name="file">The file.</param>
        /// <param name="featureFolderName">Name of the feature folder.</param>
        void TryDeployFile(IDeploymentContext context,
            string sourceRootPath,
            ISharePointProjectItemFile file,
            string featureFolderName)
        {
            if (file.DeploymentType != DeploymentType.PackageOnly &&
                file.DeploymentType != DeploymentType.NoDeployment)
            {
                foreach (string deploymentRootPath in GetRootPaths(context, file))
                {
                    string currentRootPath = deploymentRootPath;
                    if (featureFolderName != null)
                    {
                        sourceRootPath = Path.Combine(sourceRootPath, featureFolderName);
                        currentRootPath = Path.Combine(deploymentRootPath, featureFolderName);
                    }
                    string relativePath = Path.Combine(file.DeploymentPath ?? "", file.Name);
                    string sourcePath = Path.Combine(sourceRootPath, relativePath);
                    string targetPath = Path.Combine(currentRootPath, relativePath);
                    DeployFile(context, sourcePath, targetPath);
                }
            }
        }

        /// <summary>
        /// Deploys the file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="targetPath">The target path.</param>
        void DeployFile(IDeploymentContext context, string sourcePath, string targetPath)
        {
            string directoryName = Path.GetDirectoryName(targetPath);
            if (Directory.Exists(directoryName) == false)
            {
                Directory.CreateDirectory(directoryName);
            }
            File.Copy(sourcePath, targetPath, true);
            PathUtility.EnsureFileIsNotReadOnly(targetPath);
        }

        /// <summary>
        /// Gets the root paths.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        IEnumerable<string> GetRootPaths(IDeploymentContext context, ISharePointProjectItemFile file)
        {
            List<string> rootPaths = new List<string>();
            switch (file.DeploymentType)
            {
                case DeploymentType.RootFile:
                    rootPaths.Add(context.Project.ProjectService.SharePointInstallPath);
                    break;
                case DeploymentType.TemplateFile:
                    rootPaths.Add(Path.Combine(context.Project.ProjectService.SharePointInstallPath, "TEMPLATE"));
                    break;
                case DeploymentType.ElementManifest:
                case DeploymentType.ElementFile:
                case DeploymentType.Resource:
                    rootPaths.Add(Path.Combine(context.Project.ProjectService.SharePointInstallPath, @"TEMPLATE\FEATURES"));
                    break;
                case DeploymentType.ApplicationResource:
                    foreach (string binPath in GetBinPaths(context))
                    {
                        rootPaths.Add(Path.Combine(binPath, "resources"));
                    }
                    break;
                case DeploymentType.AppGlobalResource:
                    foreach (string binPath in GetBinPaths(context))
                    {
                        rootPaths.Add(Path.Combine(binPath, "App_GlobalResources"));
                    }
                    break;
                case DeploymentType.ClassResource:
                    if (file.Project.IncludeAssemblyInPackage)
                    {
                        if (file.Project.AssemblyDeploymentTarget == AssemblyDeploymentTarget.WebApplication)
                        {
                            foreach (string binPath in GetBinPaths(context))
                            {
                                rootPaths.Add(Path.Combine(binPath, "wpresources"));
                            }
                        }
                        else
                        {
                            string installPath = context.Project.ProjectService.SharePointInstallPath.TrimEnd('\\');
                            string parentFolder = Path.GetDirectoryName(installPath);
                            rootPaths.Add(Path.Combine(parentFolder, "wpresources"));
                        }
                    }
                    break;
            }
            return rootPaths;
        }

        /// <summary>
        /// Gets the bin paths.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        IEnumerable<string> GetBinPaths(IDeploymentContext context)
        {
            BinPathCollection binPaths = null;
            if ((binPaths = context.Annotations.GetValue<BinPathCollection>()) == null)
            {
                binPaths = new BinPathCollection()
                {
                    BinPaths = context.Project.SharePointConnection.ExecuteCommand<string[]>(DeploymentSharePointCommandIds.GetWebApplicationPhysicalPaths)
                };
                context.Annotations.Add<BinPathCollection>(binPaths);
            }
            return binPaths.BinPaths;
        }

        /// <summary>
        /// The bin path collection.
        /// </summary>
        class BinPathCollection
        {
            public IEnumerable<string> BinPaths { get; set; }
        }
    }
}

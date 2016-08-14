using System.IO;
using System;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;
using Microsoft.VisualStudio.SharePoint.Packages;
using System.ComponentModel.Composition;
using System.Linq;
using System.DirectoryServices;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Xml;
using EnvDTE;
using EnvDTE80;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.QuickDeployment
{
    /// <summary>
    /// Project item file that can be copied to the SharePoint root.
    /// </summary>
    public class SharePointProjectItemFileArtefact : QuickCopyableSharePointArtefact
    {
        private ISharePointProjectItemFile file = null;

        /// <summary>
        /// Create a new instance of the SharePointProjectItemFileArtefact object.
        /// </summary>
        /// <param name="file">The project file.</param>
        public SharePointProjectItemFileArtefact(ISharePointProjectItemFile file)
        {
            this.file = file;
        }

        /// <summary>
        /// Gets the child artefacts.
        /// </summary>
        public override IEnumerable<QuickCopyableSharePointArtefact> ChildArtefacts
        {
            get
            {
                return new List<QuickCopyableSharePointArtefact>();
            }
        }

        /// <summary>
        /// Determines if this artefact is packaged anywhere in the solution.
        /// </summary>
        /// <param name="service">The SharePoint service.</param>
        /// <returns>True if the artefact is packaged.</returns>
        public override bool IsPackaged(ISharePointProjectService service)
        {
            return file.ProjectItem.IsPartOfAnyProjectPackage(service);
        }

        /// <summary>
        /// Determines if this artefact is packaged as part of a specific project.
        /// </summary>
        /// <param name="project">The SharePoint project.</param>
        /// <returns>True if the artefact is packaged.</returns>
        public override bool IsPackaged(ISharePointProject project)
        {
            return file.ProjectItem.IsPartOfProjectPackage(project);
        }

        /// <summary>
        /// Gets all projects in the solution where this artefact is packaged.
        /// </summary>
        /// <param name="service">The SharePoint service.</param>
        /// <returns>An enumerable of the SharePoint projects.</returns>
        public override IEnumerable<ISharePointProject> GetPackagedProjects(ISharePointProjectService service)
        {
            return file.ProjectItem.GetProjectsWhereInPackage(service);
        }

        /// <summary>
        /// Gets the substitution tokens for this artefact.
        /// </summary>
        /// <returns>The tokens dictionary.</returns>
        public override Dictionary<string, string> Tokens
        {
            get
            {
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Quick copy this artefact in the context of the specific package, but wherever this artefact is contained in that package.
        /// </summary>
        /// <param name="packageProject"></param>
        /// <param name="requiresQuickPackage"></param>
        public override void QuickCopy(SharePointPackageArtefact packageProject, bool requiresQuickPackage)
        {
            // We are directly deploying just this file and have not been called by our parent.
            // A file will always be in one SPI, but that SPI may be included in multiple features, or be against the project directly.
            if (file.ProjectItem.IsDirectPartOfProjectPackage(packageProject.Project))
            {
                // Pass in the package itself as the parent.
                this.QuickCopy(packageProject, packageProject, requiresQuickPackage);
            }
            else
            {
                // A file will always be in one SPI, but that SPI may be included in multiple features in the project in context.
                foreach (ISharePointProjectFeature feature in file.ProjectItem.GetFeaturesWhereInPackage(packageProject.Project))
                {
                    QuickCopyableSharePointArtefact parent = new SharePointProjectFeatureArtefact(feature);
                    this.QuickCopy(packageProject, parent, requiresQuickPackage);
                }
            }
        }

        /// <summary>
        /// Quick copy this artefact in the context of the specific package, and the specific containing artefact only.
        /// </summary>
        /// <param name="packageProject">The project.</param>
        /// <param name="parentArtefact">The deployable SharePoint artefact.</param>
        /// <param name="requiresQuickPackage">Flag to indicate it requires a quick package.</param>
        public override void QuickCopy(SharePointPackageArtefact packageProject, QuickCopyableSharePointArtefact parentArtefact, bool requiresQuickPackage)
        {
            if (parentArtefact == null)
            {
                throw new NotSupportedException();
            }
            else if (parentArtefact is SharePointProjectFeatureArtefact || parentArtefact is SharePointPackageArtefact)
            {
                SharePointProjectFeatureArtefact feature = parentArtefact as SharePointProjectFeatureArtefact;
                this.QuickCopy(packageProject, feature, requiresQuickPackage);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Quick copy this artefact in the context of the specific package, and the specific containing artefact only.
        /// </summary>
        /// <param name="packageProject">The project.</param>
        /// <param name="parentFeature">The deployable SharePoint artefact.</param>
        /// <param name="requiresQuickPackage">Flag to indicate it requires a quick package.</param>
        public void QuickCopy(SharePointPackageArtefact packageProject, SharePointProjectFeatureArtefact parentFeature, bool requiresQuickPackage)
        {
            if (file.DeploymentType != DeploymentType.NoDeployment)
            {
                // Determine the folder name of the parent feature (if applicable).
                string featureFolderName = parentFeature == null ? "" : parentFeature.FeatureFolderName;

                // The default destination path is given to us by the tooling (though includes tokens).
                string destinationPathHiveRelative = String.Empty;
                if (String.IsNullOrEmpty(file.DeploymentPath))
                {
                    destinationPathHiveRelative = file.DeploymentRoot;
                }
                else
                {
                    destinationPathHiveRelative = Path.Combine(file.DeploymentRoot, file.DeploymentPath);
                }

                // The source path of the packageable file begins with the base package path of the project.
                string sourcePackagePathProjectRelative = packageProject.BasePackagePath;

                // The remainder of the package path depends on the type of file.
                if (file.DeploymentType == DeploymentType.ElementFile || file.DeploymentType == DeploymentType.ElementManifest)
                {
                    // Source path within pkg is {FeatureName} + the item's relative path.
                    sourcePackagePathProjectRelative = Path.Combine(sourcePackagePathProjectRelative, featureFolderName);
                    sourcePackagePathProjectRelative = Path.Combine(sourcePackagePathProjectRelative, Path.GetDirectoryName(file.RelativePath));
                }
                if (file.DeploymentType == DeploymentType.AppGlobalResource || file.DeploymentType == DeploymentType.ApplicationResource)
                {
                    sourcePackagePathProjectRelative = Path.Combine(sourcePackagePathProjectRelative, Path.GetDirectoryName(file.RelativePath));
                }
                else if (file.DeploymentType == DeploymentType.RootFile || file.DeploymentType == DeploymentType.TemplateFile)
                {
                    // For both template and root files, these are stored relative to the pkg folder with a
                    // path matching file.DeploymentPath.
                    sourcePackagePathProjectRelative = Path.Combine(sourcePackagePathProjectRelative, Path.GetDirectoryName(file.DeploymentPath));
                }
                else
                {
                    // Unhandled file type.  Just show a message for now.
                    // TODO: check all the file types we should spuport and test.
                    packageProject.Project.ProjectService.Logger.ActivateOutputWindow();
                    packageProject.Project.ProjectService.Logger.WriteLine(string.Format("Unhandled File Type - {0} at {1} - please notify the CKSDEV team", file.DeploymentType, file.FullPath), LogCategory.Status);
                }

                // Make some final substitutions on the paths as necessary.
                sourcePackagePathProjectRelative = sourcePackagePathProjectRelative.Replace("{FeatureName}", featureFolderName);
                destinationPathHiveRelative = destinationPathHiveRelative.Replace("{FeatureName}", featureFolderName);

                // First package (if appropriate), then quick copy the file.
                if (requiresQuickPackage)
                {
                    // The actual project file path is also given to us by the tooling.
                    string originalFileProjectRelative = Path.GetDirectoryName(file.FullPath);

                    Dictionary<string, string> allTokens = null;
                    if (QuickDeploymentUtilities.IsTokenReplacementFile(file.Project, file.Name))
                    {
                        // Tokens consist of those from this package, SPI, and (optionally) the feature. 
                        allTokens = new Dictionary<string, string>();
                        allTokens.AddRange(packageProject.Tokens);
                        allTokens.AddRange(new SharePointProjectItemArtefact(file.ProjectItem).Tokens);
                        if (parentFeature != null)
                        {
                            allTokens.AddRange(parentFeature.Tokens);
                        }
                    }

                    QuickDeploymentUtilities.CopyFileWithTokenReplacement(packageProject.Project, file.Name, originalFileProjectRelative, sourcePackagePathProjectRelative, allTokens);
                }

                QuickDeploymentUtilities.CopyFile(packageProject.Project, file.Name, sourcePackagePathProjectRelative, destinationPathHiveRelative);
            }
        }
    }
}
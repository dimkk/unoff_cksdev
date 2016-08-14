using Microsoft.VisualStudio.SharePoint;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnvDTE;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.QuickDeployment
{
    /// <summary>
    /// Extends the SharePoint projects with menu items.
    /// </summary>
    // Export attribute: Enables Visual Studio to discover and load this extension.
    [Export(typeof(ISharePointProjectExtension))]
    public class DeploymentProjectExtension : ISharePointProjectExtension
    {
        /// <summary>
        /// Implements ISharePointProjectService.Initialize, which determines the behavior of the new property.
        /// </summary>
        /// <param name="projectService"></param>
        public void Initialize(ISharePointProjectService projectService)
        {
            // Handle events for when a project property is changed.
            projectService.ProjectPropertiesRequested += new EventHandler<SharePointProjectPropertiesRequestedEventArgs>(projectService_ProjectPropertiesRequested);
        }

        void projectService_ProjectPropertiesRequested(object sender, SharePointProjectPropertiesRequestedEventArgs e)
        {
            if (!e.Project.IsSandboxedSolution)
            {
                // Add new properties to the SharePoint project.
                e.PropertySources.Add((object)new AutoCopyToSharePointRootProperty(e.Project));
                e.PropertySources.Add((object)new AutoCopyAssembliesProperty(e.Project));
                e.PropertySources.Add((object)new BuildOnCopyAssembliesProperty(e.Project));
            }
        }
    }
}




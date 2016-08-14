using Microsoft.VisualStudio.SharePoint;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnvDTE;

#if VS2012Build_SYMBOL
using CKS.Dev11.VisualStudio.SharePoint.Deployment.ProjectProperties;
#elif VS2013Build_SYMBOL
using CKS.Dev12.VisualStudio.SharePoint.Deployment.ProjectProperties;
#elif VS2014Build_SYMBOL
using CKS.Dev13.VisualStudio.SharePoint.Deployment.ProjectProperties;
#else
using CKS.Dev.VisualStudio.SharePoint.Deployment.ProjectProperties;
#endif

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Deployment
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Deployment
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Deployment
#else
namespace CKS.Dev.VisualStudio.SharePoint.Deployment
#endif
{
    /// <summary>
    /// Extends the SharePoint projects with menu items.
    /// </summary>
    // Export attribute: Enables Visual Studio to discover and load this extension.
    [Export(typeof(ISharePointProjectExtension))]
    public class DeploymentProjectExtension : ISharePointProjectExtension
    {
        #region Methods

        /// <summary>
        /// Implements ISharePointProjectService.Initialize, which determines the behavior of the new property.
        /// </summary>
        /// <param name="projectService"></param>
        public void Initialize(ISharePointProjectService projectService)
        {
            // Handle events for when a project property is changed.
            projectService.ProjectPropertiesRequested += new EventHandler<SharePointProjectPropertiesRequestedEventArgs>(projectService_ProjectPropertiesRequested);
        }

        /// <summary>
        /// Handles the ProjectPropertiesRequested event of the projectService control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SharePointProjectPropertiesRequestedEventArgs" /> instance containing the event data.</param>
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

        #endregion
    }
}

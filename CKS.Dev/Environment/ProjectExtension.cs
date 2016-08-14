using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using EnvDTE;
using Microsoft.VisualStudio.SharePoint;
using VSLangProj;
using System.IO;
using System.Windows.Forms;
using CKS.Dev.VisualStudio.SharePoint.Environment;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;


namespace CKS.Dev.VisualStudio.SharePoint.Environment
{
    /// <summary>
    /// Project extension.
    /// </summary>
    [Export(typeof(ISharePointProjectExtension))]
    public class ProjectExtension : ISharePointProjectExtension
    {
        /// <summary>
        /// Initializes the SharePoint project extension.
        /// </summary>
        /// <param name="projectService">An instance of SharePoint project service.</param>
        public void Initialize(ISharePointProjectService projectService)
        {
            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.CopyAssemblyName, true))
            {
                projectService.ProjectMenuItemsRequested += new EventHandler<SharePointProjectMenuItemsRequestedEventArgs>(projectService_ProjectMenuItemsRequested);
            }
        }

        /// <summary>
        /// Handles the ProjectMenuItemsRequested event of the projectService control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.SharePoint.SharePointProjectMenuItemsRequestedEventArgs"/> instance containing the event data.</param>
        void projectService_ProjectMenuItemsRequested(object sender, SharePointProjectMenuItemsRequestedEventArgs e)
        {
            ProjectManager projectManager = new ProjectManager(e.Project);

            //Some error in the VS code which leaves the first menu item as 'Menu Command'
            IMenuItem dummy = e.ActionMenuItems.Add("");
            dummy.IsEnabled = false;

            IMenuItem copyAssemblyNameItem = e.ActionMenuItems.Add(Resources.ProjectExtension_CopyAssemblyName);
            copyAssemblyNameItem.Click += new EventHandler<MenuItemEventArgs>(copyAssemblyNameItem_Click);
        }
        
        /// <summary>
        /// Handles the Click event of the copyAssemblyNameItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.SharePoint.MenuItemEventArgs"/> instance containing the event data.</param>
        void copyAssemblyNameItem_Click(object sender, MenuItemEventArgs e)
        {
            ProjectManager c = new ProjectManager(e.Owner as ISharePointProject);
            Microsoft.Build.Evaluation.Project g = new Microsoft.Build.Evaluation.Project();
            if (c.DteProject.DTE.Solution.SolutionBuild.BuildState != vsBuildState.vsBuildStateInProgress)
            {                
                c.DteProject.DTE.Solution.SolutionBuild.BuildProject(c.DteProject.DTE.Solution.SolutionBuild.ActiveConfiguration.Name, c.DteProject.UniqueName, true);
                Clipboard.SetText(c.GetAssemblyName());
            }
        }
    }
}

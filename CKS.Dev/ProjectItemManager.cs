using System;
using Microsoft.VisualStudio.SharePoint;
using EnvDTE;

namespace CKS.Dev.VisualStudio.SharePoint
{
    /// <summary>
    /// The project item manager.
    /// </summary>
    class ProjectItemManager
    {
        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public ISharePointProjectItem Item { get; private set; }

        /// <summary>
        /// Gets or sets the DTE item.
        /// </summary>
        /// <value>The DTE item.</value>
        public ProjectItem DteItem { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectItemManager"/> class.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        public ProjectItemManager(ISharePointProjectItem projectItem)
            : this(projectItem,
            DTEManager.ProjectService.Convert<ISharePointProjectItem, ProjectItem>(projectItem))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectItemManager"/> class.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <param name="dteProjectItem">The DTE project item.</param>
        ProjectItemManager(ISharePointProjectItem projectItem, ProjectItem dteProjectItem)
        {
            Item = projectItem;
            DteItem = dteProjectItem;
        }

        /// <summary>
        /// Creates the specified DTE project item.
        /// </summary>
        /// <param name="dteProjectItem">The DTE project item.</param>
        /// <returns></returns>
        public static ProjectItemManager Create(ProjectItem dteProjectItem)
        {
            EnvDTE.Project dteProject = dteProjectItem.ContainingProject;
            if (DTEManager.IsSharePointProject(dteProject) == false)
            {
                throw new ArgumentException();
            }
            ISharePointProjectItem projectItem = DTEManager.ProjectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItem>(dteProjectItem);
            return new ProjectItemManager(projectItem, dteProjectItem);
        }
    }
}

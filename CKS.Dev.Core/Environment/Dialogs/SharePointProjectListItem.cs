﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.SharePoint;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Environment.Dialogs
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Environment.Dialogs
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Environment.Dialogs
#else
namespace CKS.Dev.VisualStudio.SharePoint.Environment.Dialogs
#endif
{
    /// <summary>
    /// Helper class to store basic information about a Project List Item.
    /// </summary>
    public class SharePointProjectListItem
    {
        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        public ISharePointProject Project { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointProjectListItem" /> class.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <exception cref="System.ArgumentNullException">project</exception>
        public SharePointProjectListItem(ISharePointProject project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            Project = project;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Project.Name;
        }
    }
}

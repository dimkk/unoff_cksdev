using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    /// <summary>
    /// The starter master page wizard
    /// </summary>
    class StarterMasterPageWizard : BaseWizard
    {
        /// <summary>
        /// Set the project properties
        /// </summary>
        /// <param name="project">The project</param>
        public override void SetProjectProperties(EnvDTE.Project project)
        {
            ProjectManager projectManager = ProjectManager.Create(project);
        }

        /// <summary>
        /// Gets a flag indicating whether a SharePoint connection is required
        /// </summary>
        /// <value></value>
        protected override bool IsSharePointConnectionRequired
        {
            get { return true; }
        }

        /// <summary>
        /// Create the wizard form
        /// </summary>
        /// <param name="designTimeEnvironment">The design time environment</param>
        /// <param name="runKind">The wizard run kind</param>
        /// <returns>The IWizardFormExtension</returns>
        public override IWizardFormExtension CreateWizardForm(DTE designTimeEnvironment, WizardRunKind runKind)
        {
            return null;
        }
    }
}
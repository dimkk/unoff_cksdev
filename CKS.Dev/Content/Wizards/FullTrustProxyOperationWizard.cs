using System;
using EnvDTE;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using Microsoft.VisualStudio.TemplateWizard;
using System.Collections.Generic;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    /// <summary>
    /// The wizard for the FullTrustProxy SPI.
    /// </summary>
    class FullTrustProxyOperationWizard : BaseWizard
    {
        #region Properties

        /// <summary>
        /// Gets a flag indicating whether a SharePoint connection is required
        /// </summary>
        /// <value></value>
        protected override bool IsSharePointConnectionRequired
        {
            get { return true; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the project properties
        /// </summary>
        /// <param name="project">The project</param>
        public override void SetProjectProperties(EnvDTE.Project project)
        {
            ProjectManager projectManager = ProjectManager.Create(project);
            projectManager.Project.AssemblyDeploymentTarget = AssemblyDeploymentTarget.GlobalAssemblyCache;
            projectManager.Project.IsSandboxedSolution = false;
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

        /// <summary>
        /// Initialise from the the wizard data
        /// </summary>
        /// <param name="replacementsDictionary">The replacements dictionary</param>
        public override void InitializeFromWizardData(Dictionary<string, string> replacementsDictionary)
        {
            base.InitializeFromWizardData(replacementsDictionary);

            if (replacementsDictionary.ContainsKey("$rootname$"))
            {
                replacementsDictionary.Add("$subnamespace$", WizardHelpers.MakeNameCompliant(replacementsDictionary["$rootname$"]));
                replacementsDictionary.Add("$strongTypedArgs$", replacementsDictionary["$rootname$"].ToLower() + "Args");

                //Do this with a guid rather than $guid3$ as we have $ in the token that break it
                Guid frGuid = Guid.NewGuid();

                replacementsDictionary.Add("$frGuid$", frGuid.ToString("D"));

                replacementsDictionary.Add("$frGuidSPData$", "$SharePoint.Type." + frGuid.ToString("D") + ".FullName$");
            }
        }

        /// <summary>
        /// Run project item finished generating to add AllowPartiallyTrustedCallers attribute.
        /// </summary>
        /// <param name="projectItem">The project item</param>
        public override void RunProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            base.RunProjectItemFinishedGenerating(projectItem);

            ProjectManager.AddAllowPartiallyTrustedCallersAttribute(projectItem.ContainingProject);
        }

        /// <summary>
        /// Run the project finished generating to add AllowPartiallyTrustedCallers attribute.
        /// </summary>
        /// <param name="project">The project</param>
        public override void RunProjectFinishedGenerating(Project project)
        {
            base.RunProjectFinishedGenerating(project);

            ProjectManager.AddAllowPartiallyTrustedCallersAttribute(project);
        }

        #endregion
    }
}

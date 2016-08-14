using System;
using Microsoft.VisualStudio.TemplateWizard;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using EnvDTE;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    class CentralAdminPageWizard : BaseWizard
    {
        /// <summary>
        /// Set the project properties
        /// </summary>
        /// <param name="project">The project</param>
        public override void SetProjectProperties(EnvDTE.Project project)
        {
            
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
            }
        }

        public override void RunProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            base.RunProjectItemFinishedGenerating(projectItem);

            ProjectManager projectManager = ProjectManager.Create(projectItem.ContainingProject);
            string templatePath = ProjectUtilities.GetOfficeServerTemplatePath();
            templatePath = templatePath.ToLower().Replace("template\\", "");
            string dllRef = templatePath + @"CONFIG\ADMINBIN\Microsoft.SharePoint.ApplicationPages.Administration.dll";
            projectManager.AddReference(projectItem.ContainingProject, dllRef);
        }
    }
}

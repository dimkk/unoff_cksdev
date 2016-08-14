using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    /// <summary>
    /// The Ashx Handler wizard
    /// </summary>
    class AshxHandlerWizard : BaseWizard
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

        /// <summary>
        /// Intialise the form wizard data
        /// </summary>
        /// <param name="replacementsDictionary">The replacements dictionary</param>
        public override void InitializeFromWizardData(Dictionary<string, string> replacementsDictionary)
        {
            base.InitializeFromWizardData(replacementsDictionary);

            if (replacementsDictionary.ContainsKey("$rootname$"))
            {
                replacementsDictionary.Add("$subnamespace$", WizardHelpers.MakeNameCompliant(replacementsDictionary["$rootname$"]));

                //Do this with a guid rather than $guid3$ as we have $ in the token that break it
                Guid ashxGuid = Guid.NewGuid();

                replacementsDictionary.Add("$ashxGuid$", ashxGuid.ToString("D"));

                replacementsDictionary.Add("$ashxGuidClass$", "$SharePoint.Type." + ashxGuid.ToString("D") + ".FullName$");
            }

            ProjectManager projectManager = new ProjectManager(DTEManager.ActiveSharePointProject);
            projectManager.AddTokenReplacementFileExtension(DTEManager.ActiveProject, "ashx");
        }
    }
}

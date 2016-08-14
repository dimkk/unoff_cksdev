using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using Microsoft.VisualStudio.SharePoint;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Pages;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Exploration;
using CKS.Dev.VisualStudio.SharePoint.Content;
namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    class CustomActionWizard : BaseWizard
    {
        #region Fields

        /// <summary>
        /// Field to hold the custom action properties
        /// </summary>
        private CustomActionProperties _customActionProperties;

        /// <summary>
        /// Field to hold the deployment properties
        /// </summary>
        private CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties.DeploymentProperties _deploymentProperties;

        /// <summary>
        /// Field to hold the project name
        /// </summary>
        private string _projectName;

        /// <summary>
        /// Field to hold the root name
        /// </summary>
        private string _rootName;

        #endregion

        /// <summary>
        /// Gets the flag to indicate whether a SharePoint connection is required
        /// </summary>
        protected override bool IsSharePointConnectionRequired
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Set the project properties
        /// </summary>
        /// <param name="project">The project</param>
        public override void SetProjectProperties(EnvDTE.Project project)
        {
            ProjectManager projectManager = ProjectManager.Create(project);
            projectManager.Project.SiteUrl = _deploymentProperties.Url;
            projectManager.Project.IsSandboxedSolution = _deploymentProperties.IsSandboxedSolution;
            projectManager.Project.StartupItem = Enumerable.FirstOrDefault<ISharePointProjectItem>(projectManager.GetItemsOfType(ProjectItemIds.CustomAction));
        }

        /// <summary>
        /// Create the wizard forms
        /// </summary>
        /// <param name="designTimeEnvironment">The design time environment</param>
        /// <param name="runKind">The wizard run kind</param>
        /// <returns>The IWizardFormExtension</returns>
        public override IWizardFormExtension CreateWizardForm(DTE designTimeEnvironment, WizardRunKind runKind)
        {
            _deploymentProperties = new DeploymentProperties();

            ArtifactWizardForm wiz = new ArtifactWizardForm(designTimeEnvironment, Resources.CustomAction_WizardTitle);

            if (runKind == WizardRunKind.AsNewProject)
            {
                _customActionProperties = new CustomActionProperties(Guid.NewGuid(), _deploymentProperties);
                DeploymentPresentationModel model = new DeploymentPresentationModel(_deploymentProperties, false, IsSharePointConnectionRequired);
                CustomActionPresentationModel model2 = new CustomActionPresentationModel(_customActionProperties, false, designTimeEnvironment);
                DeploymentPage page = new DeploymentPage(wiz, model);
                CustomActionPage1 page2 = new CustomActionPage1(wiz, model2);
                CustomActionPage2 page3 = new CustomActionPage2(wiz, model2);
                CustomActionPage3 page4 = new CustomActionPage3(wiz, model2);
                wiz.AddPage(page);
                wiz.AddPage(page2);
                wiz.AddPage(page3);
                wiz.AddPage(page4);
                return wiz;
            }
            ProjectManager projectManager = ProjectManager.Create(
                DTEManager.ActiveProject);
            _deploymentProperties.IsSandboxedSolution = projectManager.Project.IsSandboxedSolution;
            _deploymentProperties.Url = projectManager.Project.SiteUrl;
            _projectName = projectManager.Project.Name;
            _customActionProperties = new CustomActionProperties(Guid.NewGuid(), _deploymentProperties);
            CustomActionPresentationModel model3 = new CustomActionPresentationModel(_customActionProperties, false, designTimeEnvironment);
            CustomActionPage1 page5 = new CustomActionPage1(wiz, model3);
            CustomActionPage2 page6 = new CustomActionPage2(wiz, model3);
            CustomActionPage3 page7 = new CustomActionPage3(wiz, model3);
            wiz.AddPage(page5);
            wiz.AddPage(page6);
            wiz.AddPage(page7);
            return wiz;

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
                _rootName = replacementsDictionary["$rootname$"];
            }
            if (replacementsDictionary.ContainsKey("$projectname$"))
            {
                _projectName = replacementsDictionary["$projectname$"];
            }
        }

        /// <summary>
        /// Populate the replacement dictionary
        /// </summary>
        /// <param name="replacementsDictionary">The replacements dictionary</param>
        public override void PopulateReplacementDictionary(Dictionary<string, string> replacementsDictionary)
        {
            base.PopulateReplacementDictionary(replacementsDictionary);
            replacementsDictionary["$CustomAction$"] = _customActionProperties.ToString();

        }

    }
}

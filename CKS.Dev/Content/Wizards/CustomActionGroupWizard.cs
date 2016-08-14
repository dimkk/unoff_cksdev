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
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Pages;
using CKS.Dev.VisualStudio.SharePoint.Exploration;
using CKS.Dev.VisualStudio.SharePoint.Content;
namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    /// <summary>
    /// The Custom Action Group wizard
    /// </summary>
    class CustomActionGroupWizard : BaseWizard
    {
        #region Properties

        /// <summary>
        /// Gets or sets the custom action group properties
        /// </summary>
        private CustomActionGroupProperties CurrentCustomActionGroupProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the deployment properties
        /// </summary>
        private DeploymentProperties CurrentDeploymentProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the project name
        /// </summary>
        private string CurrentProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the root name
        /// </summary>
        private string CurrentRootName
        {
            get;
            set;
        }

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
            projectManager.Project.SiteUrl = CurrentDeploymentProperties.Url;
            projectManager.Project.IsSandboxedSolution = CurrentDeploymentProperties.IsSandboxedSolution;
            projectManager.Project.StartupItem = Enumerable.FirstOrDefault<ISharePointProjectItem>(projectManager.GetItemsOfType(ProjectItemIds.CustomActionGroup));
        }

        /// <summary>
        /// Create the wizard forms
        /// </summary>
        /// <param name="designTimeEnvironment">The design time environment</param>
        /// <param name="runKind">The wizard run kind</param>
        /// <returns>The IWizardFormExtension</returns>
        public override IWizardFormExtension CreateWizardForm(DTE designTimeEnvironment, WizardRunKind runKind)
        {
            CurrentDeploymentProperties = new DeploymentProperties();

            ArtifactWizardForm wiz = new ArtifactWizardForm(designTimeEnvironment, Resources.CustomActionGroup_WizardTitle);

            if (runKind == WizardRunKind.AsNewProject)
            {
                CurrentCustomActionGroupProperties = new CustomActionGroupProperties(Guid.NewGuid(), CurrentDeploymentProperties);
                DeploymentPresentationModel model = new DeploymentPresentationModel(CurrentDeploymentProperties, false, IsSharePointConnectionRequired);
                CustomActionGroupPresentationModel model2 = new CustomActionGroupPresentationModel(CurrentCustomActionGroupProperties, false, designTimeEnvironment);
                DeploymentPage page = new DeploymentPage(wiz, model);
                CustomActionGroupPage page2 = new CustomActionGroupPage(wiz, model2);
                wiz.AddPage(page);
                wiz.AddPage(page2);
                return wiz;
            }
            ProjectManager projectManager = ProjectManager.Create(
                DTEManager.ActiveProject);
            CurrentDeploymentProperties.IsSandboxedSolution = projectManager.Project.IsSandboxedSolution;
            CurrentDeploymentProperties.Url = projectManager.Project.SiteUrl;
            CurrentProjectName = projectManager.Project.Name;
            CurrentCustomActionGroupProperties = new CustomActionGroupProperties(Guid.NewGuid(), CurrentDeploymentProperties);
            CustomActionGroupPresentationModel model3 = new CustomActionGroupPresentationModel(CurrentCustomActionGroupProperties, false, designTimeEnvironment);
            CustomActionGroupPage page3 = new CustomActionGroupPage(wiz, model3);
            
            wiz.AddPage(page3);
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
                CurrentRootName = replacementsDictionary["$rootname$"];
            }
            if (replacementsDictionary.ContainsKey("$projectname$"))
            {
                CurrentProjectName = replacementsDictionary["$projectname$"];
            }
        }

        /// <summary>
        /// Populate the replacement dictionary
        /// </summary>
        /// <param name="replacementsDictionary">The replacements dictionary</param>
        public override void PopulateReplacementDictionary(Dictionary<string, string> replacementsDictionary)
        {
            base.PopulateReplacementDictionary(replacementsDictionary);
            replacementsDictionary["$CustomActionGroup$"] = CurrentCustomActionGroupProperties.ToString();

        }

        #endregion
    }
}

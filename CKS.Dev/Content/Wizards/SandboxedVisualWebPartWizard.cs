using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using Microsoft.VisualStudio.TemplateWizard;
using System.IO;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Pages;
using CKS.Dev.VisualStudio.SharePoint.Environment.CustomTools;
using CKS.Dev.VisualStudio.SharePoint.Exploration;
using CKS.Dev.VisualStudio.SharePoint.Content;
namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    class SandboxedVisualWebPartWizard : BaseWizard
    {
        #region Properties

        /// <summary>
        /// Gets or sets the sandbox visual web part properties
        /// </summary>
        private SandBoxedVisualWebPartProperties CurrentSandBoxVisualWebPartProperties
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

        #endregion

        #region Methods

        /// <summary>
        /// Set the project properties
        /// </summary>
        /// <param name="project">The project</param>
        public override void SetProjectProperties(EnvDTE.Project project)
        {
            ProjectManager projectManager = ProjectManager.Create(project);
            projectManager.Project.SiteUrl = CurrentDeploymentProperties.Url;
            projectManager.Project.IsSandboxedSolution = CurrentDeploymentProperties.IsSandboxedSolution;
            projectManager.Project.StartupItem = Enumerable.FirstOrDefault<ISharePointProjectItem>(projectManager.GetItemsOfType(ProjectItemIds.SandBoxedVisualWebPart));
        }

        /// <summary>
        /// Create the wizard forms
        /// </summary>
        /// <param name="designTimeEnvironment">The design time environment</param>
        /// <param name="runKind">The wizard run kind</param>
        /// <returns>The IWizardFormExtension</returns>
        public override IWizardFormExtension CreateWizardForm(EnvDTE.DTE designTimeEnvironment, WizardRunKind runKind)
        {
            //CurrentDeploymentProperties = new DeploymentProperties();

            //ArtifactWizardForm wiz = new ArtifactWizardForm(designTimeEnvironment, Resources.SandBoxVisualWebPart_WizardTitle);

            //if (runKind == WizardRunKind.AsNewProject)
            //{
            //    CurrentSandBoxVisualWebPartProperties = new SandBoxedVisualWebPartProperties(Guid.NewGuid(), CurrentDeploymentProperties);
            //    DeploymentPresentationModel model = new DeploymentPresentationModel(CurrentDeploymentProperties, false, IsSharePointConnectionRequired);
            //    SandBoxedVisualWebPartPresentationModel model2 = new SandBoxedVisualWebPartPresentationModel(CurrentSandBoxVisualWebPartProperties, false, designTimeEnvironment);
            //    DeploymentPage page = new DeploymentPage(wiz, model);
            //    SandBoxedVisualWebPartPage page2 = new SandBoxedVisualWebPartPage(wiz, model2);
            //    wiz.AddPage(page);
            //    wiz.AddPage(page2);
            //    return wiz;
            //}
            //ExtendedSharePointServices sharePointServices = BaseWizard.GetSharePointServices(WizardHelpers.GetActiveProject(designTimeEnvironment));
            //CurrentDeploymentProperties.IsSandboxedSolution = sharePointServices.Project.IsSandboxedSolution;
            //CurrentDeploymentProperties.Url = sharePointServices.Project.SiteUrl;
            //CurrentProjectName = WizardHelpers.GetActiveProjectName(designTimeEnvironment);
            //CurrentSandBoxVisualWebPartProperties = new SandBoxedVisualWebPartProperties(Guid.NewGuid(), CurrentDeploymentProperties);
            //SandBoxedVisualWebPartPresentationModel model3 = new SandBoxedVisualWebPartPresentationModel(CurrentSandBoxVisualWebPartProperties, false, designTimeEnvironment);
            //SandBoxedVisualWebPartPage page3 = new SandBoxedVisualWebPartPage(wiz, model3);
            //wiz.AddPage(page3);
            //return wiz;
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
                CurrentRootName = replacementsDictionary["$rootname$"];
            }
            if (replacementsDictionary.ContainsKey("$projectname$"))
            {
                CurrentProjectName = replacementsDictionary["$projectname$"];
            }
        }

        public override void RunProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
            base.RunProjectItemFinishedGenerating(projectItem);

            if (projectItem != null &&
                String.Equals(Path.GetExtension(projectItem.Name), ".ascx", StringComparison.OrdinalIgnoreCase))
            {
                //TODO: re work
                EnvDTE.Property property = projectItem.Properties.Item("CustomTool");
                property.Value = "SandboxedVisualWebPartGenerator";
            }
        }

        /// <summary>
        /// Populate the replacement dictionary
        /// </summary>
        /// <param name="replacementsDictionary">The replacements dictionary</param>
        public override void PopulateReplacementDictionary(Dictionary<string, string> replacementsDictionary)
        {
            base.PopulateReplacementDictionary(replacementsDictionary);

            //TODO: re work
            //From here.....
            if (replacementsDictionary.ContainsKey("$rootname$"))
            {
                replacementsDictionary.Add("$subnamespace$", replacementsDictionary["$rootname$"]);
            }
            if (replacementsDictionary.ContainsKey("$safeitemrootname$"))
            {
                string rootName = replacementsDictionary["$safeitemrootname$"];
                if (String.IsNullOrEmpty(rootName) == false)
                {
                    replacementsDictionary.Add(
                        "$safeitemrootnamelowercase$",
                        rootName.ToLower());
                }
            }

        }

        #endregion
                
    }
}

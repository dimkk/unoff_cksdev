using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using Microsoft.VisualStudio.TemplateWizard;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Pages;
using System.Text;
using System.Globalization;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    /// <summary>
    /// Blank site definition wizard.
    /// </summary>
    class BlankSiteDefinitionWizard : BaseWizard
    {
        private const int _templateLocale = 0x409;

        //#region Properties

        /// <summary>
        /// Gets or sets the hide custom action properties
        /// </summary>
        private HideCustomActionProperties CurrentHideCustomActionProperties
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

        //#endregion

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
            //TODO: REplace string with constant from OM or add comment that there is none.
            projectManager.Project.StartupItem = projectManager.GetItemsOfType(
                "Microsoft.VisualStudio.SharePoint.SiteDefinition")
                .FirstOrDefault();
            CurrentDeploymentProperties.SaveProjectProperties(projectManager.Project);
        }
        
        /// <summary>
        /// Create the wizard forms
        /// </summary>
        /// <param name="designTimeEnvironment">The design time environment</param>
        /// <param name="runKind">The wizard run kind</param>
        /// <returns>The IWizardFormExtension</returns>
        public override IWizardFormExtension CreateWizardForm(DTE designTimeEnvironment, WizardRunKind runKind)
        {
            //this._dte = designTimeEnvironment;
            //this._deploymentProperties = new DeploymentProperties();
            //ArtifactWizardForm wiz = new ArtifactWizardForm(designTimeEnvironment, "WizardResources.WizardTitle");
            //DeploymentPresentationModel model = new DeploymentPresentationModel(this._deploymentProperties, true, this.IsSharePointConnectionRequired);
            //model.EnableUserSolutionInput = false;
            //DeploymentPage page = new DeploymentPage(wiz, model);
            //wiz.AddPage(page);
            //return wiz;

            return null;
        }

        ///// <summary>
        ///// Initialise from the the wizard data
        ///// </summary>
        ///// <param name="replacementsDictionary">The replacements dictionary</param>
        //public override void InitializeFromWizardData(Dictionary<string, string> replacementsDictionary)
        //{
        //    base.InitializeFromWizardData(replacementsDictionary);
        //    if (replacementsDictionary.ContainsKey("$rootname$"))
        //    {
        //        CurrentRootName = replacementsDictionary["$rootname$"];
        //    }
        //    if (replacementsDictionary.ContainsKey("$projectname$"))
        //    {
        //        CurrentProjectName = replacementsDictionary["$projectname$"];
        //    }
        //}

        /// <summary>
        /// Populate the replacement dictionary
        /// </summary>
        /// <param name="replacementsDictionary">The replacements dictionary</param>
        public override void PopulateReplacementDictionary(Dictionary<string, string> replacementsDictionary)
        {
            base.PopulateReplacementDictionary(replacementsDictionary);
            replacementsDictionary["$SafeSiteDefName$"] = WizardHelpers.MakeNameCompliant(replacementsDictionary["$rootname$"]);
            replacementsDictionary["$sitedefid$"] = "100000";
        }  

        #endregion
    }
}
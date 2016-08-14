namespace CKS.Dev.WCT
{
    using System;
    using System.Collections.Generic;
    using EnvDTE;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.TemplateWizard;
    using CKS.Dev.WCT.Resources;
    using CKS.Dev.WCT.ProjectWizard;
    using CKS.Dev.WCT.SolutionModel;
    using CKS.Dev.WCT.Mappers;
    using CKS.Dev.WCT.ModelCreators;

    public class WSPProjImportWizard : IWizard
    {
        private WizardWindow _wizardUI;

        private DTE _dteObject;

        private WSPProjWizardModel _presentationModel;

        private WCTContext ProjectContext { get; set; }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            _dteObject = automationObject as DTE;

            _presentationModel = new WSPProjWizardModel(_dteObject, false);

            string language;
            if (replacementsDictionary.TryGetValue("$language$", out language))
            {
                _presentationModel.IsCSharp = (language == "CSharp");
            }

            if (!_presentationModel.ProjectService.IsSharePointInstalled)
            {
                VsShellUtilities.ShowMessageBox(_presentationModel.GetProjectService().ServiceProvider,
                                StringResources.Strings_WizardResources_NoSharePointMessage,
                                StringResources.Strings_WizardResources_NoSharePointCaption,
                                OLEMSGICON.OLEMSGICON_CRITICAL,
                                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

                throw new WizardCancelledException(StringResources.Strings_WizardResources_NoSharePointMessage);
            }

            _wizardUI = new WizardWindow(_presentationModel);
            bool? dialogCompleted = _wizardUI.ShowModal();

            if (dialogCompleted != true)
            {
                throw new WizardCancelledException(StringResources.Strings_WizardResources_WizardCancelledMessage);
            }
        }

        public void ProjectFinishedGenerating(Project project)
        {
            WCTContext context = new WCTContext();
            // Fixing the VSItem name bug
            VSSharePointItem.Reset();

            // Exclude files 
            context.ExcludedFileExtensions.Add(".gpState"); // Source control
            context.ExcludedFileExtensions.Add(".vssscc"); // Source control
            context.ExcludedFileExtensions.Add(".vspscc"); // Source control
            context.ExcludedFileExtensions.Add(".pdb"); // Symbols
            // Exclude folders
            context.ExcludedFolders.Add(".svn"); // Subversion

            try
            {
                context.DteProject = project;

                VSProjectCreator projectCreator = new VSProjectCreator(context);
                context.SourceProject = projectCreator.Create(
                    _presentationModel.PackagePath,
                    _presentationModel.IsCSharp);

                context.SourceProject.SiteURL = this._presentationModel.CurrentSiteUrl;
                context.SourceProject.IsSandboxedSolution = _presentationModel.IsSandboxed;


                ProjectHandler updater = new ProjectHandler(context);
                updater.Update();
            }
            catch (Exception ex)
            {
                VsShellUtilities.ShowMessageBox(_presentationModel.GetProjectService().ServiceProvider,
                                                ex.ToString(),
                                                StringResources.Strings_Wizard_WizardTitle,
                                                OLEMSGICON.OLEMSGICON_CRITICAL,
                                                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                                                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }
    }
}
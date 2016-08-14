using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.PlatformUI;
using CKS.Dev.WCT.Resources;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;

namespace CKS.Dev.WCT.ProjectWizard
{
    public partial class WizardWindow : DialogWindow
    {
        internal WSPProjWizardModel PresentationModel { get; set; }

        internal WizardWindow(WSPProjWizardModel presentationModel)
        {
            InitializeComponent();

            this.PresentationModel = presentationModel;

            this.Title = StringResources.Strings_Wizard_WizardTitle;

            lblHeadline.Content = StringResources.Strings_Wizard_DeploymentHeadline;

            lblLocalSite.Content = StringResources.Strings_Wizard_DeploymentLocalSiteQuestion;
            btnValidate.Content = StringResources.Strings_Wizard_DeploymentValidateButton;

            lblTrustLevel.Content = StringResources.Strings_Wizard_DeploymentTrustLevelQuestion;
            rbtnSandboxed.Content = StringResources.Strings_Wizard_DeploymentRadioUserSolution;
            tblkSandboxed.Text = StringResources.Strings_Wizard_DeploymentLabelUserSolution;
            rbtnFullTrust.Content = StringResources.Strings_Wizard_DeploymentRadioFullTrust;
            tblkFullTrust.Text = StringResources.Strings_Wizard_DeploymentLabelFullTrust;

            lblVseWSS.Content = StringResources.Strings_Wizard_VSeWSSImportSourcePathLabel;
            tblkVSeWSS.Text = StringResources.Strings_Wizard_VSeWSSImportSourceCaptionLabel;
            btnBrowse.Content = StringResources.Strings_Wizard_SourceBrowseButton;

            btnFinish.Content = StringResources.Strings_Wizard_FinishButton;
            btnCancel.Content = StringResources.Strings_Wizard_CancelButton;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtLocalSite.Text = PresentationModel.GetLocalHostUrl();
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateUrl())
            {
                if (!String.IsNullOrEmpty(txtLocalSite.Text) && !String.IsNullOrEmpty(txtPackagePath.Text))
                {
                    PresentationModel.CurrentSiteUrl = txtLocalSite.Text;
                    PresentationModel.IsSandboxed = rbtnSandboxed.IsChecked.Value;
                    PresentationModel.PackagePath = txtPackagePath.Text;

                    DialogResult = true;
                    Close();
                }
                else
                {
                    VsShellUtilities.ShowMessageBox(PresentationModel.GetProjectService().ServiceProvider,
                                                    StringResources.Strings_WizardResources_NotValid,
                                                    StringResources.Strings_Wizard_WizardTitle,
                                                    OLEMSGICON.OLEMSGICON_CRITICAL,
                                                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                                                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                }
            }
        }

        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            btnValidate.IsEnabled = false;
            btnFinish.IsEnabled = false;

            string errorMessage;
            if (!PresentationModel.ValidateCurrentUrl(out errorMessage))
            {
                VsShellUtilities.ShowMessageBox(PresentationModel.GetProjectService().ServiceProvider,
                                String.Format(StringResources.Strings_WizardResources_ValidationErrorMessage, PresentationModel.CurrentSiteUrl, errorMessage),
                                StringResources.Strings_WizardResources_ValidationErrorCaption,
                                OLEMSGICON.OLEMSGICON_CRITICAL,
                                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
            else
            {
                VsShellUtilities.ShowMessageBox(PresentationModel.GetProjectService().ServiceProvider,
                                                String.Format(StringResources.Strings_WizardResources_ValidSiteUrlMessage, PresentationModel.CurrentSiteUrl),
                                                StringResources.Strings_WizardResources_ValidSiteUrlCaption,
                                                OLEMSGICON.OLEMSGICON_INFO,
                                                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                                                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }

            btnValidate.IsEnabled = true;
            btnFinish.IsEnabled = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (PresentationModel.IsCSharp)
            {
                dlg.DefaultExt = ".csproj";
                dlg.Filter = "CS Projects (.csproj)|*.csproj";
            }
            else
            {
                dlg.DefaultExt = ".vbproj";
                dlg.Filter = "VB Projects (.vbproj)|*.vbproj";
            }

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                txtPackagePath.Text = dlg.FileName;
            }
        }

        private bool ValidateUrl()
        {
            //string errorMessage;
            //if (!PresentationModel.ValidateCurrentUrl(out errorMessage))
            //{
            //    VsShellUtilities.ShowMessageBox(PresentationModel.GetProjectService().ServiceProvider,
            //                    String.Format(StringResources.Strings_WizardResources_ValidationErrorMessage, PresentationModel.CurrentSiteUrl, errorMessage),
            //                    StringResources.Strings_WizardResources_ValidationErrorCaption,
            //                    OLEMSGICON.OLEMSGICON_CRITICAL,
            //                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
            //                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            //    return false;
            //}

            return true;
        }

        private void txtLocalSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            string url = EnsureTrailingSlash(txtLocalSite.Text);

            if ((url.Length > 0) && (Uri.IsWellFormedUriString(Uri.EscapeUriString(url), UriKind.Absolute)))
            {
                btnFinish.IsEnabled = true;
                btnValidate.IsEnabled = true;
                PresentationModel.CurrentSiteUrl = url;
            }
            else
            {
                btnFinish.IsEnabled = true;
                btnValidate.IsEnabled = true;
            }
        }

        private string EnsureTrailingSlash(string url)
        {
            if (!String.IsNullOrEmpty(url)
                && url[url.Length - 1] != '/')
            {
                url += '/';
            }
            return url;
        }
    }

}

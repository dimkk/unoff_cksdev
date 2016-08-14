namespace CKS.Dev.WCT
{
    using System;
    using System.Collections.Generic;
    using EnvDTE;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.SharePoint;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using CKS.Dev.WCT.Commands;
    using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

    internal class WSPProjWizardModel
    {
        private DTE _dteObject;

        private ISharePointProjectService _projectServiceValue;

        private List<string> _validatedUrls = new List<string>();

        internal ISharePointProjectService ProjectService
        {
            get
            {
                if (_projectServiceValue == null)
                {
                    _projectServiceValue = GetProjectService();
                }
                return _projectServiceValue;
            }
        }

        internal bool IsSandboxed { get; set; }

        internal string PackagePath { get; set; }

        internal string CurrentSiteUrl 
        { get; set; }

        internal bool IsCSharp { get; set; }

        internal WSPProjWizardModel(DTE dteObject, bool isSandboxed)
        {
            this._dteObject = dteObject;

            IsSandboxed = !isSandboxed;
            CurrentSiteUrl = GetLocalHostUrl();
        }

        internal bool ValidateCurrentUrl(out string errorMessage)
        {
            bool isValid = false;
            errorMessage = String.Empty;

            if (_validatedUrls.Contains(CurrentSiteUrl))
            {
                isValid = true;
            }
            else
            {
                Uri uriToValidate = new Uri(CurrentSiteUrl, UriKind.Absolute);
                IVsThreadedWaitDialog2 vsThreadedWaitDialog = null;

                try
                {
                    vsThreadedWaitDialog = ShowProgressDialog("Connect to SharePoint", "Connecting to SharePoint site " + CurrentSiteUrl);
                    isValid = this.ProjectService.SharePointConnection.ExecuteCommand<Uri, bool>(SharePointCommandIds.ValidateSite, uriToValidate);
                }
                catch (Exception ex)
                {
                    errorMessage = "An error occurred while validating the site. " + ex.Message;
                }
                finally
                {
                    if (isValid)
                    {
                        _validatedUrls.Add(CurrentSiteUrl);
                    }

                    if (vsThreadedWaitDialog != null)
                    {
                        CloseProgressDialog(vsThreadedWaitDialog);
                    }
                }
            }

            return isValid;
        }

        public string GetLocalHostUrl()
        {
            const string HttpScheme = "http";
            UriBuilder builder = new UriBuilder(HttpScheme, Environment.MachineName.ToLowerInvariant());
            return builder.ToString();
        }

        internal ISharePointProjectService GetProjectService()
        {
            ServiceProvider serviceProvider = new ServiceProvider(_dteObject as IOleServiceProvider);
            return serviceProvider.GetService(typeof(ISharePointProjectService)) as ISharePointProjectService;
        }

        private IVsThreadedWaitDialog2 ShowProgressDialog(string caption, string message)
        {
            IOleServiceProvider oleServiceProvider = _dteObject as IOleServiceProvider;
            IVsThreadedWaitDialogFactory dialogFactory = new ServiceProvider(oleServiceProvider).GetService(
                typeof(SVsThreadedWaitDialogFactory)) as IVsThreadedWaitDialogFactory;

            if (dialogFactory == null)
            {
                throw new InvalidOperationException("The IVsThreadedWaitDialogFactory object could not be retrieved.");
            }

            IVsThreadedWaitDialog2 vsThreadedWaitDialog = null;
            ErrorHandler.ThrowOnFailure(dialogFactory.CreateInstance(out vsThreadedWaitDialog));
            ErrorHandler.ThrowOnFailure(vsThreadedWaitDialog.StartWaitDialog(caption, message,
                                                            null, null, String.Empty, 0, false, true));
            return vsThreadedWaitDialog;
        }

        private void CloseProgressDialog(IVsThreadedWaitDialog2 vsThreadedWaitDialog)
        {
            if (vsThreadedWaitDialog == null)
            {
                throw new ArgumentNullException("vsThreadedWaitDialog");
            }

            int canceled;
            ErrorHandler.ThrowOnFailure(vsThreadedWaitDialog.EndWaitDialog(out canceled));
        }
    }
}
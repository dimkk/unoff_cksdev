using System;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using System.Windows.Forms;
using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;
using Microsoft.VisualStudio.Shell.Flavor;
using System.Reflection;
using Microsoft.VisualStudio.ExtensionManager;
using System.IO;
using System.Xml.Linq;
using CKS.Dev.VisualStudio.SharePoint.Environment;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;

namespace CKS.Dev.VisualStudio.SharePoint.Environment
{
    /// <summary>
    /// The VSPackage.
    /// </summary>
    [DefaultRegistryRoot(@"Software\Microsoft\VisualStudio\10.0exp")]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    //TODO: change this version number to effect the VS help about dialog
    [InstalledProductRegistration("#110", "#112", "2.5.0.0", IconResourceID = 400)]
    // Guid for the package.
    [Guid(GuidList.guidCKSDEV_Extensions_PackagePkgString)]
    // This attribute is needed to ensure our package is loaded as soon as a SP project exists (and thus the Quick Deploy menu handlers can be registered).
    [ProvideAutoLoad(VSPackageGuids.UIContext_SharePointProject)]
    [ProvideComponentPickerPage(GuidList.guidCKSDEV_ComponentPickerPageSharePoint, GuidList.guidCKSDEV_Extensions_PackagePkgString, "SharePoint")]
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]


    //The Enabled Extensions page
    [ProvideOptionPageAttribute(typeof(EnabledExtensionsOptionsPage), "CKS Development Tools Edition", "Extensions", 101, 102, true)]
    [ProvideProfileAttribute(typeof(EnabledExtensionsOptionsPage), "CKS Development Tools Edition", "Extensions", 101, 102, true, DescriptionResourceID = 101)]
    

    public sealed class VSPackage : Microsoft.VisualStudio.Shell.Package, IVsShellPropertyEvents, IVsComponentSelectorProvider
    {
        #region Constants

        const string ExtensionIdentifier = "CKS.Dev";
        
        #endregion

        #region Initialize

        bool hasInitialised = false;

        /// <summary>
        /// Initialise the VSPackage.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Added by MattSmithNZ
            // Cache the SP project service for reuse as necessary.
            // We will always have a SP project, otherwise CKSDEV will not work at all and should not have been installed.
            this.SharePointProjectService = this.GetService(typeof(ISharePointProjectService)) as ISharePointProjectService;

            // Added by MattSmithNZ
            // Call out to our event manager to register and handle project events.
            this.EventManager = new EventHandlerManager(this);
            this.EventManager.RegisterHandlers();

            // Added by MattSmithNZ
            // Workaround DTE not being available until startup is complete (under certain conditions for loading devenv.exe)
            // by receiving notification when the shell is loaded.
            IVsShell shellService = GetService(typeof(SVsShell)) as IVsShell;
            if (shellService != null)
            {
                shellService.AdviseShellPropertyChanges(this, out dteCookie);
            }

            // Call to ensure the DTE stuff is awake, again needed under certain conditions where DTE is not available yet.
            InitialiseDTEDependantObjects();
        }

        #endregion

        #region Properties

        SharePointReferenceView control;
        SharePointReferenceView ReferenceViewControl
        {
            get
            {
                if (control == null)
                {
                    control = CreateSharePointReferenceView();
                }
                return control;
            }
        }
        
        internal object GetServiceInternal(Type type)
        {
            return this.GetService(type);
        }

        internal ISharePointProjectService SharePointProjectService
        {
            get;
            set;
        }

        internal OutputWindow OutputWindow
        {
            get
            {
                EnvDTE80.DTE2 dte2 = GetService(typeof(SDTE)) as EnvDTE80.DTE2;
                return dte2.ToolWindows.OutputWindow;
            }
        }

        internal EventHandlerManager EventManager
        {
            get;
            set;
        }

        #endregion

        #region IVsShellPropertyEvents Members

        // Cached copies of event handler objects to avoid COM destroying our handlers.
        private uint dteCookie;
        private uint projTrackCookie;

        /// <summary>
        /// Handler to react to the OnShellPropertyChanged event.
        /// </summary>
        /// <param name="propid">The property id.</param>
        /// <param name="var">The value.</param>
        /// <returns>The result as int.</returns>
        public int OnShellPropertyChange(int propid, object var)
        {
            // When zombie state changes to false, finish package initialization.
            if ((int)__VSSPROPID.VSSPROPID_Zombie == propid)
            {
                if ((bool)var == false)
                {
                    InitialiseDTEDependantObjects();

                    // Zombie state clean-up code.
                    IVsShell shellService = GetService(typeof(SVsShell)) as IVsShell;
                    if (shellService != null)
                    {
                        shellService.UnadviseShellPropertyChanges(this.dteCookie);
                        shellService.UnadviseShellPropertyChanges(this.projTrackCookie);
                    }
                    this.dteCookie = 0;
                    this.projTrackCookie = 0;
                }

            }

            return VSConstants.S_OK;
        }

        private void InitialiseDTEDependantObjects()
        {
            if (!hasInitialised)
            {
                EnvDTE80.DTE2 dte2 = GetService(typeof(SDTE)) as EnvDTE80.DTE2;

                if (dte2 != null)
                {
                    // Zombie state dependent code.
                    dte2 = GetService(typeof(SDTE)) as EnvDTE80.DTE2;
                    EnvDTE80.Events2 events = (EnvDTE80.Events2)dte2.Events;
                    EnvDTE.DTE dte = GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;

                    // Register event handlers that need the DTE active to do work.
                    EventManager.RegisterDteDependentHandlers(dte2, events);

                    // Initialize the output logger.  This is a bit hacky but a quick fix for Alpha release.
                    StatusBarLogger.InitializeInstance(dte);
                    hasInitialised = true;
                }
            }
        }

        #endregion

        int IVsComponentSelectorProvider.GetComponentSelectorPage(
            ref Guid rguidPage, VSPROPSHEETPAGE[] ppage)
        {
            if (rguidPage !=  new Guid(GuidList.guidCKSDEV_ComponentPickerPageSharePoint))
            {
                throw new ArgumentException("rguidPage");
            }
            ppage[0].dwSize = (uint)Marshal.SizeOf(typeof(VSPROPSHEETPAGE));
            ppage[0].hwndDlg = ReferenceViewControl.Handle;

            ppage[0].dwFlags = 0;
            ppage[0].HINSTANCE = 0;
            ppage[0].dwTemplateSize = 0;
            ppage[0].pTemplate = IntPtr.Zero;
            ppage[0].pfnDlgProc = IntPtr.Zero;
            ppage[0].lParam = IntPtr.Zero;
            ppage[0].pfnCallback = IntPtr.Zero;
            ppage[0].pcRefParent = IntPtr.Zero;
            ppage[0].dwReserved = 0;
            ppage[0].wTemplateId = (ushort)0;
            return VSConstants.S_OK;
        }

        SharePointReferenceView CreateSharePointReferenceView()
        {
            SharePointReferenceView view = new SharePointReferenceView();
            IVsExtensionManager extensionManager = (IVsExtensionManager)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SVsExtensionManager));
            IInstalledExtension extension = extensionManager.GetInstalledExtension(ExtensionIdentifier);
            string referenceXmlPath = Path.Combine(extension.InstallPath, "SharePointReferences.xml");
            view.ReferencePath = referenceXmlPath;
            return view;
        }
    }
}

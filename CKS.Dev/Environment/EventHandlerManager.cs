using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Xml;

using EnvDTE;
using EnvDTE80;
using EnvDTE90;
using EnvDTE100;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

using CKS.Dev.VisualStudio.SharePoint.Exploration;

using CKS.Dev.VisualStudio.SharePoint.Deployment.QuickDeployment;
using CKS.Dev.VisualStudio.SharePoint.Deployment;
using CKS.Dev.VisualStudio.SharePoint.Properties;

namespace CKS.Dev.VisualStudio.SharePoint.Environment
{
    /// <summary>
    /// Manager class for registering and responding to various Visual Studio buttons and events.
    /// </summary>
    internal class EventHandlerManager
    {

        public const string vsProjectItemKindPhysicalFolder = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";

        #region Private Properties

        // Reference back to the package that created us.
        private VSPackage CKSDEVPackage
        {
            get;
            set;
        }

        EnvDTE.DTE DTE
        {
            get
            {
                return DTEManager.DTE;
            }
        }

        // Cache MenuCommandService locally for re-use.
        private OleMenuCommandService mcs = null;
        private OleMenuCommandService MenuCommandService
        {
            get
            {
                if (this.mcs == null)
                {
                    this.mcs = this.CKSDEVPackage.GetServiceInternal(typeof(IMenuCommandService)) as OleMenuCommandService;
                }
                return this.mcs;
            }
        }

        private bool IsSharePointInstalled
        {
            get;
            set;
        }

        #endregion

        #region Construction and Handler Registration

        /// <summary>
        /// Creates a new EventHandlerManager with a reference back to the SharePoint CKSDEVPackage.
        /// </summary>
        /// <param name="package"></param>
        public EventHandlerManager(VSPackage package)
        {
            this.CKSDEVPackage = package;
            this.IsSharePointInstalled = CKSDEVPackage.SharePointProjectService.IsSharePointInstalled;
        }

        internal void RegisterHandlers()
        {
            // Package All SharePoint Packages.
            AddHandler(PkgCmdIDList.cmdidPackageAllSharePointProjects, new EventHandler(this.PackageAllSharePointProjects_Click), new EventHandler(PackageAllSharePointProjects_BeforeQueryStatus));

            // Query status handlers for overall Quick Deploy menu.
            AddHandler(PkgCmdIDList.cmdidMnuQuickDeployCtx, null, new EventHandler(QuickDeployContext_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidMnuQuickDeploySel, null, new EventHandler(QuickDeploySelection_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidMnuQuickDeploySln, null, new EventHandler(QuickDeploySolution_BeforeQueryStatus));

            // Copy to 14 project, selection (multiple projects) and solution (all projects).
            AddHandler(PkgCmdIDList.cmdidCopySharePointRootCtx, new EventHandler(this.CopySharePointRootContext_Click), new EventHandler(StandardContextDisableForSandboxed_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidCopySharePointRootSel, new EventHandler(this.CopySharePointRootSelection_Click), new EventHandler(StandardContextDisableForSandboxed_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidCopySharePointRootSln, new EventHandler(this.CopySharePointRootSolution_Click), new EventHandler(StandardContextDisableForSandboxed_BeforeQueryStatus));

            // Copy binary, selection (multiple projects) and solution (all projects).
            AddHandler(PkgCmdIDList.cmdidCopyBinaryCtx, new EventHandler(this.CopyBinaryContext_Click), new EventHandler(StandardContextDisableForSandboxed_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidCopyBinarySel, new EventHandler(this.CopyBinarySelection_Click), new EventHandler(StandardSelectionDisableForSandboxed_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidCopyBinarySln, new EventHandler(this.CopyBinarySolution_Click), new EventHandler(StandardSolutionDisableForSandboxed_BeforeQueryStatus));

            // Copy both on project, selection (multiple projects) and solution (all projects).
            AddHandler(PkgCmdIDList.cmdidCopyBothCtx, new EventHandler(this.CopyBothContext_Click), new EventHandler(StandardContextDisableForSandboxed_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidCopyBothSel, new EventHandler(this.CopyBothSelection_Click), new EventHandler(StandardSelectionDisableForSandboxed_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidCopyBothSln, new EventHandler(this.CopyBothSolution_Click), new EventHandler(StandardSolutionDisableForSandboxed_BeforeQueryStatus));

            // Recycle app pool on project, selection (multiple projects) and solution (all projects).
            AddHandler(PkgCmdIDList.cmdidRecycleAppPoolCtx, new EventHandler(this.RecycleAppPoolContext_Click), new EventHandler(StandardContextFarmOrSandboxed_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidRecycleAppPoolSel, new EventHandler(this.RecycleAppPoolSelection_Click), new EventHandler(StandardSelectionFarmOrSandboxed_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidRecycleAppPoolSln, new EventHandler(this.RecycleAppPoolSolution_Click), new EventHandler(StandardSolutionFarmOrSandboxed_BeforeQueryStatus));

            // Recycle All Application Pools.
            AddHandler(PkgCmdIDList.cmdidRecycleAllAppPools, new EventHandler(this.RecycleAllAppPools_Click), new EventHandler(StandardRequireSharePointInstalled_BeforeQueryStatus));

            // Restart IIS.
            AddHandler(PkgCmdIDList.cmdidRestartIIS, new EventHandler(this.RestartIIS_Click), new EventHandler(StandardRequireSharePointInstalled_BeforeQueryStatus));

            // Restart User Code Process.
            AddHandler(PkgCmdIDList.cmdidRestartUserCodeProcess, new EventHandler(this.RestartUserCodeProcess_Click), new EventHandler(RestartUserCodeProcess_BeforeQueryStatus));

            // Restart OWS Timer Process.
            AddHandler(PkgCmdIDList.cmdidRestartOWSTimerProcess, new EventHandler(this.RestartOWSTimerProcess_Click), new EventHandler(RestartOWSTimerProcess_BeforeQueryStatus));


            // Attach to processes.
            AddHandler(PkgCmdIDList.cmdidAttachToAllSharePointProcess, new EventHandler(this.AttachToAllSharePointProcesses_Click), new EventHandler(AttachToAllSharePointProcesses_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidAttachToIISProcesses, new EventHandler(this.AttachToIISProcesses_Click), new EventHandler(AttachToIISProcesses_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidAttachToOWSTimerProcess, new EventHandler(this.AttachToOWSTimerProcess_Click), new EventHandler(AttachToOWSTimerProcess_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidAttachToUserCodeProcess, new EventHandler(this.AttachToUserCodeProcess_Click), new EventHandler(AttachToUserCodeProcess_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidAttachToVSSPHost4Process, new EventHandler(this.AttachToVSSPHost4Process_Click), new EventHandler(AttachToVSSPHost4Process_BeforeQueryStatus));

            // Copy to SharePoint Root at file/folder level.
            AddHandler(PkgCmdIDList.cmdidCopySharePointRootFld, new EventHandler(this.CopySharePointRootFolder_Click), new EventHandler(CopySharePointRootFolder_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidCopySharePointRootFle, new EventHandler(this.CopySharePointRootFile_Click), new EventHandler(CopySharePointRootFile_BeforeQueryStatus));

            // Open the deployment location folder
            AddHandler(PkgCmdIDList.cmdidOpenDeplFld, new EventHandler(this.OpenDeploymentLocationFolder_Click), new EventHandler(OpenDeploymentLocationFolder_BeforeQueryStatus));

            //Help Menus
            AddHandler(PkgCmdIDList.cmdidCodePlexHome, new EventHandler(this.CodePlexHome_Click), new EventHandler(CodePlexHome_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidCodePlexDocumentation, new EventHandler(this.CodePlexDocumentation_Click), new EventHandler(CodePlexDocumentation_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidCodePlexNewFeature, new EventHandler(this.CodePlexNewFeature_Click), new EventHandler(CodePlexNewFeature_BeforeQueryStatus));
            AddHandler(PkgCmdIDList.cmdidCodePlexNewIssue, new EventHandler(this.CodePlexNewIssue_Click), new EventHandler(CodePlexNewIssue_BeforeQueryStatus));

        }

        // Cache DTE objects locally to workaround COM disposing them before they can be called back.        
        private DTE2 m_ApplicationObject2;
        private EnvDTE.DocumentEvents m_DocumentEvents;
        private EnvDTE.BuildEvents m_BuildEvents;
        private _dispDocumentEvents_DocumentSavedEventHandler m_DocumentSavedEventHandler;
        private _dispBuildEvents_OnBuildBeginEventHandler m_BuildBeginEventHandler;
        private _dispBuildEvents_OnBuildDoneEventHandler m_BuildDoneEventHandler;
        private _dispBuildEvents_OnBuildProjConfigDoneEventHandler m_BuildProjConfigDoneHandler;

        internal void RegisterDteDependentHandlers(DTE2 dte2, Events2 events)
        {
            m_ApplicationObject2 = dte2;
            ////EnvDTE.DTE = GetService(typeof(SDTE)) as DTE;
            //m_SolutionEvents = dte2.Events.SolutionEvents;
            //m_ProjAddedEventHandler = new _dispSolutionEvents_ProjectAddedEventHandler(this.SolutionEvents_ProjectAdded);
            //dte2.Events.SolutionEvents.ProjectAdded += m_ProjAddedEventHandler;

            //ProjectItemsEvents projItemEvents = (ProjectItemsEvents)events.ProjectItemsEvents;
            //m_ProjectItemsEvents = projItemEvents;
            //m_ItemRenamedEventHandler = new _dispProjectItemsEvents_ItemRenamedEventHandler(this.ProjectItemsEvents_ItemRenamed);
            //projItemEvents.ItemRenamed += m_ItemRenamedEventHandler;

            m_DocumentEvents = (DocumentEvents)events.DocumentEvents;
            m_DocumentSavedEventHandler = new _dispDocumentEvents_DocumentSavedEventHandler(DocEvents_DocumentSaved);
            m_DocumentEvents.DocumentSaved += m_DocumentSavedEventHandler;

            m_BuildEvents = (BuildEvents)events.BuildEvents;
            m_BuildBeginEventHandler = new _dispBuildEvents_OnBuildBeginEventHandler(BuildEvents_OnBuildBegin);
            m_BuildEvents.OnBuildBegin += m_BuildBeginEventHandler;
            m_BuildDoneEventHandler = new _dispBuildEvents_OnBuildDoneEventHandler(BuildEvents_OnBuildDone);
            m_BuildEvents.OnBuildDone += m_BuildDoneEventHandler;
            m_BuildProjConfigDoneHandler = new _dispBuildEvents_OnBuildProjConfigDoneEventHandler(BuildEvents_OnBuildProjConfigDone);
            m_BuildEvents.OnBuildProjConfigDone += m_BuildProjConfigDoneHandler;
        }

        private OleMenuCommand AddHandler(uint commandID,
            EventHandler commandHandler,
            EventHandler queryHandler)
        {
            CommandID cmdID = new CommandID(GuidList.guidCKSDEVCmdSet, (int)commandID);
            OleMenuCommand cmdOmc = new OleMenuCommand(commandHandler, cmdID);
            if (queryHandler != null)
            {
                cmdOmc.BeforeQueryStatus += queryHandler;
            }

            this.MenuCommandService.AddCommand(cmdOmc);
            return cmdOmc;
        }

        private OleMenuCommand FindCommand(uint commandID)
        {
            CommandID cmdID = new CommandID(GuidList.guidCKSDEVCmdSet, (int)commandID);
            return this.MenuCommandService.FindCommand(cmdID) as OleMenuCommand;
        }

        #endregion

        #region Package All SharePoint Projects

        /// <summary>
        /// Handles the BeforeQueryStatus event of the PackageAllSharePointProjects control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PackageAllSharePointProjects_BeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                EnvDTE.DTE dte = this.DTE;

                cmd.Visible = cmd.Supported; // && DoLoadedSharePointProjectsExist(dte);
                cmd.Enabled = IsSharePointInstalled && IsNotBuilding(dte); //&& IsLoadedSharePointProjectSelected(dte);

                if (cmd.Enabled)
                {
                    // Only show the command if SharePoint projects exist.
                    try
                    {
                        if (DTEManager.SharePointProjects.Count > 0)
                        {
                            cmd.Enabled = true;
                        }
                        else
                        {
                            cmd.Enabled = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEManager.ProjectService.Logger.ActivateOutputWindow();
                        DTEManager.ProjectService.Logger.WriteLine(ex.ToString(), LogCategory.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the PackageAllSharePointProjects menu.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PackageAllSharePointProjects_Click(object sender, EventArgs arguments)
        {
            try
            {
                DTEManager.ProjectService.Logger.ActivateOutputWindow();
                foreach (ISharePointProject spProject in DTEManager.SharePointProjects)
                {
                    DTEManager.ProjectService.Logger.WriteLine("Packaging: " + spProject.Name, LogCategory.Status);
                    spProject.Package.BuildPackage();
                }
            }
            catch (Exception ex)
            {
                DTEManager.ProjectService.Logger.ActivateOutputWindow();
                DTEManager.ProjectService.Logger.WriteLine(ex.ToString(), LogCategory.Error);
            }
        }

        #endregion

        #region Standard Command Handlers

        private void StandardRequireSharePointInstalled_BeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                cmd.Visible = cmd.Supported;
                cmd.Enabled = cmd.Visible && IsSharePointInstalled;
            }
        }

        private void StandardContextDisableForSandboxed_BeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                EnvDTE.DTE dte = this.DTE;
                cmd.Visible = cmd.Supported && IsSharePointInstalled && IsLoadedSharePointProjectSelected(dte, false);
                cmd.Enabled = cmd.Visible && IsNotBuilding(dte) && IsLoadedSharePointProjectSelected(dte, true);
            }
        }

        private void StandardSolutionDisableForSandboxed_BeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                EnvDTE.DTE dte = this.DTE;
                cmd.Visible = cmd.Supported && IsSharePointInstalled && DoLoadedSharePointProjectsExist(dte, false);
                cmd.Enabled = cmd.Visible && IsNotBuilding(dte) && DoLoadedSharePointProjectsExist(dte, true);
            }
        }

        private void StandardSelectionDisableForSandboxed_BeforeQueryStatus(object sender, EventArgs e)
        {
            StandardContextDisableForSandboxed_BeforeQueryStatus(sender, e);
        }

        private void StandardContextFarmOrSandboxed_BeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                EnvDTE.DTE dte = this.DTE;
                cmd.Visible = cmd.Supported && IsSharePointInstalled && IsLoadedSharePointProjectSelected(dte, false);
                cmd.Enabled = cmd.Visible && IsNotBuilding(dte);
            }
        }

        private void StandardSolutionFarmOrSandboxed_BeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                EnvDTE.DTE dte = this.DTE;
                cmd.Visible = cmd.Supported && IsSharePointInstalled && DoLoadedSharePointProjectsExist(dte, false);
                cmd.Enabled = cmd.Visible && IsNotBuilding(dte);
            }
        }

        private void StandardSelectionFarmOrSandboxed_BeforeQueryStatus(object sender, EventArgs e)
        {
            StandardContextFarmOrSandboxed_BeforeQueryStatus(sender, e);
        }

        private void UpdateSelectionText(OleMenuCommand cmd, string prefix, string suffix)
        {
            EnvDTE.DTE dte = this.DTE;

            try
            {


                // As per internal VS behaviour, add project name if only one project is selected and it is loaded.
                Array asp = ((Array)dte.ActiveSolutionProjects);
                string name = "Selection";
                if (asp.Length == 1)
                {
                    EnvDTE.Project activeProject = (EnvDTE.Project)asp.GetValue(0);
                    if ((activeProject.ConfigurationManager != null) && (this.IsSharePointProject(activeProject, false)))
                    {
                        // Only loaded projects have a configuration manager.
                        // We also only apply this text if it is a SharePoint project for consistency with VS itself.
                        name = activeProject.Name;
                    }
                }
                cmd.Text = prefix + " " + name + " " + suffix;
            }
            catch (COMException)
            {
            }
        }

        #endregion

        #region Quick Deploy Menu Handlers

        private void QuickDeployContext_BeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                EnvDTE.DTE dte = this.DTE;

                // Quick Deploy menu is only shown if a SP project is selected, but we do not require it to be a farm solution
                // as some of the sub-commands can operate on sandboxed solutions.  We also show it if building, but require
                // that the sub-commands disable themselves.
                cmd.Visible = cmd.Supported && IsSharePointInstalled && IsLoadedSharePointProjectSelected(dte, false);
                cmd.Enabled = cmd.Visible;
            }
        }

        private void QuickDeploySolution_BeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                // Quick Deploy menu is only shown if a SP project is selected, but we do not require it to be a farm solution
                // as some of the sub-commands can operate on sandboxed solutions.  We also show it if building, but require
                // that the sub-commands disable themselves.
                EnvDTE.DTE dte = this.DTE;
                cmd.Visible = cmd.Supported && IsSharePointInstalled && DoLoadedSharePointProjectsExist(dte, false);
                cmd.Enabled = cmd.Visible;
            }
        }

        private void QuickDeploySelection_BeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                // Quick Deploy menu is only shown if a SP project is selected, but we do not require it to be a farm solution
                // as some of the sub-commands can operate on sandboxed solutions.  We also show it if building, but require
                // that the sub-commands disable themselves.
                cmd.Visible = cmd.Supported && IsSharePointInstalled && IsLoadedSharePointProjectSelected(DTE, false);
                cmd.Enabled = cmd.Visible;
                this.UpdateSelectionText(cmd, "Quick Deploy", "(CKSDEV)");
            }
        }

        #endregion

        #region Copy to Root (File + Folder) Handlers

        /// <summary>
        /// Handles the BeforeQueryStatus event of the CopySharePointRootFolder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CopySharePointRootFolder_BeforeQueryStatus(object sender, EventArgs e)
        {
            this.CopySharePointRootFile_BeforeQueryStatus(sender, e);
        }

        /// <summary>
        /// Handles the BeforeQueryStatus event of the CopySharePointRootFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CopySharePointRootFile_BeforeQueryStatus(object sender, EventArgs e)
        {
            ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;

            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                // Default to not show this command at all.
                cmd.Visible = cmd.Enabled = false;

                // Determine if we have at least one deployable SharePoint item - return as soon as we find one.
                EnvDTE.DTE dte = this.DTE;
                foreach (SelectedItem item in dte.SelectedItems)
                {
                    // Only respond if we actually have a project item (i.e. not to projects).
                    if (item.ProjectItem != null)
                    {
                        if (projectService != null && projectService.IsSharePointInstalled)
                        {
                            List<QuickCopyableSharePointArtefact> artefacts = QuickDeploymentUtilities.ResolveProjectItemToArtefacts(projectService, item.ProjectItem);
                            if ((artefacts != null) && (artefacts.Count > 0))
                            {
                                // We have a quick copyable SharePoint file, so at least show the command.
                                cmd.Visible = true;

                                // For now, just enable the command evne if the item is not included in the package.
                                // This is for performance reasons - we should error if it is not on click.
                                cmd.Enabled = true;
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the BeforeQueryStatus event of the OpenDeploymentLocationFolder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OpenDeploymentLocationFolder_BeforeQueryStatus(object sender, EventArgs e)
        {
            ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;

            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                // Default to not show this command at all.
                cmd.Visible = cmd.Enabled = false;

                // Determine if we have a single item selected
                EnvDTE.DTE dte = this.DTE;
                if (dte.SelectedItems.Count == 1)
                {
                    // 1-based index!
                    SelectedItem item = dte.SelectedItems.Item(1);
                    // Only respond if we actually have a project item (i.e. not to projects).
                    if ((item != null) && (item.ProjectItem != null))
                    {
                        if (projectService != null && projectService.IsSharePointInstalled)
                        {
                            bool found = false;
                            ISharePointProjectItemFile spFile = projectService.Convert<ProjectItem, ISharePointProjectItemFile>(item.ProjectItem);
                            if ((spFile != null) && (!String.IsNullOrEmpty(spFile.DeploymentPath)))
                            {
                                found = true;
                            }
                            else
                            {
                                IMappedFolder spFolder = projectService.Convert<ProjectItem, IMappedFolder>(item.ProjectItem);
                                if (spFolder != null)
                                {
                                    found = true;
                                }
                            }

                            ProjectItem parentProjItem = item.ProjectItem.Collection.Parent as ProjectItem;
                            String folderPath = String.Empty;
                            while ((!found) && (parentProjItem != null))
                            {
                                IMappedFolder spFolder = projectService.Convert<ProjectItem, IMappedFolder>(parentProjItem);
                                if (spFolder != null)
                                {
                                    found = true;
                                }
                                parentProjItem = parentProjItem.Collection.Parent as ProjectItem;
                            }

                            cmd.Visible = found;
                            cmd.Enabled = found;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the CopySharePointRootFile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CopySharePointRootFile_Click(object sender, EventArgs arguments)
        {
            ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;

            // Convert the DTE project into a SharePoint project. If the conversion fails, this is not a SP project.
            EnvDTE.DTE dte = this.DTE;
            foreach (SelectedItem item in dte.SelectedItems)
            {
                // Only respond if we actually have a project item (i.e. not to projects).
                if (item.ProjectItem != null)
                {
                    if (projectService != null && projectService.IsSharePointInstalled)
                    {
                        IEnumerable<QuickCopyableSharePointArtefact> artefacts = QuickDeploymentUtilities.ResolveProjectItemToArtefacts(projectService, item.ProjectItem);
                        artefacts = artefacts.Where(af => af.IsPackaged(projectService));

                        StatusBarLogger.Instance.SetStatus("Copying to SharePoint Root...");

                        CKSDEVPackage.SharePointProjectService.Logger.ActivateOutputWindow();
                        CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Copying to SharePoint Root ==========", LogCategory.Status);

                        if (artefacts.Count() > 0)
                        {
                            foreach (QuickCopyableSharePointArtefact artefact in artefacts)
                            {
                                artefact.QuickCopy(projectService, true);
                            }

                            CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Copy to SharePoint Root succeeded ==========", LogCategory.Status);
                            StatusBarLogger.Instance.SetStatus("Copying to SharePoint Root... All Done!");
                        }
                        else
                        {
                            CKSDEVPackage.SharePointProjectService.Logger.WriteLine("ERROR: Unable to Copy to SharePoint Root. Ensure at least one selected item is included in a package.", LogCategory.Error);

                            CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Copying to SharePoint Root failed ==========", LogCategory.Status);

                            StatusBarLogger.Instance.SetStatus("Copying to SharePoint Root... ERROR!");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the OpenDeploymentLocationFolder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OpenDeploymentLocationFolder_Click(object sender, EventArgs arguments)
        {
            ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;

            // Convert the DTE project into a SharePoint project. If the conversion fails, this is not a SP project.
            EnvDTE.DTE dte = this.DTE;
            if (dte.SelectedItems.Count == 1)
            {
                // 1-based index!
                SelectedItem item = dte.SelectedItems.Item(1);
                // Only respond if we actually have a project item (i.e. not to projects).
                if ((item != null) && (item.ProjectItem != null))
                {
                    if (projectService != null && projectService.IsSharePointInstalled)
                    {
                        String path = null;

                        ISharePointProjectItemFile spFile = projectService.Convert<ProjectItem, ISharePointProjectItemFile>(item.ProjectItem);
                        if ((spFile != null) && (!String.IsNullOrEmpty(spFile.DeploymentPath)))
                        {
                            path = Path.Combine(QuickDeploymentUtilities.SubstituteRootTokens(spFile.Project, spFile.DeploymentRoot), spFile.DeploymentPath);
                        }
                        else
                        {
                            IMappedFolder spFolder = projectService.Convert<ProjectItem, IMappedFolder>(item.ProjectItem);
                            if (spFolder != null)
                            {
                                path = Path.Combine(QuickDeploymentUtilities.SubstituteRootTokens(spFolder.Project, "{SharePointRoot}"), spFolder.DeploymentLocation);
                            }
                        }

                        ProjectItem projItem = item.ProjectItem;
                        String folderPath = String.Empty;
                        while ((String.IsNullOrEmpty(path)) && (projItem != null))
                        {

                            IMappedFolder spFolder = projectService.Convert<ProjectItem, IMappedFolder>(projItem);
                            if (spFolder != null)
                            {
                                path = Path.Combine(QuickDeploymentUtilities.SubstituteRootTokens(spFolder.Project, "{SharePointRoot}"), spFolder.DeploymentLocation, folderPath);
                            }

                            if (projItem.Kind == vsProjectItemKindPhysicalFolder)
                            {
                                folderPath = projItem.Name + "\\" + folderPath;
                            }

                            projItem = projItem.Collection.Parent as ProjectItem;
                        }

                        if (!String.IsNullOrEmpty(path))
                        {
                            OpenDeploymentLocationFolder(path);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Opens the deployment location folder
        /// </summary>
        /// <param name="path">Path of the folder to be opened.</param>
        public void OpenDeploymentLocationFolder(String path)
        {
            if (Directory.Exists(path))
            {
                System.Diagnostics.Process explorer = new System.Diagnostics.Process();
                explorer.StartInfo.FileName = "explorer.exe";
                explorer.StartInfo.Arguments = path;
                explorer.Start();
            }
            else
            {
                ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;
                projectService.Logger.ActivateOutputWindow();
                projectService.Logger.WriteLine(String.Format("Path '{0}' not found.", path), LogCategory.Status);
            }
        }

        /// <summary>
        /// Handles the Click event of the CopySharePointRootFolder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CopySharePointRootFolder_Click(object sender, EventArgs arguments)
        {
            this.CopySharePointRootFile_BeforeQueryStatus(sender, arguments);
        }

        #endregion

        #region CopySharePointRoot Command Handlers

        private void CopySharePointRootContext_Click(object sender, EventArgs arguments)
        {
            this.CustomDeploy(DeployType.CopySharePointRoot, false);
        }

        private void CopySharePointRootSolution_Click(object sender, EventArgs arguments)
        {
            this.CustomDeploy(DeployType.CopySharePointRoot, true);
        }

        private void CopySharePointRootSelection_Click(object sender, EventArgs arguments)
        {
            this.CustomDeploy(DeployType.CopySharePointRoot, false);
        }

        #endregion

        #region CopyBinary Command Handlers

        private void CopyBinaryContext_Click(object sender, EventArgs arguments)
        {
            this.CustomDeploy(DeployType.CopyBinary, false);
        }

        private void CopyBinarySolution_Click(object sender, EventArgs arguments)
        {
            this.CustomDeploy(DeployType.CopyBinary, true);
        }

        private void CopyBinarySelection_Click(object sender, EventArgs arguments)
        {
            this.CustomDeploy(DeployType.CopyBinary, false);
        }

        #endregion

        #region CopyBoth Command Handlers

        private void CopyBothContext_Click(object sender, EventArgs arguments)
        {
            this.CustomDeploy(DeployType.CopyBoth, false);
        }

        private void CopyBothSolution_Click(object sender, EventArgs arguments)
        {
            this.CustomDeploy(DeployType.CopyBoth, true);
        }

        private void CopyBothSelection_Click(object sender, EventArgs arguments)
        {
            this.CustomDeploy(DeployType.CopyBoth, false);
        }

        #endregion

        #region RecycleAppPool Command Handlers

        private void RecycleAppPoolContext_Click(object sender, EventArgs arguments)
        {
            RecycleSelectedPools(false);
        }

        private void RecycleAppPoolSolution_Click(object sender, EventArgs arguments)
        {
            RecycleSelectedPools(true);
        }

        private void RecycleAppPoolSelection_Click(object sender, EventArgs arguments)
        {
            RecycleSelectedPools(false);
        }

        private void RecycleSelectedPools(bool isSolutionContext)
        {
            ISharePointProject[] projects = GetSharePointProjects(isSolutionContext, false);
            List<string> appPoolNames = new List<string>();
            foreach (ISharePointProject project in projects)
            {
                string name = RecycleUtilities.GetApplicationPoolName(CKSDEVPackage.SharePointProjectService, project.SiteUrl.ToString());
                if (!String.IsNullOrEmpty(name))
                {
                    appPoolNames.Add(name);
                }
            }

            CKSDEVPackage.SharePointProjectService.Logger.ActivateOutputWindow();

            //OutputWindowLogger.Instance.ClearAndActivate();
            RecycleAppPools(appPoolNames.Distinct().ToArray());
        }

        private void RecycleAllAppPools_Click(object sender, EventArgs arguments)
        {
            try
            {
                CKSDEVPackage.SharePointProjectService.Logger.ActivateOutputWindow();

                //OutputWindowLogger.Instance.ClearAndActivate();
                string[] names = RecycleUtilities.GetAllApplicationPoolNames(this.CKSDEVPackage.SharePointProjectService);
                RecycleAppPools(names);
            }
            catch
            {
            }
        }

        private void RecycleAppPools(string[] names)
        {
            StatusBarLogger.Instance.SetStatus("Recycling Application Pool(s)...");
            CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Recycling Application Pool(s) ==========", LogCategory.Status);

            //OutputWindowLogger.Instance.WriteLine("========== Recycling Application Pool(s) ==========", LogCategory.Status);

            if (names.Length == 0)
            {
                CKSDEVPackage.SharePointProjectService.Logger.WriteLine("No Application Pools found to recycle!", LogCategory.Warning);

                //OutputWindowLogger.Instance.WriteLine("No Application Pools found to recycle!", LogCategory.Warning);
            }

            foreach (string name in names)
            {
                try
                {
                    CKSDEVPackage.SharePointProjectService.Logger.WriteLine("Recycling Application Pool: " + name, LogCategory.Status);

                    //OutputWindowLogger.Instance.WriteLine("Recycling Application Pool: " + name, LogCategory.Status);
                    RecycleUtilities.RecycleApplicationPool(name);
                }
                catch (Exception ex)
                {
                    CKSDEVPackage.SharePointProjectService.Logger.WriteLine("EXCEPTION: " + ex.Message, LogCategory.Error);

                    //OutputWindowLogger.Instance.WriteLine("EXCEPTION: " + ex.Message, LogCategory.Error);
                }
            }

            StatusBarLogger.Instance.SetStatus("Recycling Application Pool(s)... All Done!");
            CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Recycling Application Pool(s) completed ==========", LogCategory.Status);

            //OutputWindowLogger.Instance.WriteLine("========== Recycling Application Pool(s) completed ==========", LogCategory.Status);
        }

        #endregion

        #region Attach to Processes Command Handlers

        /// <summary>
        /// Sets the attach to process command visibility.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="processName">Name of the process.</param>
        private void SetAttachToProcessCommandVisibility(object sender, string processName)
        {
            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                EnvDTE.DTE dte = this.DTE;

                cmd.Visible = cmd.Supported; // && DoLoadedSharePointProjectsExist(dte);
                cmd.Enabled = IsSharePointInstalled && IsNotBuilding(dte); //&& IsLoadedSharePointProjectSelected(dte);

                if (cmd.Enabled)
                {
                    // Only show the command if worker processes exist.
                    try
                    {
                        ProcessUtilities utils = new ProcessUtilities(dte);
                        cmd.Enabled = utils.IsProcessAvailableByName(processName);
                    }
                    catch (Exception ex)
                    {
                        DTEManager.ProjectService.Logger.ActivateOutputWindow();
                        DTEManager.ProjectService.Logger.WriteLine(ex.ToString(), LogCategory.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Attacheds to named process.
        /// </summary>
        /// <param name="processName">Name of the process.</param>
        private void AttachToNamedProcess(string processName)
        {
            try
            {
                ProcessUtilities utils = new ProcessUtilities(this.DTE);
                utils.AttachToProcessByName(DTEManager.ActiveSharePointProject, processName);
            }
            catch (Exception ex)
            {
                DTEManager.ProjectService.Logger.ActivateOutputWindow();
                DTEManager.ProjectService.Logger.WriteLine(ex.ToString(), LogCategory.Error);
            }
        }

        #region Attach to All SharePoint Processes

        /// <summary>
        /// Handles the BeforeQueryStatus event of the AttachToAllSharePointProcesses control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AttachToAllSharePointProcesses_BeforeQueryStatus(object sender, EventArgs e)
        {
            //Assume if IIS is ok then we will show attach to all
            SetAttachToProcessCommandVisibility(sender, ProcessConstants.IISWorkerProcess);
        }

        /// <summary>
        /// Handles the Click event of the AttachToAllSharePointProcesses menu.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AttachToAllSharePointProcesses_Click(object sender, EventArgs arguments)
        {
            ProcessUtilities utils = new ProcessUtilities(this.DTE);

            //Try attach to the iis worker processes
            if (utils.IsProcessAvailableByName(ProcessConstants.IISWorkerProcess))
            {
                AttachToNamedProcess(ProcessConstants.IISWorkerProcess);
            }

            //Try attach to the user code process
            if (utils.IsProcessAvailableByName(ProcessConstants.SPUCWorkerProcess))
            {
                AttachToNamedProcess(ProcessConstants.SPUCWorkerProcess);
            }

            //Try attach to the OWS timer process
            if (utils.IsProcessAvailableByName(ProcessConstants.OWSTimerProcess))
            {
                AttachToNamedProcess(ProcessConstants.OWSTimerProcess);
            }

            //Try attach to the VSSPHost4 processes
            if (utils.IsProcessAvailableByName(ProcessConstants.VSSHost4Process))
            {
                AttachToNamedProcess(ProcessConstants.VSSHost4Process);
            }
        }

        #endregion

        #region Attach to IIS Processes

        /// <summary>
        /// Handles the BeforeQueryStatus event of the AttachToIISProcesses control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AttachToIISProcesses_BeforeQueryStatus(object sender, EventArgs e)
        {
            SetAttachToProcessCommandVisibility(sender, ProcessConstants.IISWorkerProcess);
        }

        /// <summary>
        /// Handles the Click event of the AttachToIISProcesses menu.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AttachToIISProcesses_Click(object sender, EventArgs arguments)
        {
            AttachToNamedProcess(ProcessConstants.IISWorkerProcess);
        }

        #endregion

        #region Attach to User Code Process

        /// <summary>
        /// Handles the BeforeQueryStatus event of the AttachToUserCodeProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AttachToUserCodeProcess_BeforeQueryStatus(object sender, EventArgs e)
        {
            SetAttachToProcessCommandVisibility(sender, ProcessConstants.SPUCWorkerProcess);
        }

        /// <summary>
        /// Handles the Click event of the AttachToUserCodeProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AttachToUserCodeProcess_Click(object sender, EventArgs arguments)
        {
            try
            {
                DTEManager.ProjectService.Logger.ActivateOutputWindow();
                DTEManager.ProjectService.Logger.WriteLine("========== Attaching to User Code Process ==========", LogCategory.Status);

                StatusBarLogger.Instance.SetStatus("Attaching to User Code Process...");

                AttachToNamedProcess(ProcessConstants.SPUCWorkerProcess);

                StatusBarLogger.Instance.SetStatus("Attaching to User Code Process... All Done!");
                DTEManager.ProjectService.Logger.WriteLine("Done!", LogCategory.Status);
            }
            catch (Exception ex)
            {
                DTEManager.ProjectService.Logger.WriteLine("EXCEPTION: " + ex.Message, LogCategory.Error);
                StatusBarLogger.Instance.SetStatus("Attaching to User Code Process.. ERROR!");
            }
        }

        #endregion

        #region Attach to OWSTimer Process

        /// <summary>
        /// Handles the BeforeQueryStatus event of the AttachToOWSTimerProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AttachToOWSTimerProcess_BeforeQueryStatus(object sender, EventArgs e)
        {
            SetAttachToProcessCommandVisibility(sender, ProcessConstants.OWSTimerProcess);
        }

        /// <summary>
        /// Handles the Click event of the AttachToOWSTimerProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AttachToOWSTimerProcess_Click(object sender, EventArgs arguments)
        {
            try
            {
                DTEManager.ProjectService.Logger.ActivateOutputWindow();
                DTEManager.ProjectService.Logger.WriteLine("========== Attaching to OWS Timer Process ==========", LogCategory.Status);

                StatusBarLogger.Instance.SetStatus("Attaching to OWS Timer Process...");

                AttachToNamedProcess(ProcessConstants.OWSTimerProcess);

                StatusBarLogger.Instance.SetStatus("Attaching to OWS Timer Process... All Done!");
                DTEManager.ProjectService.Logger.WriteLine("Done!", LogCategory.Status);
            }
            catch (Exception ex)
            {
                DTEManager.ProjectService.Logger.WriteLine("EXCEPTION: " + ex.Message, LogCategory.Error);
                StatusBarLogger.Instance.SetStatus("Attaching to OWS Timer Process.. ERROR!");
            }
        }

        #endregion

        #region Attach to VSSPHost4 Process

        /// <summary>
        /// Handles the BeforeQueryStatus event of the AttachToVSSPHost4Process control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AttachToVSSPHost4Process_BeforeQueryStatus(object sender, EventArgs e)
        {
            SetAttachToProcessCommandVisibility(sender, ProcessConstants.VSSHost4Process);
        }

        /// <summary>
        /// Handles the Click event of the AttachToVSSPHost4Process control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AttachToVSSPHost4Process_Click(object sender, EventArgs arguments)
        {
            try
            {
                DTEManager.ProjectService.Logger.ActivateOutputWindow();
                DTEManager.ProjectService.Logger.WriteLine("========== Attaching to VSS Host4 Process ==========", LogCategory.Status);

                StatusBarLogger.Instance.SetStatus("Attaching to VSS Host4 Process...");

                AttachToNamedProcess(ProcessConstants.VSSHost4Process);

                StatusBarLogger.Instance.SetStatus("Attaching to VSS Host4 Process... All Done!");
                DTEManager.ProjectService.Logger.WriteLine("Done!", LogCategory.Status);
            }
            catch (Exception ex)
            {
                DTEManager.ProjectService.Logger.WriteLine("EXCEPTION: " + ex.Message, LogCategory.Error);
                StatusBarLogger.Instance.SetStatus("Attaching to VSS Host4 Process.. ERROR!");
            }
        }

        #endregion

        #endregion

        #region Restart Processes Command Handlers

        /// <summary>
        /// Sets the restart process command visibility.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="processName">Name of the process.</param>
        private void SetRestartProcessCommandVisibility(object sender, string processName)
        {
            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                EnvDTE.DTE dte = this.DTE;

                cmd.Visible = cmd.Supported;
                cmd.Enabled = IsSharePointInstalled && IsNotBuilding(dte);

                if (cmd.Enabled)
                {
                    // Only show the command if worker processes exist.
                    try
                    {
                        ProcessUtilities utils = new ProcessUtilities(dte);
                        cmd.Enabled = utils.IsProcessAvailableByName(processName);
                    }
                    catch (Exception ex)
                    {
                        DTEManager.ProjectService.Logger.ActivateOutputWindow();
                        DTEManager.ProjectService.Logger.WriteLine(ex.ToString(), LogCategory.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Restarts named process.
        /// </summary>
        /// <param name="processName">Name of the process.</param>
        private void RestartNamedProcess(string processName)
        {
            try
            {
                ProcessUtilities utils = new ProcessUtilities(this.DTE);
                utils.RestartProcess(processName, 10);
            }
            catch (Exception ex)
            {
                DTEManager.ProjectService.Logger.ActivateOutputWindow();
                DTEManager.ProjectService.Logger.WriteLine(ex.ToString(), LogCategory.Error);
            }
        }



        /// <summary>
        /// Handles the Click event of the RestartIIS control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RestartIIS_Click(object sender, EventArgs arguments)
        {
            RestartIIS();
        }

        /// <summary>
        /// Restarts the IIS.
        /// </summary>
        private void RestartIIS()
        {
            try
            {
                DTEManager.ProjectService.Logger.ActivateOutputWindow();
                DTEManager.ProjectService.Logger.WriteLine("========== Restarting IIS ==========", LogCategory.Status);

                StatusBarLogger.Instance.SetStatus("Restarting IIS...");

                RecycleUtilities.RestartIIS();

                StatusBarLogger.Instance.SetStatus("Restarting IIS... All Done!");
                DTEManager.ProjectService.Logger.WriteLine("Done!", LogCategory.Status);
            }
            catch (Exception ex)
            {
                DTEManager.ProjectService.Logger.WriteLine("EXCEPTION: " + ex.Message, LogCategory.Error);
                StatusBarLogger.Instance.SetStatus("Restarting IIS... ERROR!");
            }
        }


        #region Restart User Code Process

        /// <summary>
        /// Handles the BeforeQueryStatus event of the RestartUserCodeProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RestartUserCodeProcess_BeforeQueryStatus(object sender, EventArgs e)
        {
            SetRestartProcessCommandVisibility(sender, ProcessConstants.SPUCWorkerProcess);
        }

        /// <summary>
        /// Handles the Click event of the RestartUserCodeProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RestartUserCodeProcess_Click(object sender, EventArgs arguments)
        {
            try
            {
                DTEManager.ProjectService.Logger.ActivateOutputWindow();
                DTEManager.ProjectService.Logger.WriteLine("========== Restarting User Code Process ==========", LogCategory.Status);

                StatusBarLogger.Instance.SetStatus("Restarting User Code Process...");

                RestartNamedProcess(ProcessConstants.SPUCWorkerProcessName);

                StatusBarLogger.Instance.SetStatus("Restarting User Code Process... All Done!");
                DTEManager.ProjectService.Logger.WriteLine("Done!", LogCategory.Status);
            }
            catch (Exception ex)
            {
                DTEManager.ProjectService.Logger.WriteLine("EXCEPTION: " + ex.Message, LogCategory.Error);
                StatusBarLogger.Instance.SetStatus("Restarting User Code Process... ERROR!");
            }
        }

        #endregion

        #region Restart OWSTimer Process

        /// <summary>
        /// Handles the BeforeQueryStatus event of the RestartOWSTimerProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RestartOWSTimerProcess_BeforeQueryStatus(object sender, EventArgs e)
        {
            SetRestartProcessCommandVisibility(sender, ProcessConstants.OWSTimerProcess);
        }

        /// <summary>
        /// Handles the Click event of the RestartOWSTimerProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RestartOWSTimerProcess_Click(object sender, EventArgs arguments)
        {
            try
            {
                DTEManager.ProjectService.Logger.ActivateOutputWindow();
                DTEManager.ProjectService.Logger.WriteLine("========== Restarting OWS Timer Process ==========", LogCategory.Status);

                StatusBarLogger.Instance.SetStatus("Restarting OWS Timer Process...");

                RestartNamedProcess(ProcessConstants.OWSTimerProcessName);

                StatusBarLogger.Instance.SetStatus("Restarting OWS Timer Process... All Done!");
                DTEManager.ProjectService.Logger.WriteLine("Done!", LogCategory.Status);
            }
            catch (Exception ex)
            {
                DTEManager.ProjectService.Logger.WriteLine("EXCEPTION: " + ex.Message, LogCategory.Error);
                StatusBarLogger.Instance.SetStatus("Restarting OWS Timer Process... ERROR!");
            }
        }

        #endregion

        #endregion

        #region Auto Save and Build Handlers

        private List<string> successfulBuiltProjects = null;
        private bool hasAnyBuildFailed = false;

        /// <summary>
        /// Builds the events_ on build begin.
        /// </summary>
        /// <param name="Scope">The scope.</param>
        /// <param name="Action">The action.</param>
        private void BuildEvents_OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
        {
            // Clear our cheeky little cache of successfuly built projects.
            this.successfulBuiltProjects = new List<string>();
            this.hasAnyBuildFailed = false;
        }

        /// <summary>
        /// Builds the events_ on build proj config done.
        /// </summary>
        /// <param name="Project">The project.</param>
        /// <param name="ProjectConfig">The project config.</param>
        /// <param name="Platform">The platform.</param>
        /// <param name="SolutionConfig">The solution config.</param>
        /// <param name="Success">if set to <c>true</c> [success].</param>
        public void BuildEvents_OnBuildProjConfigDone(string Project, string ProjectConfig, string Platform, string SolutionConfig, bool Success)
        {
            if (Success)
            {
                // Add this project to the list of those built successfully.  This list is used
                // so that we can do one Auto Quick Deploy at completion, and only if all builds were successful.
                this.successfulBuiltProjects.Add(Project);
            }
            else
            {
                this.hasAnyBuildFailed = true;
            }
        }

        /// <summary>
        /// Builds the events_ on build done.
        /// </summary>
        /// <param name="Scope">The scope.</param>
        /// <param name="Action">The action.</param>
        public void BuildEvents_OnBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            // Don't auto copy if there is no SharePoint, or ANY build failed.
            if (!CKSDEVPackage.SharePointProjectService.IsSharePointInstalled || hasAnyBuildFailed)
            {
                return;
            }

            // We will never auto copy on deploy - just build or rebuild.
            if (Action == vsBuildAction.vsBuildActionBuild || Action == vsBuildAction.vsBuildActionRebuildAll)
            {
                // Get all farm SP projects where the auto copy flag is set, and where the project was built succesfully.
                IEnumerable<ISharePointProject> spProjects = GetSharePointProjects(Scope == vsBuildScope.vsBuildScopeSolution, true)
                    .Where(proj => AutoCopyAssembliesProperty.GetFromProject(proj)
                    && successfulBuiltProjects.Any(sbp => proj.FullPath.EndsWith(sbp)));
                if (spProjects.Count() > 0)
                {
                    // We don't clear the log since we want this to show straight after the build details.
                    StatusBarLogger.Instance.SetStatus("Auto Copying to GAC/BIN...");
                    CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Auto Copying to GAC/BIN ==========", LogCategory.Status);

                    //OutputWindowLogger.Instance.WriteLine("========== Auto Copying to GAC/BIN ==========", LogCategory.Status);

                    List<string> appPools = new List<string>();

                    foreach (ISharePointProject spProject in spProjects)
                    {
                        new SharePointPackageArtefact(spProject).QuickCopyBinaries(true);

                        // Get the app pool for recycling while we are here.
                        string appPool = RecycleUtilities.GetApplicationPoolName(CKSDEVPackage.SharePointProjectService, spProject.SiteUrl.ToString());
                        if (!String.IsNullOrEmpty(appPool))
                        {
                            appPools.Add(appPool);
                        }
                    }

                    CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Auto Copy to GAC/BIN succeeded ==========", LogCategory.Status);

                    //OutputWindowLogger.Instance.WriteLine("========== Auto Copy to GAC/BIN succeeded ==========", LogCategory.Status);
                    StatusBarLogger.Instance.SetStatus("Auto Copying to GAC/BIN... All Done!");

                    if (appPools.Count > 0)
                    {
                        this.RecycleAppPools(appPools.Distinct().ToArray());
                    }
                    else
                    {
                        this.RestartIIS();
                    }
                }
            }
        }

        /// <summary>
        /// Docs the events_ document saved.
        /// </summary>
        /// <param name="document">The document.</param>
        public void DocEvents_DocumentSaved(Document document)
        {
            if (!CKSDEVPackage.SharePointProjectService.IsSharePointInstalled)
            {
                return;
            }

            // Check this document is in a SP project.
            EnvDTE.Project dteProject = document.ProjectItem.ContainingProject;
            ProjectManager manager = ProjectManager.Create(dteProject);
            ISharePointProject spProject = manager.Project;
            if (spProject != null)
            {
                // Document was contained in a SharePoint project.  Check if the Auto Copy flag is set.
                bool isAutoCopyToRoot = AutoCopyToSharePointRootProperty.GetFromProject(spProject);
                if (isAutoCopyToRoot)
                {
                    IEnumerable<QuickCopyableSharePointArtefact> items = QuickDeploymentUtilities.ResolveProjectItemToArtefacts(CKSDEVPackage.SharePointProjectService, document.ProjectItem);
                    items = items.Where(af => af.IsPackaged(CKSDEVPackage.SharePointProjectService));
                    if (items != null && items.Count() > 0)
                    {
                        StatusBarLogger.Instance.SetStatus("Auto Copying to SharePoint Root...");
                        CKSDEVPackage.SharePointProjectService.Logger.ActivateOutputWindow();
                        CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Auto Copying to SharePoint Root ==========", LogCategory.Status);

                        //OutputWindowLogger.Instance.WriteLine("========== Auto Copying to SharePoint Root ==========", LogCategory.Status);

                        foreach (QuickCopyableSharePointArtefact item in items)
                        {
                            item.QuickCopy(CKSDEVPackage.SharePointProjectService, true);
                        }

                        CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Auto Copy to SharePoint Root succeeded ==========", LogCategory.Status);

                        //OutputWindowLogger.Instance.WriteLine("========== Auto Copy to SharePoint Root succeeded ==========", LogCategory.Status);
                        StatusBarLogger.Instance.SetStatus("Auto Copying to SharePoint Root... All Done!");
                    }
                }
            }
        }

        #endregion

        #region Help Handlers

        /// <summary>
        /// Handles the BeforeQueryStatus event of the CodePlexHome control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CodePlexHome_BeforeQueryStatus(object sender, EventArgs e)
        {
            ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;

            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                cmd.Visible = true;
                cmd.Enabled = true;

                return;
            }
        }

        /// <summary>
        /// Handles the Click event of the CodePlexHome control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CodePlexHome_Click(object sender, EventArgs arguments)
        {
            ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;

            ProcessUtilities utils = new ProcessUtilities(this.DTE);
            CKSDEVPackage.SharePointProjectService.Logger.ActivateOutputWindow();
            CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Opening CKSDev Home Page ==========", LogCategory.Status);

            utils.ExecuteBrowserUrlProcess(Resources.Url_CodePlexHome);
        }

        /// <summary>
        /// Handles the BeforeQueryStatus event of the CodePlexDocumentation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CodePlexDocumentation_BeforeQueryStatus(object sender, EventArgs e)
        {
            ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;

            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                cmd.Visible = true;
                cmd.Enabled = true;

                return;
            }
        }

        /// <summary>
        /// Handles the Click event of the CodePlexDocumentation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CodePlexDocumentation_Click(object sender, EventArgs arguments)
        {
            ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;

            ProcessUtilities utils = new ProcessUtilities(this.DTE);
            CKSDEVPackage.SharePointProjectService.Logger.ActivateOutputWindow();
            CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Opening CKSDev Documentation ==========", LogCategory.Status);

            utils.ExecuteBrowserUrlProcess(Resources.Url_CodePlexDocumentation);
        }

        /// <summary>
        /// Handles the BeforeQueryStatus event of the CodePlexNewFeature control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CodePlexNewFeature_BeforeQueryStatus(object sender, EventArgs e)
        {
            ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;

            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                cmd.Visible = true;
                cmd.Enabled = true;

                return;
            }
        }

        /// <summary>
        /// Handles the Click event of the CodePlexNewFeature control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CodePlexNewFeature_Click(object sender, EventArgs arguments)
        {
            ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;

            ProcessUtilities utils = new ProcessUtilities(this.DTE);
            CKSDEVPackage.SharePointProjectService.Logger.ActivateOutputWindow();
            CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Opening CKSDev New Feature ==========", LogCategory.Status);

            utils.ExecuteBrowserUrlProcess(Resources.Url_CodePlexNewFeature);
        }

        /// <summary>
        /// Handles the BeforeQueryStatus event of the CodePlexNewIssue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CodePlexNewIssue_BeforeQueryStatus(object sender, EventArgs e)
        {
            ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;

            OleMenuCommand cmd = sender as OleMenuCommand;
            if (null != cmd)
            {
                cmd.Visible = true;
                cmd.Enabled = true;

                return;
            }
        }

        /// <summary>
        /// Handles the Click event of the CodePlexNewIssue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arguments">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CodePlexNewIssue_Click(object sender, EventArgs arguments)
        {
            ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;

            ProcessUtilities utils = new ProcessUtilities(this.DTE);
            CKSDEVPackage.SharePointProjectService.Logger.ActivateOutputWindow();
            CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Opening CKSDev New Issue ==========", LogCategory.Status);

            utils.ExecuteBrowserUrlProcess(Resources.Url_CodePlexNewIssue);
        }

        #endregion

        #region Visibility Utility Methods

        /// <summary>
        /// Determines whether [is not building] [the specified DTE].
        /// </summary>
        /// <param name="dte">The DTE.</param>
        /// <returns>
        /// 	<c>true</c> if [is not building] [the specified DTE]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsNotBuilding(EnvDTE.DTE dte)
        {
            return dte.Solution.SolutionBuild.BuildState != vsBuildState.vsBuildStateInProgress;
        }

        /// <summary>
        /// Determines whether [is loaded share point project selected] [the specified DTE].
        /// </summary>
        /// <param name="dte">The DTE.</param>
        /// <param name="requireFarm">if set to <c>true</c> [require farm].</param>
        /// <returns>
        /// 	<c>true</c> if [is loaded share point project selected] [the specified DTE]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsLoadedSharePointProjectSelected(EnvDTE.DTE dte, bool requireFarm)
        {
            // We must loop to check a project is not only selecetd but loaded.
            foreach (SelectedItem item in dte.SelectedItems)
            {
                // If selected item is a project, and it is loaded, and it is a SharePoint project, then at least one project is selected.
                if ((item.Project != null) && (item.Project.ConfigurationManager != null) && IsSharePointProject(item.Project, requireFarm))
                {
                    return true;
                }
            }
            // no SP project selected, check startup projects
            SolutionBuild solutionBuild = dte.Solution.SolutionBuild;
            if (solutionBuild != null)
            {
                Array startupProj = solutionBuild.StartupProjects as Array;
                if (startupProj != null)
                {
                    foreach (String projectName in startupProj)
                    {
                        Project project = GetProjectByName(projectName);
                        if ((project != null) && (project.ConfigurationManager != null) && IsSharePointProject(project, requireFarm))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private Project GetProjectByName(String projectName, bool isUniqueName = true)
        {
            Project result = null;
            foreach (Project project in DTE.Solution.Projects)
            {
                if (((isUniqueName) && (project.UniqueName == projectName)) || ((!isUniqueName) && (project.Name == projectName)))
                {
                    result = project;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Does the loaded share point projects exist.
        /// </summary>
        /// <param name="dte">The DTE.</param>
        /// <param name="requireFarm">if set to <c>true</c> [require farm].</param>
        /// <returns></returns>
        private bool DoLoadedSharePointProjectsExist(EnvDTE.DTE dte, bool requireFarm)
        {
            // We use the fact that ConfigurationManager is null for an unloaded project to detect loaded projects.
            foreach (EnvDTE.Project project in dte.Solution.Projects)
            {
                if (project.ConfigurationManager != null && IsSharePointProject(project, requireFarm))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Detects if a project is a SharePoint project based on a list of project type GUIDs.
        /// </summary>
        private bool IsSharePointProject(EnvDTE.Project project, bool requireFarm)
        {
            try
            {
                ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;
                if (projectService != null)
                {
                    // Convert the DTE project into a SharePoint project. If the conversion fails, this is not a SP project.
                    ISharePointProject p = projectService.Convert<EnvDTE.Project, Microsoft.VisualStudio.SharePoint.ISharePointProject>(project);
                    if (p != null && requireFarm)
                    {
                        return !p.IsSandboxedSolution;
                    }
                    else
                    {
                        return p != null;
                    }
                }
            }
            catch
            {
                // Must be VERY careful not to throw exceptions in here, since this is called on every click of the context menu.
            }

            return false;
        }

        /// <summary>
        /// Gets the share point projects.
        /// </summary>
        /// <param name="isSolutionContext">if set to <c>true</c> [is solution context].</param>
        /// <param name="requireFarm">if set to <c>true</c> [require farm].</param>
        /// <returns></returns>
        private ISharePointProject[] GetSharePointProjects(bool isSolutionContext, bool requireFarm)
        {
            List<EnvDTE.Project> candidateProjects = new List<EnvDTE.Project>();

            if (isSolutionContext)
            {
                foreach (EnvDTE.Project project in DTE.Solution.Projects)
                {
                    candidateProjects.Add(project);
                }
            }
            else
            {
                foreach (SelectedItem item in DTE.SelectedItems)
                {
                    if (item.Project != null)
                    {
                        candidateProjects.Add(item.Project);
                    }
                }
            }

            List<ISharePointProject> spProjects = new List<ISharePointProject>();

            foreach (EnvDTE.Project project in candidateProjects)
            {
                ProjectManager manager = ProjectManager.Create(project);
                ISharePointProject spProject = manager.Project;
                if (spProject != null)
                {
                    if ((!spProject.IsSandboxedSolution) || (!requireFarm))
                    {
                        spProjects.Add(spProject);
                    }
                }
            }

            // no SP project selected, check startup projects
            if (spProjects.Count == 0)
            {
                SolutionBuild solutionBuild = DTE.Solution.SolutionBuild;
                if (solutionBuild != null)
                {
                    Array startupProj = solutionBuild.StartupProjects as Array;
                    if (startupProj != null)
                    {
                        foreach (String projectName in startupProj)
                        {
                            Project project = GetProjectByName(projectName);
                            if (project != null)
                            {
                                ProjectManager manager = ProjectManager.Create(project);
                                ISharePointProject spProject = manager.Project;
                                if (spProject != null)
                                {
                                    if ((!spProject.IsSandboxedSolution) || (!requireFarm))
                                    {
                                        spProjects.Add(spProject);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return spProjects.ToArray();
        }

        #endregion

        #region Custom Deployment - Project Level

        /// <summary>
        /// Gets the active project.
        /// </summary>
        /// <param name="dte">The DTE.</param>
        /// <returns></returns>
        internal static EnvDTE.Project GetActiveProject(DTE dte)
        {
            EnvDTE.Project activeProject = null;

            Array activeSolutionProjects = dte.ActiveSolutionProjects as Array;
            if (activeSolutionProjects != null && activeSolutionProjects.Length > 0)
            {
                activeProject = activeSolutionProjects.GetValue(0) as EnvDTE.Project;
            }

            return activeProject;
        }

        /// <summary>
        /// Customs the deploy.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="isSolution">if set to <c>true</c> [is solution].</param>
        private void CustomDeploy(DeployType type, bool isSolution)
        {
            // Get reference to DTE automation object and project service.
            EnvDTE.DTE dte = this.DTE;
            ISharePointProjectService projectService = CKSDEVPackage.SharePointProjectService;

            bool canDeploy = true;

            // Calling Build.Package will correctly call Package on all projects in solution, or the selected project(s).
            //dte.ExecuteCommand("Build.Package", "");

            // Convert the DTE project into a SharePoint project. If the conversion fails, this is not a SP project.
            ISharePointProject[] projects = GetSharePointProjects(isSolution, true);

            if (type == DeployType.CopyBinary || type == DeployType.CopyBoth)
            {
                // Forcefully build any projects where this flag is set.  We let VS handle the "dirtiness" of a build,
                // so even if the user selected BUILD then COPY TO GAC, this should have minimal impact.
                IEnumerable<ISharePointProject> buildProjects = projects.Where(proj => BuildOnCopyAssembliesProperty.GetFromProject(proj));

                foreach (ISharePointProject buildProject in buildProjects)
                {

                    // just to be sure...
                    if ((buildProject != null) && (buildProject.Project != null))
                    {
                        String projectName = buildProject.Project.Name;
                        Project project = GetProjectByName(projectName, false);

                        if (project != null)
                        {
                            StatusBarLogger.Instance.SetStatus(String.Format("Building project: '{0}'...", projectName));
                            CKSDEVPackage.SharePointProjectService.Logger.WriteLine(String.Format("========== Building project '{0}' ==========", projectName),
                                LogCategory.Status);

                            string config = project.ConfigurationManager.ActiveConfiguration.ConfigurationName;

                            SolutionBuild2 builder = this.m_ApplicationObject2.Solution.SolutionBuild as SolutionBuild2;
                            builder.BuildProject(
                                config,
                                project.UniqueName,
                                true);
                            canDeploy = (builder.LastBuildInfo == 0);

                            // we don't build further projects if one fails
                            if (!canDeploy)
                            {
                                StatusBarLogger.Instance.SetStatus(String.Format("Building of project '{0}' failed, deployment stopped", projectName));
                                CKSDEVPackage.SharePointProjectService.Logger.WriteLine(String.Format("========== Building project '{0}' failed ==========", projectName),
                                    LogCategory.Status);
                                break;
                            }

                            StatusBarLogger.Instance.SetStatus(String.Format("Building project: '{0}'... Done!", projectName));
                            CKSDEVPackage.SharePointProjectService.Logger.WriteLine(String.Format("========== Building project '{0}' succeeded ==========", projectName),
                                LogCategory.Status);
                        }
                    }

                }

                // Recycle selected application pools first (if applicable), so we do not duplicate recycle for the same target 
                // URL used in multiple projects.
                if (canDeploy)
                {
                    this.RecycleSelectedPools(isSolution);
                }
            }

            if (((canDeploy)) && (type == DeployType.CopySharePointRoot || type == DeployType.CopyBoth))
            {
                StatusBarLogger.Instance.SetStatus("Copying to SharePoint Root...");
                CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Copying to SharePoint Root ==========", LogCategory.Status);

                //OutputWindowLogger.Instance.WriteLine("========== Copying to SharePoint Root ==========", LogCategory.Status);

                foreach (ISharePointProject project in projects)
                {
                    SharePointPackageArtefact package = new SharePointPackageArtefact(project);
                    package.QuickCopy(true);
                }

                CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Copy to SharePoint Root succeeded ==========", LogCategory.Status);

                //OutputWindowLogger.Instance.WriteLine("========== Copy to SharePoint Root succeeded ==========", LogCategory.Status);
                StatusBarLogger.Instance.SetStatus("Copying to SharePoint Root... All Done!");
            }

            if (((canDeploy)) && (type == DeployType.CopyBinary || type == DeployType.CopyBoth))
            {
                StatusBarLogger.Instance.SetStatus("Copying to GAC/BIN...");
                CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Copying to GAC/BIN ==========", LogCategory.Status);

                //OutputWindowLogger.Instance.WriteLine("========== Copying to GAC/BIN ==========", LogCategory.Status);

                foreach (ISharePointProject project in projects)
                {
                    SharePointPackageArtefact package = new SharePointPackageArtefact(project);
                    package.QuickCopyBinaries(true);
                }

                CKSDEVPackage.SharePointProjectService.Logger.WriteLine("========== Copying to GAC/BIN succeeded ==========", LogCategory.Status);

                //OutputWindowLogger.Instance.WriteLine("========== Copying to GAC/BIN succeeded ==========", LogCategory.Status);
                StatusBarLogger.Instance.SetStatus("Copying to GAC/BIN... All Done!");
            }
        }
        #endregion
    }

}

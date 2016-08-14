using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint;
using EnvDTE;
using EnvDTE80;
using System.ServiceProcess;
using Microsoft.Win32;

namespace CKS.Dev.VisualStudio.SharePoint.Environment
{
    /// <summary>
    /// Process Utilities
    /// </summary>
    class ProcessUtilities
    {

        /// <summary>
        /// Gets or sets the current DTE.
        /// </summary>
        /// <value>The current DTE.</value>
        public DTE CurrentDTE { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessUtilities"/> class.
        /// </summary>
        private ProcessUtilities()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessUtilities"/> class.
        /// </summary>
        /// <param name="dte">The DTE.</param>
        public ProcessUtilities(DTE dte)
        {
            CurrentDTE = dte;
        }

        /// <summary>
        /// Attaches the name of to process by.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="processName">Name of the process.</param>
        public void AttachToProcessByName(ISharePointProject project, String processName)
        {
            EnvDTE.Project dteProj = project.ProjectService.Convert<ISharePointProject, EnvDTE.Project>(project);
            project.ProjectService.Logger.ActivateOutputWindow();
            project.ProjectService.Logger.WriteLine(String.Format("Trying to attach to {0} process", processName), LogCategory.Message);
            
            if (AttachToProcessByName(processName))
            {
                project.ProjectService.Logger.WriteLine(String.Format("Debugger attached to {0} process", processName), LogCategory.Message);
            }
            else
            {
                project.ProjectService.Logger.WriteLine(String.Format("Failed to attach to {0} process", processName), LogCategory.Warning);
            }
        }

        /// <summary>
        /// Attaches the name of to process by.
        /// </summary>
        /// <param name="processName">Name of the process.</param>
        /// <returns></returns>
        public bool AttachToProcessByName(String processName)
        {
            Debugger2 debugger = CurrentDTE.Application.Debugger as Debugger2;
            if (debugger != null)
            {
                foreach (EnvDTE80.Process2 process in debugger.LocalProcesses)
                {
                    if (process.Name.ToUpper().LastIndexOf(processName.ToUpper()) == (process.Name.Length - processName.Length))
                    {
                        process.Attach();
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is process available by name] [the specified process name].
        /// </summary>
        /// <param name="processName">Name of the process.</param>
        /// <returns>
        /// 	<c>true</c> if [is process available by name] [the specified process name]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsProcessAvailableByName(String processName)
        {
            Debugger2 debugger = CurrentDTE.Application.Debugger as Debugger2;
            if (debugger != null)
            {
                foreach (EnvDTE80.Process2 process in debugger.LocalProcesses)
                {
                    if (process.Name.ToUpper().LastIndexOf(processName.ToUpper()) == (process.Name.Length - processName.Length))
                    {
                        return true;
                    }
                }                
            }
            return false;
        }

        /// <summary>
        /// Restarts the process.
        /// </summary>
        /// <param name="processName">Name of the process.</param>
        /// <param name="timeOut">The time out.</param>
        /// <returns></returns>
        public bool RestartProcess(String processName, int timeOut)
        {
            try
            {
                TimeSpan timeOutSpan = TimeSpan.FromSeconds(timeOut);
                ServiceController sc = new ServiceController(processName);
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, timeOutSpan);
                }
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, timeOutSpan);
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Executes the browser URL process.
        /// </summary>
        /// <param name="url">The URL.</param>
        public void ExecuteBrowserUrlProcess(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                // Start Internet Explorer. Defaults to the home page.

                string browser = GetDefaultBrowser();

                if (!String.IsNullOrEmpty(browser))
                {
                    System.Diagnostics.Process.Start(browser, uri.AbsoluteUri);
                }
                else
                {
                    System.Diagnostics.Process.Start("IExplore.exe", uri.AbsoluteUri);
                }
            }
            catch (UriFormatException)
            {
            }
            catch (ArgumentNullException)
            {
            }
        }

        /// <summary>
        /// Gets the default browser.
        /// </summary>
        /// <returns></returns>
        private string GetDefaultBrowser()
        {
            string browser = string.Empty;
            RegistryKey key = null;
            try
            {
                key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                //trim off quotes
                browser = key.GetValue(null).ToString().ToLower().Replace("\"", "");
                if (!browser.EndsWith("exe"))
                {
                    //get rid of everything after the ".exe"
                    browser = browser.Substring(0, browser.LastIndexOf(".exe") + 4);
                }
            }
            finally
            {
                if (key != null)
                {
                    key.Close();
                }
            }
            return browser;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using CKS.Dev.VisualStudio.SharePoint.Properties;

namespace CKS.Dev.VisualStudio.SharePoint
{
    /// <summary>
    /// Logger providing access to the Visual Studio StatusBar
    /// </summary>
    public class StatusBarLogger
    {
        #region Fields

        /// <summary>
        /// Field to hold the singleton.
        /// </summary>
        private static StatusBarLogger singleton = null;

        /// <summary>
        /// Field to hold the DTE.
        /// </summary>
        private DTE dte = null;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the StatusBarLogger instance.
        /// </summary>
        public static StatusBarLogger Instance
        {
            get
            {
                if (singleton == null)
                {
                    InitializeInstance(DTEManager.DTE);
                }
                return singleton;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialise an instance of StatusBarLogger.
        /// </summary>
        /// <param name="dte">The DTE.</param>
        public static void InitializeInstance(DTE dte)
        {
            singleton = new StatusBarLogger(dte);
        }

        /// <summary>
        /// Initialises a new StatusBarLogger.
        /// </summary>
        /// <param name="dte">The DTE.</param>
        private StatusBarLogger(DTE dte)
        {
            this.dte = dte;
        }

        /// <summary>
        /// Set the status of the status bar.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public void SetStatus(string message)
        {
            this.dte.StatusBar.Text = message;
        }

        #endregion
    }
}

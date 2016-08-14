using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Environment
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Environment
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Environment
#else
namespace CKS.Dev.VisualStudio.SharePoint.Environment
#endif
{
    /// <summary>
    /// The process identifiers.
    /// </summary>
    internal class ProcessConstants
    {
        /// <summary>
        /// The IISWorkerProcess Process.
        /// </summary>
        public const string IISWorkerProcess = "w3wp.exe";

        /// <summary>
        /// The IISProcess Process.
        /// </summary>
        public const string IISProcess = "iisreset.exe";

        /// <summary>
        /// The SPUCWorkerProcess Process.
        /// </summary>
        public const string SPUCWorkerProcess = "SPUCWorkerProcess.exe";

        /// <summary>
        /// The SPUCWorkerProcess Process Name.
        /// </summary>
        public const string SPUCWorkerProcessName = "SPUserCodeV4";

        /// <summary>
        /// The OWSTimerProcess Process.
        /// </summary>
        public const string OWSTimerProcess = "owstimer.exe";

        /// <summary>
        /// The OWSTimerProcess Process Name.
        /// </summary>
        public const string OWSTimerProcessName = "SPTimerV4";

        /// <summary>
        /// The VSSPHost4 Process VS2010.
        /// </summary>
        public const string VSSHost4Process = "vssphost4.exe";

        /// <summary>
        /// The VSSPHost5 Process VS2012.
        /// </summary>
        public const string VSSHost5Process = "vssphost5.exe";
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev11.VisualStudio.SharePoint.Commands.Info
{
    /// <summary>
    /// Contains information required to parse a control for use in a sandboxed solution. This class is 
    /// serializable so that instances of it can be sent between the Visual Studio and 
    /// SharePoint command assemblies.
    /// </summary>
    [Serializable]
    public class CompilationInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the user control name.
        /// </summary>
        public string UserControlName { get; set; }

        /// <summary>
        /// Gets or sets the in folder.
        /// </summary>
        public string InFolder { get; set; }

        /// <summary>
        /// Gets or sets the out folder.
        /// </summary>
        public string OutFolder { get; set; }

        /// <summary>
        /// Gets or sets the string name key file.
        /// </summary>
        public string StrongNameKeyFile { get; set; }

        #endregion
    }
}

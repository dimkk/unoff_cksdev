using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKS.Dev11.VisualStudio.SharePoint.Environment
{
    /// <summary>
    /// The deploy types.
    /// </summary>
    public enum DeployType
    {
        /// <summary>
        /// Copy to the SharePoint root.
        /// </summary>
        CopySharePointRoot = 1,
        /// <summary>
        /// Copy the binary.
        /// </summary>
        CopyBinary = 2,
        /// <summary>
        /// Copy both to the SharePoint root and the binary.
        /// </summary>
        CopyBoth = 3
    };
}
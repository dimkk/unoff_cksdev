using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint;
using EnvDTE;
using System.IO;
using VSLangProj;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.QuickDeployment
{
    /// <summary>
    /// Extensions to the Path object.
    /// </summary>
    public static class PathUtility
    {
        #region Methods

        /// <summary>
        /// Returns the last part of a path representing a folder, whether or not the path has
        /// a trailing directory separator.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        public static string GetLastFolderName(string fullPath)
        {
            string path = fullPath + "";
            if (fullPath.EndsWith(Path.DirectorySeparatorChar + ""))
            {
                path = path.Remove(path.Length - 1);
            }
            return Path.GetFileName(path);
        }

        /// <summary>
        /// Checks a file for the ReadOnly attribute, and removes it if it exists.
        /// </summary>
        /// <param name="fullPath"></param>
        public static void EnsureFileIsNotReadOnly(string fullPath)
        {
            FileAttributes attributes = File.GetAttributes(fullPath);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                attributes ^= FileAttributes.ReadOnly;
                File.SetAttributes(fullPath, attributes);
            }
        }

        #endregion
    }
}

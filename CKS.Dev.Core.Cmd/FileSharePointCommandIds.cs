﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Commands
#elif VS2013Build_SYMBOL
    namespace CKS.Dev12.VisualStudio.SharePoint.Commands
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Commands
#else
    namespace CKS.Dev.VisualStudio.SharePoint.Commands
#endif
    {
    /// <summary>
    /// SharePoint Command Ids for files.
    /// </summary>
    public static class FileSharePointCommandIds
    {
        #region Constants

        /// <summary>
        /// Get the file contents.
        /// </summary>
        public const string GetFileContentsCommand = "File.GetFileContents";

        /// <summary>
        /// Check out the file.
        /// </summary>
        public const string CheckOutFileCommand = "File.CheckOutFile";

        /// <summary>
        /// Discard the check out.
        /// </summary>
        public const string DiscardCheckOutCommand = "File.DiscardCheckOut";

        /// <summary>
        /// Check in the file.
        /// </summary>
        public const string CheckInFileCommand = "File.CheckInFile";

        /// <summary>
        /// Save the file.
        /// </summary>
        public const string SaveFileCommand = "File.SaveFile";

        /// <summary>
        /// Get the files.
        /// </summary>
        public const string GetFilesCommand = "File.GetFiles";

        /// <summary>
        /// Get the folders.
        /// </summary>
        public const string GetFoldersCommand = "File.GetFolders";

        /// <summary>
        /// Get the file properties.
        /// </summary>
        public const string GetFilePropertiesCommand = "File.GetFileProperties";

        #endregion
    }
}

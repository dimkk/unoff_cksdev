using CKS.Dev11.VisualStudio.SharePoint.Commands;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev11.VisualStudio.SharePoint.Environment.Options;
using CKS.Dev11.VisualStudio.SharePoint.Properties;
using EnvDTE;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Explorer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CKS.Dev11.VisualStudio.SharePoint.Explorer
{
    /// <summary>
    /// Represents an extension of SharePoint folder nodes in Server Explorer 
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(IExplorerNodeTypeProvider))]
    // Indicates that this class extends SharePoint file nodes in Server Explorer.
    [ExplorerNodeType(ExplorerNodeIds.FileNode)]
    internal class FileNodeTypeProvider : IExplorerNodeTypeProvider
    {
        #region Methods

        /// <summary>
        /// Initializes the new node type.
        /// </summary>
        /// <param name="typeDefinition">The definition of the new node type.</param>
        public virtual void InitializeType(IExplorerNodeTypeDefinition typeDefinition)
        {
            typeDefinition.DefaultIcon = Properties.Resources.icgen.ToBitmap();
            typeDefinition.IsAlwaysLeaf = true;

            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.FileProperties, true))
            {
                typeDefinition.NodePropertiesRequested += NodePropertiesRequested;
            }

            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.FileOperations, true))
            {
                typeDefinition.NodeMenuItemsRequested += NodeMenuItemsRequested;
            }
        }

        /// <summary>
        /// Nodes the properties requested.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExplorerNodePropertiesRequestedEventArgs" /> instance containing the event data.</param>
        protected virtual void NodePropertiesRequested(object sender, ExplorerNodePropertiesRequestedEventArgs e)
        {
            IExplorerNode fileNode = e.Node;
            FileNodeInfo file = fileNode.Annotations.GetValue<FileNodeInfo>();
            Dictionary<string, string> fileProperties = fileNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, Dictionary<string, string>>(FileSharePointCommandIds.GetFilePropertiesCommand, file);
            object propertySource = fileNode.Context.CreatePropertySourceObject(fileProperties);
            e.PropertySources.Add(propertySource);
        }

        /// <summary>
        /// Nodes the menu items requested.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExplorerNodeMenuItemsRequestedEventArgs" /> instance containing the event data.</param>
        protected virtual void NodeMenuItemsRequested(object sender, ExplorerNodeMenuItemsRequestedEventArgs e)
        {
            IExplorerNode fileNode = e.Node;
            FileNodeInfo file = fileNode.Annotations.GetValue<FileNodeInfo>();

            IMenuItem openFileMenuItem = e.MenuItems.Add(Resources.FileNodeTypeProvider_OpenFile);
            openFileMenuItem.Click += OpenFileMenuItemClick;

            IMenuItem checkOutFileMenuItem = e.MenuItems.Add(Resources.FileNodeTypeProvider_CheckOutFile);
            checkOutFileMenuItem.Click += CheckOutFileMenuItemClick;
            checkOutFileMenuItem.IsEnabled = file.IsCheckedOut == false;

            IMenuItem checkInFileMenuItem = e.MenuItems.Add(Resources.FileNodeTypeProvider_CheckInFile);
            checkInFileMenuItem.Click += CheckInFileMenuItemClick;
            checkInFileMenuItem.IsEnabled = file.IsCheckedOut == true;

            IMenuItem discardCheckOutFileMenuItem = e.MenuItems.Add(Resources.FileNodeTypeProvider_DiscardCheckOut);
            discardCheckOutFileMenuItem.Click += DiscardCheckOutFileMenuItemClick;
            discardCheckOutFileMenuItem.IsEnabled = file.IsCheckedOut == true;

            IMenuItem saveFileMenuItem = e.MenuItems.Add(Resources.FileNodeTypeProvider_SaveFile);
            saveFileMenuItem.Click += SaveFileMenuItemClick;
        }

        /// <summary>
        /// Opens the file menu item click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MenuItemEventArgs" /> instance containing the event data.</param>
        protected virtual void OpenFileMenuItemClick(object sender, MenuItemEventArgs e)
        {
            IExplorerNode fileNode = e.Owner as IExplorerNode;
            var fileNodeInfo = fileNode.Annotations.GetValue<FileNodeInfo>();

            DTEManager.SetStatus(Resources.FileUtilities_OpeningFile);

            string fileContents = fileNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, string>(FileSharePointCommandIds.GetFileContentsCommand, fileNodeInfo);
            DTEManager.CreateNewTextFile(fileNodeInfo.Name, fileContents);

            DTEManager.SetStatus(Resources.FileUtilities_FileSuccessfullyOpened);

        }

        /// <summary>
        /// Checks the out file menu item click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MenuItemEventArgs" /> instance containing the event data.</param>
        protected virtual void CheckOutFileMenuItemClick(object sender, MenuItemEventArgs e)
        {
            IExplorerNode parentNode = e.Owner as IExplorerNode;
            var fileNodeInfo = parentNode.Annotations.GetValue<FileNodeInfo>();

            DTEManager.SetStatus(Resources.FileUtilities_CheckingOutFile);

            bool result = parentNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, bool>(FileSharePointCommandIds.CheckOutFileCommand, fileNodeInfo);
            if (result)
            {
                parentNode.ParentNode.Refresh();
                DTEManager.SetStatus(Resources.FileUtilities_FileSuccessfullyCheckedOut);
            }
            else
            {
                MessageBox.Show(Resources.FileUtilities_FileCheckOutError, Resources.FileUtilities_FileCheckOutErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Checks the in file menu item click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MenuItemEventArgs" /> instance containing the event data.</param>
        protected virtual void CheckInFileMenuItemClick(object sender, MenuItemEventArgs e)
        {
            IExplorerNode parentNode = e.Owner as IExplorerNode;
            var fileNodeInfo = parentNode.Annotations.GetValue<FileNodeInfo>();

            DTEManager.SetStatus(Resources.FileUtilities_CheckingInFile);

            bool result = parentNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, bool>(FileSharePointCommandIds.CheckInFileCommand, fileNodeInfo);
            if (result)
            {
                parentNode.ParentNode.Refresh();
                DTEManager.SetStatus(Resources.FileUtilities_FileSuccessfullyCheckedIn);
            }
            else
            {
                MessageBox.Show(Resources.FileUtilities_FileCheckInError, Resources.FileUtilities_FileCheckInErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Discards the check out file menu item click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MenuItemEventArgs" /> instance containing the event data.</param>
        protected virtual void DiscardCheckOutFileMenuItemClick(object sender, MenuItemEventArgs e)
        {
            IExplorerNode parentNode = e.Owner as IExplorerNode;
            var fileNodeInfo = parentNode.Annotations.GetValue<FileNodeInfo>();

            DTEManager.SetStatus(Resources.FileUtilities_DiscardingFileCheckOut);

            bool result = parentNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, bool>(FileSharePointCommandIds.DiscardCheckOutCommand, fileNodeInfo);
            if (result)
            {
                parentNode.ParentNode.Refresh();
                DTEManager.SetStatus(Resources.FileUtilities_FileCheckOutSuccessfullyDiscarded);
            }
            else
            {
                MessageBox.Show(Resources.FileUtilities_DiscardFileCheckOutError, Resources.FileUtilities_DiscardFileCheckOutErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Saves the file menu item click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MenuItemEventArgs" /> instance containing the event data.</param>
        protected virtual void SaveFileMenuItemClick(object sender, MenuItemEventArgs e)
        {
            IExplorerNode parentNode = e.Owner as IExplorerNode;
            var fileNodeInfo = parentNode.Annotations.GetValue<FileNodeInfo>();

            DTEManager.SetStatus(Resources.FileUtilities_SavingFile);

            Document file = DTEManager.DTE.ActiveDocument;
            TextSelection selection = file.Selection;
            selection.SelectAll();
            fileNodeInfo.Contents = selection.Text;
            selection.StartOfDocument();

            bool result = parentNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, bool>(FileSharePointCommandIds.SaveFileCommand, fileNodeInfo);
            if (result)
            {
                DTEManager.SetStatus(Resources.FileUtilities_FileSuccessfullySaved);
            }
            else
            {
                MessageBox.Show(Resources.FileUtilities_FileSaveError, Resources.FileUtilities_FileSaveErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Creates the files nodes.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        public static void CreateFilesNodes(IExplorerNode parentNode)
        {
            DTEManager.SetStatus(Resources.FileNodeTypeProvider_RetrievingFolders);
            FolderNodeInfo[] folders = parentNode.Context.SharePointConnection.ExecuteCommand<FolderNodeInfo, FolderNodeInfo[]>(FileSharePointCommandIds.GetFoldersCommand, parentNode.Annotations.GetValue<FolderNodeInfo>());
            DTEManager.SetStatus(Resources.FileNodeTypeProvider_RetrievingFiles);
            FileNodeInfo[] files = parentNode.Context.SharePointConnection.ExecuteCommand<FolderNodeInfo, FileNodeInfo[]>(FileSharePointCommandIds.GetFilesCommand, parentNode.Annotations.GetValue<FolderNodeInfo>());

            if (folders != null)
            {
                foreach (FolderNodeInfo folder in folders)
                {
                    var annotations = new Dictionary<object, object>
                    {
                        { typeof(FolderNodeInfo), folder }
                    };

                    string nodeTypeId = ExplorerNodeIds.FolderNode;

                    IExplorerNode fileNode = parentNode.ChildNodes.Add(nodeTypeId, folder.Name, annotations);
                }
            }

            if (files != null)
            {
                foreach (FileNodeInfo file in files)
                {
                    var annotations = new Dictionary<object, object>
                    {
                        { typeof(FileNodeInfo), file }
                    };

                    string nodeTypeId = ExplorerNodeIds.FileNode;

                    IExplorerNode fileNode = parentNode.ChildNodes.Add(nodeTypeId, file.Name, annotations);
                    fileNode.DoubleClick += delegate(object sender, ExplorerNodeEventArgs e)
                    {
                        var fileNodeInfo = e.Node.Annotations.GetValue<FileNodeInfo>();

                        DTEManager.SetStatus(Resources.FileUtilities_OpeningFile);

                        string fileContents = fileNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, string>(FileSharePointCommandIds.GetFileContentsCommand, fileNodeInfo);
                        DTEManager.CreateNewTextFile(fileNodeInfo.Name, fileContents);

                        DTEManager.SetStatus(Resources.FileUtilities_FileSuccessfullyOpened);

                    };
                    SetExplorerNodeIcon(file, fileNode);
                }
            }

            DTEManager.SetStatus(String.Empty);
        }

        /// <summary>
        /// Sets the explorer node icon.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="fileNode">The file node.</param>
        private static void SetExplorerNodeIcon(FileNodeInfo file, IExplorerNode fileNode)
        {
            string extension = Path.GetExtension(file.Name.ToLower()).TrimStart('.');

            if (file.IsCheckedOut)
            {
                switch (extension)
                {
                    case "css":
                        fileNode.Icon = Resources.co_iccss.ToBitmap();
                        break;
                    case "gif":
                    case "jpg":
                    case "jpeg":
                        fileNode.Icon = Resources.co_icjpg.ToBitmap();
                        break;
                    case "js":
                        fileNode.Icon = Resources.co_icjs.ToBitmap();
                        break;
                    case "png":
                        fileNode.Icon = Resources.co_icpng.ToBitmap();
                        break;
                    case "xsl":
                        fileNode.Icon = Resources.co_icxsl.ToBitmap();
                        break;
                    case "xaml":
                        fileNode.Icon = Resources.co_icxaml.ToBitmap();
                        break;
                    default:
                        fileNode.Icon = Resources.co_icgen.ToBitmap();
                        break;
                }
            }
            else
            {
                switch (extension)
                {
                    case "css":
                        fileNode.Icon = Resources.iccss.ToBitmap();
                        break;
                    case "gif":
                    case "jpg":
                    case "jpeg":
                        fileNode.Icon = Resources.icjpg.ToBitmap();
                        break;
                    case "js":
                        fileNode.Icon = Resources.icjs.ToBitmap();
                        break;
                    case "png":
                        fileNode.Icon = Resources.icpng.ToBitmap();
                        break;
                    case "xsl":
                        fileNode.Icon = Resources.icxsl.ToBitmap();
                        break;
                    case "xaml":
                        fileNode.Icon = Resources.icxaml.ToBitmap();
                        break;
                    default:
                        fileNode.Icon = Resources.icgen.ToBitmap();
                        break;
                }
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;

using CKS.Dev.VisualStudio.SharePoint.Properties;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Explorer;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    [Export(typeof(IExplorerNodeTypeProvider))]
    [ExplorerNodeType(FileNodeTypeProvider.FileNodeTypeId)]
    internal class FileNodeTypeProvider : IExplorerNodeTypeProvider
    {
        internal const string FileNodeTypeId = "CKS.Dev.VisualStudio.SharePoint.Exploration.File";

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

        private void NodePropertiesRequested(object sender, ExplorerNodePropertiesRequestedEventArgs e)
        {
            IExplorerNode fileNode = e.Node;
            FileNodeInfo file = fileNode.Annotations.GetValue<FileNodeInfo>();
            Dictionary<string, string> fileProperties = fileNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, Dictionary<string, string>>(FileSharePointCommandIds.GetFilePropertiesCommand, file);
            object propertySource = fileNode.Context.CreatePropertySourceObject(fileProperties);
            e.PropertySources.Add(propertySource);
        }

        void NodeMenuItemsRequested(object sender, ExplorerNodeMenuItemsRequestedEventArgs e)
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

        protected virtual void OpenFileMenuItemClick(object sender, MenuItemEventArgs e)
        {
            IExplorerNode fileNode = e.Owner as IExplorerNode;
            var fileNodeInfo = fileNode.Annotations.GetValue<FileNodeInfo>();

            StatusBarLogger.Instance.SetStatus(Resources.FileUtilities_OpeningFile);

            string fileContents = fileNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, string>(FileSharePointCommandIds.GetFileContentsCommand, fileNodeInfo);
            DTEManager.CreateNewTextFile(fileNodeInfo.Name, fileContents);

            StatusBarLogger.Instance.SetStatus(Resources.FileUtilities_FileSuccessfullyOpened);
        
        }

        protected virtual void CheckOutFileMenuItemClick(object sender, MenuItemEventArgs e)
        {
            IExplorerNode parentNode = e.Owner as IExplorerNode;
            var fileNodeInfo = parentNode.Annotations.GetValue<FileNodeInfo>();

            StatusBarLogger.Instance.SetStatus(Resources.FileUtilities_CheckingOutFile);

            bool result = parentNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, bool>(FileSharePointCommandIds.CheckOutFileCommand, fileNodeInfo);
            if (result)
            {
                parentNode.ParentNode.Refresh();
                StatusBarLogger.Instance.SetStatus(Resources.FileUtilities_FileSuccessfullyCheckedOut);
            }
            else
            {
                MessageBox.Show(Resources.FileUtilities_FileCheckOutError, Resources.FileUtilities_FileCheckOutErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        protected virtual void CheckInFileMenuItemClick(object sender, MenuItemEventArgs e)
        {
            IExplorerNode parentNode = e.Owner as IExplorerNode;
            var fileNodeInfo = parentNode.Annotations.GetValue<FileNodeInfo>();

            StatusBarLogger.Instance.SetStatus(Resources.FileUtilities_CheckingInFile);

            bool result = parentNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, bool>(FileSharePointCommandIds.CheckInFileCommand, fileNodeInfo);
            if (result)
            {
                parentNode.ParentNode.Refresh();
                StatusBarLogger.Instance.SetStatus(Resources.FileUtilities_FileSuccessfullyCheckedIn);
            }
            else
            {
                MessageBox.Show(Resources.FileUtilities_FileCheckInError, Resources.FileUtilities_FileCheckInErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void DiscardCheckOutFileMenuItemClick(object sender, MenuItemEventArgs e)
        {
            IExplorerNode parentNode = e.Owner as IExplorerNode;
            var fileNodeInfo = parentNode.Annotations.GetValue<FileNodeInfo>();

            StatusBarLogger.Instance.SetStatus(Resources.FileUtilities_DiscardingFileCheckOut);

            bool result = parentNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, bool>(FileSharePointCommandIds.DiscardCheckOutCommand, fileNodeInfo);
            if (result)
            {
                parentNode.ParentNode.Refresh();
                StatusBarLogger.Instance.SetStatus(Resources.FileUtilities_FileCheckOutSuccessfullyDiscarded);
            }
            else
            {
                MessageBox.Show(Resources.FileUtilities_DiscardFileCheckOutError, Resources.FileUtilities_DiscardFileCheckOutErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void SaveFileMenuItemClick(object sender, MenuItemEventArgs e)
        {
            IExplorerNode parentNode = e.Owner as IExplorerNode;
            var fileNodeInfo = parentNode.Annotations.GetValue<FileNodeInfo>();

            StatusBarLogger.Instance.SetStatus(Resources.FileUtilities_SavingFile);

            Document file = DTEManager.DTE.ActiveDocument;
            TextSelection selection = file.Selection;
            selection.SelectAll();
            fileNodeInfo.Contents = selection.Text;
            selection.StartOfDocument();

            bool result = parentNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, bool>(FileSharePointCommandIds.SaveFileCommand, fileNodeInfo);
            if (result)
            {
                StatusBarLogger.Instance.SetStatus(Resources.FileUtilities_FileSuccessfullySaved);
            }
            else
            {
                MessageBox.Show(Resources.FileUtilities_FileSaveError, Resources.FileUtilities_FileSaveErrorMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public static void CreateFilesNodes(IExplorerNode parentNode)
        {
            StatusBarLogger.Instance.SetStatus(Resources.FileNodeTypeProvider_RetrievingFolders);
            FolderNodeInfo[] folders = parentNode.Context.SharePointConnection.ExecuteCommand<FolderNodeInfo, FolderNodeInfo[]>(FileSharePointCommandIds.GetFoldersCommand, parentNode.Annotations.GetValue<FolderNodeInfo>());
            StatusBarLogger.Instance.SetStatus(Resources.FileNodeTypeProvider_RetrievingFiles);
            FileNodeInfo[] files = parentNode.Context.SharePointConnection.ExecuteCommand<FolderNodeInfo, FileNodeInfo[]>(FileSharePointCommandIds.GetFilesCommand, parentNode.Annotations.GetValue<FolderNodeInfo>());

            if (folders != null)
            {
                foreach (FolderNodeInfo folder in folders)
                {
                    var annotations = new Dictionary<object, object>
                    {
                        { typeof(FolderNodeInfo), folder }
                    };

                    string nodeTypeId = FolderNodeTypeProvider.FolderNodeTypeId;

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

                    string nodeTypeId = FileNodeTypeProvider.FileNodeTypeId;

                    IExplorerNode fileNode = parentNode.ChildNodes.Add(nodeTypeId, file.Name, annotations);
                    fileNode.DoubleClick += delegate(object sender, ExplorerNodeEventArgs e)
                    {
                        var fileNodeInfo = e.Node.Annotations.GetValue<FileNodeInfo>();

                        StatusBarLogger.Instance.SetStatus(Resources.FileUtilities_OpeningFile);

                        string fileContents = fileNode.Context.SharePointConnection.ExecuteCommand<FileNodeInfo, string>(FileSharePointCommandIds.GetFileContentsCommand, fileNodeInfo);
                        DTEManager.CreateNewTextFile(fileNodeInfo.Name, fileContents);

                        StatusBarLogger.Instance.SetStatus(Resources.FileUtilities_FileSuccessfullyOpened);

                    };
                    SetExplorerNodeIcon(file, fileNode);
                }
            }

            StatusBarLogger.Instance.SetStatus(String.Empty);
        }

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
    }
}

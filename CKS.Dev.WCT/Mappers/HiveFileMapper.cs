using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Common;
using CKS.Dev.WCT.Resources;
using CKS.Dev.WCT.SolutionModel;
using CKS.Dev.WCT.ModelCreators;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.Mappers
{
    public class HiveFileMapper
    {
        #region Properties

        private IMappedFolder _tempateFolder = null;
        public IMappedFolder TemplateFolder
        {
            get
            {
                if (_tempateFolder == null)
                {
                    _tempateFolder = GetORCreateMappedFolder(MappedFolderType.Template);
                }
                return _tempateFolder;
            }
            set { _tempateFolder = value; }
        }


        private IMappedFolder _imagesFolder = null;
        public IMappedFolder ImagesFolder
        {
            get
            {
                if (_imagesFolder == null)
                {
                    _imagesFolder = GetORCreateMappedFolder(MappedFolderType.Images);
                }
                return _imagesFolder;
            }
            set { _imagesFolder = value; }
        }


        private IMappedFolder _controlTemplatesFolder = null;
        public IMappedFolder ControlTemplatesFolder
        {
            get
            {
                if (_controlTemplatesFolder == null)
                {
                    _controlTemplatesFolder = GetORCreateMappedFolder(MappedFolderType.ControlTemplates);
                }
                return _controlTemplatesFolder;
            }
            set { _controlTemplatesFolder = value; }
        }

        private IMappedFolder _layoutsFolder = null;
        public IMappedFolder LayoutsFolder
        {
            get
            {
                if (_layoutsFolder == null)
                {
                    _layoutsFolder = GetORCreateMappedFolder(MappedFolderType.Layouts);
                }
                return _layoutsFolder;
            }
            set { _layoutsFolder = value; }
        }


        private IMappedFolder _rootFolder = null;
        public IMappedFolder RootFolder
        {
            get
            {
                if (_rootFolder == null)
                {
                    _rootFolder = GetORCreateMappedFolder(MappedFolderType.SharePointRoot);
                }
                return _rootFolder;
            }
            set { _rootFolder = value; }
        }

        public WCTContext WCTContext { get; set; }

        #endregion

        public HiveFileMapper(WCTContext context)
        {
            this.WCTContext = context;
        }

        public void Map()
        {
            try
            {
                this.MapTemplateFiles();
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(StringResources.Strings_Errors_MapTemplateFiles, ex.ToString()));
            }

            try
            {
                this.MapRootFiles();
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(StringResources.Strings_Errors_MapRootFiles, ex.ToString()));
            }
        }



        private void MapTemplateFiles()
        {
            if (WCTContext.Solution.TemplateFileList.Count > 0)
            {
                foreach (TemplateFileReference template in WCTContext.Solution.TemplateFileList)
                {
                    string location = template.Location.Substring("Template/".Length);

                    if(location.StartsWithIgnoreCase("Images"))
                    {
                        location = location.Substring("Images/".Length);
                        CopyFile(template, this.ImagesFolder, location);
                        continue;
                    }

                    if (location.StartsWithIgnoreCase("ControlTemplates"))
                    {
                        location = location.Substring("ControlTemplates/".Length);
                        CopyFile(template, this.ControlTemplatesFolder, location);
                        continue;
                    }

                    if (location.StartsWithIgnoreCase("Layouts"))
                    {
                        location = location.Substring("Layouts/".Length);
                        CopyFile(template, this.LayoutsFolder, location);
                        continue;
                    }

                    if (location.StartsWithIgnoreCase("Features"))
                    {
                        continue;
                    }

                    CopyFile(template, this.TemplateFolder, location);
                }
            }
        }


        private void MapRootFiles()
        {
            if (this.WCTContext.Solution.RootFileList.Count > 0)
            {
                foreach (RootFileReference templateFile in WCTContext.Solution.RootFileList)
                {
                    string dest = Path.Combine(this.RootFolder.FullPath, templateFile.Location);
                    EnsureDirectory(dest);
                    File.Copy(templateFile.SourceFileInfo.FullName, dest);
                    ISharePointProjectItemFile spiFile = this.RootFolder.Files.AddFromFile(dest);
                    //spiFile.DeploymentType = DeploymentType.RootFile;
                }
            }
        }

        private void CopyFile(TemplateFileReference templateFile, IMappedFolder folder, string location)
        {
            string dest = Path.Combine(folder.FullPath, location);
            EnsureDirectory(dest);
            File.Copy(templateFile.SourceFileInfo.FullName, dest);
            ISharePointProjectItemFile spiFile = folder.Files.AddFromFile(dest);
            //spiFile.DeploymentType = DeploymentType.TemplateFile;
        }

        private void EnsureDirectory(string path)
        {
            string targetFolderPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }
        }

        private IMappedFolder GetORCreateMappedFolder(MappedFolderType folderType)
        {
            IMappedFolder result = WCTContext.SharePointProject.MappedFolders.OfType<IMappedFolder>().FirstOrDefault(p => p.FolderType == folderType);

            if(result == null)
            {
                result = WCTContext.SharePointProject.MappedFolders.Add(folderType);
            }

            return result;

        }


        //private void UpdateSolutionPaths(string oldTemplateFolder, string newTemplateFolder)
        //{
        //    oldTemplateFolder = oldTemplateFolder.Substring(this.WCTContext.TargetRootFolder.Length + 1);
        //    newTemplateFolder = newTemplateFolder.Substring(this.WCTContext.TargetRootFolder.Length + 1);

        //    if (!oldTemplateFolder.EndsWith(@"\"))
        //    {
        //        oldTemplateFolder += @"\";
        //    }

        //    if (!newTemplateFolder.EndsWith(@"\"))
        //    {
        //        newTemplateFolder += @"\";
        //    }

        //    for (int i = 0; i < this.WCTContext.SolutionToMap.AspxAscxFiles.Count; ++i)
        //    {
        //        if (this.WCTContext.SolutionToMap.AspxAscxFiles[i].ToLower().StartsWith(oldTemplateFolder.ToLower()))
        //        {
        //            this.WCTContext.SolutionToMap.AspxAscxFiles[i] = newTemplateFolder + this.WCTContext.SolutionToMap.AspxAscxFiles[i].Substring(oldTemplateFolder.Length);
        //        }
        //    }
        //}

        //private string CreateMappedFolder(bool isTemplate)
        //{
        //    MappedFolderCreator folderCreator = new MappedFolderCreator(this.WCTContext.DteProject);

        //    if (isTemplate)
        //    {
        //        folderCreator.FolderType = MappedFolderCreator.MappedFolderCreatorType.Template;
        //    }
        //    else
        //    {
        //        folderCreator.FolderType = MappedFolderCreator.MappedFolderCreatorType.Root;
        //    }

        //    return folderCreator.Create();
        //}

        //private void MoveAndReferenceFiles(string sourcePath, string targetPath)
        //{
        //    ProjectFileUpdater fileUpdater = new ProjectFileUpdater(this.WCTContext.DteProject);
        //    IList<string> files = fileUpdater.MoveFolderContentsLocation(
        //        sourcePath,
        //        targetPath);
        //    fileUpdater.AddProjectFiles(files);

        //    this.LogThemeTemplates(files, targetPath);
        //}

        //private void LogThemeTemplates(IList<string> filesToCheck, string templatePath)
        //{
        //    string themesFolder = @"THEMES\";
        //    string layoutsFolder = @"LAYOUTS\";

        //    if (!templatePath.EndsWith(@"\"))
        //    {
        //        templatePath += @"\";
        //    }

        //    foreach (string file in filesToCheck)
        //    {
        //        if (file.StartsWith(templatePath))
        //        {
        //            string fileToCheck = file.Substring(templatePath.Length);

        //            if (fileToCheck.ToLower().StartsWith(themesFolder.ToLower())
        //                || fileToCheck.ToLower().StartsWith(layoutsFolder.ToLower()))
        //            {
        //                Logger.LogInformation(StringResources.String_LogMessages_ThemeCodeUpdate);
        //                break;
        //            }
        //        }
        //    }
        //}

        //private void CorrectFieldControlClassReferences()
        //{
        //    foreach (TemplateFile file in this.WCTContext.SolutionToMap.Templates)
        //    {
        //        string filename = Path.GetFileName(file.ProjectRelativePath);

        //        if (filename.StartsWith("fldTypes", StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            string fullPath = Path.Combine(this.WCTContext.TargetRootFolder, file.ProjectRelativePath);
        //            try
        //            {
        //                FieldTypeUpdater updater = new FieldTypeUpdater(fullPath);

        //                IList<Guid> classes = updater.GetTypeGuids();
        //                foreach (Guid classGuid in classes)
        //                {
        //                    if (this.WCTContext.SolutionToMap.GuidMarkedClasses.ContainsKey(classGuid))
        //                    {
        //                        string name = this.GetFullClassName(this.WCTContext.SolutionToMap.GuidMarkedClasses[classGuid].FullClassNameWithoutAssembly);
        //                        string tokenizedName = name + ", " + Constants.TargetAssemblyFullnameToken;

        //                        updater.UpdateTypeGuid(classGuid, tokenizedName);
        //                    }
        //                }

        //                updater.SaveChanges();
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.LogError(String.Format(StringResources.Strings_Errors_SetFieldControlClassReference, fullPath, ex.ToString()));
        //            }
        //        }
        //    }
        //}

        private string GetFullClassName(string fullClassName)
        {
            if (!this.WCTContext.SourceProject.IsCSharp
                && !string.IsNullOrEmpty(this.WCTContext.SourceProject.RootNamespace))
            {
                fullClassName = String.Format("{0}.{1}", this.WCTContext.SourceProject.RootNamespace, fullClassName);
            }

            return fullClassName;
        }
    }
}
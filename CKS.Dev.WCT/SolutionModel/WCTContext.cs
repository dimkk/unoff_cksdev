using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.SharePoint;
using EnvDTE;
using CKS.Dev.WCT.SolutionModel;
using CKS.Dev.WCT.Mappers;

using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Common;

namespace CKS.Dev.WCT.SolutionModel
{
    public class WCTContext
    {
        #region Target project

        private EnvDTE.Project _dteProject = null;
        public EnvDTE.Project DteProject
        {
            get { return _dteProject; }
            set { _dteProject = value; }
        }

        private System.IServiceProvider _serviceProvider;
        private System.IServiceProvider ServiceProvider
        {
            get
            {
                if (this._serviceProvider == null)
                {
                    Microsoft.VisualStudio.OLE.Interop.IServiceProvider sp = null;
                    sp = this._dteProject.DTE as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
                    if (sp != null)
                    {
                        this._serviceProvider = new Microsoft.VisualStudio.Shell.ServiceProvider(sp);
                    }
                }
                return this._serviceProvider;

            }
        }

        private ISharePointProjectService _projectService;
        public ISharePointProjectService ProjectService
        {
            get
            {
                if (_projectService == null)
                {
                    _projectService = this.ServiceProvider.GetService(typeof(ISharePointProjectService)) as ISharePointProjectService;
                }
                return _projectService;
            }
        }


        private ISharePointProject _sharePointProject = null;
        public ISharePointProject SharePointProject
        {
          get 
          { 
              if(_sharePointProject == null)
              {
                  _sharePointProject = this.ProjectService.Convert<Project, ISharePointProject>(this.DteProject);
              }
              return _sharePointProject; 
          }
          set { _sharePointProject = value; }
        }


        #endregion


        //private SolutionModel.Solution _solutionToMap = null;
        //public SolutionModel.Solution SolutionToMap
        //{
        //    get { return _solutionToMap; }
        //    set { _solutionToMap = value; }
        //}


        private SolutionDefinition _solution = null;
        public SolutionDefinition Solution
        {
            get { return _solution; }
            set { _solution = value; }
        }



        private string _targetProjectFolder = null;
        public string TargetProjectFolder
        {
            get
            {
                if (_targetProjectFolder == null)
                {
                    _targetProjectFolder = Path.GetDirectoryName(this.DteProject.FileName);
                }
                return _targetProjectFolder;
            }

        }


        public string TargetProjectFilePath
        {
            get
            {
                if (this._dteProject != null)
                {
                    return this.DteProject.FileName;
                }

                return string.Empty;
            }
        }

        //public bool IsSandboxedSolution { get; set; }

        //public bool IsCSharp { get; set; }

        //public string SourceProjectFilePath { get; set; }


        //private IDictionary<Guid, ClassInformation> _classMap;
        //public IDictionary<Guid, ClassInformation> ClassMap
        //{
        //    get { return _classMap; }
        //    set { _classMap = value; }
        //}


        private string _sharePointRoot = null;
        public string SharePointRootName
        {
            get
            {
                if (_sharePointRoot == null)
                {
                    if (!String.IsNullOrEmpty(this.SourceSharePointRootPath))
                    {
                        DirectoryInfo dir = new DirectoryInfo(this.SourceSharePointRootPath);
                        _sharePointRoot = dir.Name;
                    }
                }
                return _sharePointRoot;
            }
        }



        #region Source Project

        private VSProject _sourceProject = null;
        public VSProject SourceProject
        {
            get
            {
                return _sourceProject;
            }
            set
            {
                _sourceProject = value;
            }

        }

        public string SourceProjectPath
        {
            get
            {
                return Path.GetDirectoryName(this.SourceProject.FileName);
            }
        }

        private string _sourceSharePointRootPath = null;
        public string SourceSharePointRootPath
        {
            get
            {
                if (_sourceSharePointRootPath == null)
                {
                    _sourceSharePointRootPath = Path.Combine(this.SourceProjectPath, "12");
                    if (!Directory.Exists(_sourceSharePointRootPath))
                    {
                        _sourceSharePointRootPath = Path.Combine(this.SourceProjectPath, "14");
                        if (!Directory.Exists(_sourceSharePointRootPath))
                        {
                            _sourceSharePointRootPath = Path.Combine(this.SourceProjectPath, "SharePointRoot");
                            if (!Directory.Exists(_sourceSharePointRootPath))
                            {
                                _sourceSharePointRootPath = string.Empty;
                                Logger.LogError(String.Format("Cannot find a SharePointRoot folder (the 12 or 14 hive)"));
                            }
                        }
                    }

                }
                return _sourceSharePointRootPath;
            }
        }
       


        public bool SourceSharePointRootExist
        {
            get
            {
                return !String.IsNullOrEmpty(this.SourceSharePointRootPath);
            }
        }



        private ICollection<ProjectReference> _projectReferences = new List<ProjectReference>();
        public ICollection<ProjectReference> ProjectReferences
        {
            get
            {
                return this._projectReferences;
            }
            set
            {
                this._projectReferences = value;
            }
        }


        private Dictionary<string, int> _spiNames = null;
        public Dictionary<string, int> SPINames
        {
            get { return _spiNames; }
            set { _spiNames = value; }
        }


        private List<string> _excludedFileExtensions = new List<string>();
        public List<string> ExcludedFileExtensions
        {
            get { return _excludedFileExtensions; }
            set { _excludedFileExtensions = value; }
        }

        private List<string> _excludedFolders = new List<string>();
        public List<string> ExcludedFolders
        {
            get { return _excludedFolders; }
            set { _excludedFolders = value; }
        }


        #endregion


        public WCTContext()
        {
            this.SPINames = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        }

        public string GetSharePointProjectLocalPath(string fullpath)
        {
            string result = fullpath;

            if (!String.IsNullOrEmpty(fullpath) && this.SharePointProject != null)
            {
                result = result.ReplaceIgnoreCase(this.TargetProjectFolder, string.Empty);
                if (result.StartsWith("/") || result.StartsWith(@"\"))
                {
                    result = result.Substring(1);
                }
            }

            return result;
        }

    }
}

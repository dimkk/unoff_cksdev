using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Common;
using CKS.Dev.WCT.Resources;
using CKS.Dev.WCT.ModelCreators;
using CKS.Dev.WCT.SolutionModel;
using CKS.Dev.WCT.Mappers;


namespace CKS.Dev.WCT
{

    public class ProjectHandler
    {
        public WCTContext WCTContext { get; set; }

        public ProjectHandler(WCTContext context)
        {
            this.WCTContext = context;
            Logger.DesignTimeEnvironment = this.WCTContext.DteProject.DTE;
        }

        public void Update()
        {
            if (".csproj".EqualsIgnoreCase(Path.GetExtension(this.WCTContext.SourceProject.FileName))
                || ".vbproj".EqualsIgnoreCase(Path.GetExtension(this.WCTContext.SourceProject.FileName)))
            {
                this.UpdateProject();

                this.UpdateAssemblyInfoFile();
            }
        }

        private void UpdateProject()
        {
            Logger.LogStatus(StringResources.String_LogMessages_BuildingSourceModel);

            SolutionCreator creator = new SolutionCreator(this.WCTContext);
            this.WCTContext.Solution = creator.LoadSolution();

            Logger.LogStatus(String.Format(StringResources.String_LogMessages_MappingTarget, this.WCTContext.TargetProjectFilePath));

            VisualStudioProjectMapper vsMapper = new VisualStudioProjectMapper(this.WCTContext);
            vsMapper.Map();

            SolutionMapper spMapper = new SolutionMapper(this.WCTContext);
            spMapper.Map();

            // Add all the project files that have not been added yet.
            vsMapper.MapProjectFiles();

            PackageMapper packageMapper = new PackageMapper(this.WCTContext);
            packageMapper.Map();
        }

        public void UpdateAssemblyInfoFile()
        {
            const string strAssemblyInfoFile = "Properties\\AssemblyInfo.cs";
            string strLineToAdd = "using System.Security;" + Environment.NewLine;

            string path = Path.Combine(Path.GetDirectoryName(this.WCTContext.TargetProjectFilePath), strAssemblyInfoFile);

            string text = File.ReadAllText(path);

            if (text.IndexOf("System.Security;") < 0)
            {
                int index = text.IndexOf("using System");
                index = (index < 0) ? 0 : index;

                text.Insert(index, strLineToAdd);
                File.WriteAllText(path, text);
            }
        }
    }
}
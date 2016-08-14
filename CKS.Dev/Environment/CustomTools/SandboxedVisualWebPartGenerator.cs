using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using VSLangProj80;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.SharePoint;
using System.IO;
using System.Web.Compilation;
using Microsoft.VisualStudio.Web.Interop;
using System.Reflection;
using System.Text;
using VSLangProj;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev.VisualStudio.SharePoint.Environment;

namespace CKS.Dev.VisualStudio.SharePoint.Environment.CustomTools
{
    /// <summary>
    /// The Sandboxed Visual Web Part class generator.
    /// </summary>
    [CodeGeneratorRegistration(typeof(SandboxedVisualWebPartGenerator),
         "SandboxedVisualWebPartGenerator", vsContextGuids.vsContextGuidVCSProject)]
    [ProvideObject(typeof(SandboxedVisualWebPartGenerator))]
    [Guid(VSPackageGuids.SandboxedVisualWebPartGeneratorText)]
    public class SandboxedVisualWebPartGenerator
         : SitedObject, IObjectWithSite, IVsSingleFileGenerator
    {
        /// <summary>
        /// Defaults the extension.
        /// </summary>
        /// <param name="pbstrDefaultExtension">The PBSTR default extension.</param>
        /// <returns></returns>
        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            pbstrDefaultExtension = ".ascx.designer.cs";
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Generates the specified WSZ input file path.
        /// </summary>
        /// <param name="wszInputFilePath">The WSZ input file path.</param>
        /// <param name="bstrInputFileContents">The BSTR input file contents.</param>
        /// <param name="wszDefaultNamespace">The WSZ default namespace.</param>
        /// <param name="rgbOutputFileContents">The RGB output file contents.</param>
        /// <param name="pcbOutput">The PCB output.</param>
        /// <param name="pGenerateProgress">The p generate progress.</param>
        /// <returns></returns>
        public int Generate(string wszInputFilePath, string bstrInputFileContents,
            string wszDefaultNamespace, IntPtr[] rgbOutputFileContents,
            out uint pcbOutput, IVsGeneratorProgress pGenerateProgress)
        {
            Cursor current = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                ProjectItem projectItem = (ProjectItem)ServiceProvider.GetService(typeof(ProjectItem)); ;
                ProjectManager projectManager = ProjectManager.Create(projectItem.ContainingProject);


                string configuration = projectManager.DteProject.ConfigurationManager.ActiveConfiguration.ConfigurationName;
                string folder = Path.Combine(
                    Path.GetDirectoryName(projectManager.Project.FullPath),
                    "obj",
                    configuration,
                    "TempControl",
                    projectItem.Name);
                string inFolder = Path.Combine(folder, "In");
                string outFolder = Path.Combine(folder, "Out");
                string inBinFolder = Path.Combine(inFolder, "bin");
                if (Directory.Exists(inFolder))
                {
                    Directory.Delete(inFolder, true);
                }
                if (Directory.Exists(inBinFolder))
                {
                    Directory.Delete(inBinFolder, true);
                }
                if (Directory.Exists(outFolder))
                {
                    Directory.Delete(outFolder, true);
                }
                Directory.CreateDirectory(inFolder);
                Directory.CreateDirectory(inBinFolder);
                Directory.CreateDirectory(outFolder);
                Directory.SetCurrentDirectory(folder);
                string file = Path.Combine(inFolder, projectItem.Name);
                File.WriteAllText(file, bstrInputFileContents);
                string f = wszInputFilePath + ".cs";
                if (File.Exists(f))
                {
                    File.Copy(f, Path.Combine(inFolder, Path.GetFileName(f)));
                }
                VSProject vsLangProject = (VSProject)projectManager.DteProject.Object;
                foreach (Reference reference in vsLangProject.References)
                {
                    if (reference.CopyLocal)
                    {
                        if (reference.SourceProject != null)
                        {
                            Solution solution = DTEManager.ActiveSolution;
                            SolutionBuild build = solution.SolutionBuild;

                            build.BuildProject(
                                solution.SolutionBuild.ActiveConfiguration.Name,
                                reference.SourceProject.UniqueName,
                                true);
                        }
                        string dllFileName = Path.GetFileName(reference.Path);
                        if (File.Exists(reference.Path))
                        {
                            File.Copy(reference.Path, Path.Combine(inBinFolder, dllFileName));
                        }
                        else
                        {
                            throw new ApplicationException("Required reference not available");
                        }
                    }
                }
                string output = DTEManager.ProjectService.SharePointConnection.ExecuteCommand<CompilationInfo, string>(
                    CustomToolSharePointCommandIds.ParseUserControl,
                    new CompilationInfo
                    {
                        InFolder = inFolder,
                        OutFolder = outFolder
                    });
                if (String.IsNullOrEmpty(output) == false)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(output);
                    int length = bytes.Length;
                    IntPtr mem = Marshal.AllocCoTaskMem(length);
                    Marshal.Copy(bytes, 0, mem, length);
                    rgbOutputFileContents[0] = mem;
                    pcbOutput = (uint)length;
                }
                else
                {
                    pcbOutput = 0;
                }
                return VSConstants.S_OK;
            }
            finally
            {
                Cursor.Current = current;
            }
        }
    }
}

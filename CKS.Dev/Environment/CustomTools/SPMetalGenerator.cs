using System;
using System.Linq;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using Process = System.Diagnostics.Process;
using System.CodeDom.Compiler;
using VSLangProj80;
using Microsoft.VisualStudio.Designer.Interfaces;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.SharePoint;
using System.IO;
using System.Windows.Forms;
using CKS.Dev.VisualStudio.SharePoint.Exploration;
using CKS.Dev.VisualStudio.SharePoint.Content;
using CKS.Dev.VisualStudio.SharePoint.Environment;

namespace CKS.Dev.VisualStudio.SharePoint.Environment.CustomTools
{
    /// <summary>
    /// SPMetal Helper.
    /// </summary>
    [CodeGeneratorRegistration(typeof(SPMetalGenerator), "SPMetalGenerator", vsContextGuids.vsContextGuidVCSProject)]
    [ProvideObject(typeof(SPMetalGenerator))]
    [Guid(VSPackageGuids.SPMetalGeneratorText)]
    public class SPMetalGenerator : SitedObject, IVsSingleFileGenerator
    {
        /// <summary>
        /// Defaults the extension.
        /// </summary>
        /// <param name="defaultExtension">The default extension.</param>
        /// <returns></returns>
        int IVsSingleFileGenerator.DefaultExtension(out string defaultExtension)
        {
            defaultExtension = null;
            CodeDomProvider provider = GetCodeDomProvider();
            defaultExtension = ".Designer." + provider.FileExtension;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Generates the specified input file path.
        /// </summary>
        /// <param name="inputFilePath">The input file path.</param>
        /// <param name="inputFileContents">The input file contents.</param>
        /// <param name="defaultNamespace">The default namespace.</param>
        /// <param name="outputFileContents">The output file contents.</param>
        /// <param name="output">The output.</param>
        /// <param name="generateProgress">The generate progress.</param>
        /// <returns></returns>
        int IVsSingleFileGenerator.Generate(string inputFilePath, string inputFileContents,
            string defaultNamespace, IntPtr[] outputFileContents,
            out uint output, IVsGeneratorProgress generateProgress)
        {
            Cursor current = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                ISharePointProjectService sharePointService = DTEManager.ProjectService;

                ProjectItem projectItem = (ProjectItem)ServiceProvider.GetService(typeof(ProjectItem));
                EnvDTE.Project project = projectItem.ContainingProject;
                ProjectManager projectManager = ProjectManager.Create(project);

                FileInfo fi = new FileInfo(inputFilePath);

                //Default the site url to that of the project url

                string siteUrl = projectManager.Project.SiteUrl.ToString();
                string serialisationOption = SPMetalDefinitionSerialization.None.ToString().ToLower();

                foreach (ISharePointProjectItem item in projectManager.GetItemsOfType(ProjectItemIds.SPMetalDefinition))
                {
                    //Is this the same SPI as the one the tool is operating on
                    if (item.DefaultFile.Name == fi.Name)
                    {
                        SPMetalDefinitionProperties properties = item.Annotations.GetValue<SPMetalDefinitionProperties>();
                        if (properties != null)
                        {
                            siteUrl = properties.Url;
                            serialisationOption = properties.Serialization.ToString().ToLower();
                        }
                        break;
                    }
                }

                CodeDomProvider codeDomProvider = GetCodeDomProvider();
                string language = String.Equals(codeDomProvider.FileExtension, "cs", StringComparison.InvariantCultureIgnoreCase) ?
                    "CSharp" : "VB";

                string spmetalPath = Path.Combine(sharePointService.SharePointInstallPath, @"bin\spmetal.exe");
                string tempFile = Path.GetTempFileName();
                
                Dictionary<string, string> arguments = new Dictionary<string, string>();
                arguments.Add("web", siteUrl);
                arguments.Add("code", tempFile);
                arguments.Add("language", language);
                arguments.Add("namespace", defaultNamespace);
                arguments.Add("serialization", serialisationOption);
                arguments.Add("parameters", inputFilePath);
                
                ProcessStartInfo startInfo = new ProcessStartInfo(spmetalPath);
                startInfo.Arguments = String.Concat(arguments.Select(
                    kvp => String.Format(@" /{0}:""{1}""", kvp.Key, kvp.Value)));
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;

                Process process = Process.Start(startInfo);
                string debug = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                if (File.Exists(tempFile))
                {
                    try
                    {
                        string outputString = File.ReadAllText(tempFile);
                        byte[] content = Encoding.Default.GetBytes(outputString);
                        int length = content.Length;
                        IntPtr mem = Marshal.AllocCoTaskMem(length);
                        Marshal.Copy(content, 0, mem, length);
                        outputFileContents[0] = mem;
                        output = (uint)length;
                    }
                    finally
                    {
                        File.Delete(tempFile);
                    }
                    return VSConstants.S_OK;
                }
                else
                {
                    output = 0;
                    return VSConstants.E_FAIL;
                }
            }
            finally
            {
                Cursor.Current = current;
            }
        }

        /// <summary>
        /// Gets the code DOM provider.
        /// </summary>
        /// <returns></returns>
        protected CodeDomProvider GetCodeDomProvider()
        {
            IVSMDCodeDomProvider provider = (IVSMDCodeDomProvider)GetService(typeof(SVSMDCodeDomProvider));
            return (CodeDomProvider)provider.CodeDomProvider;
        }
    }
}

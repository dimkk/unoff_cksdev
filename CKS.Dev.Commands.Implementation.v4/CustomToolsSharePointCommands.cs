using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Web.Compilation;
using Microsoft.VisualStudio.SharePoint.Commands;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev11.VisualStudio.SharePoint.Commands;

namespace CKS.Dev11.VisualStudio.SharePoint.Commands
{
    /// <summary>
    /// The custom tools commands.
    /// </summary>
    class CustomToolsSharePointCommands
    {
        #region Methods

        /// <summary>
        /// Parses the user control.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="compilationInfo">The compilation info.</param>
        /// <returns>The parsed control.</returns>
        [SharePointCommand(CustomToolSharePointCommandIds.ParseUserControl)]
        public static string ParseUserControl(ISharePointCommandContext context,
            CompilationInfo compilationInfo)
        {
            ClientBuildManagerParameter bmp = new ClientBuildManagerParameter();
            bmp.PrecompilationFlags = PrecompilationFlags.Clean | PrecompilationFlags.FixedNames | PrecompilationFlags.OverwriteTarget | PrecompilationFlags.ForceDebug; ;

            using (ClientBuildManager bm = new ClientBuildManager("/", compilationInfo.InFolder, compilationInfo.OutFolder, bmp))
            {
                bm.PrecompileApplication();
                string sourceFolder = bm.CodeGenDir;
                string compilationResultFile = Directory.GetFiles(sourceFolder, "*.compiled").First();
                XDocument compilationResult = XDocument.Load(compilationResultFile);
                string generatedTypeName = compilationResult.Root.Attribute("type").Value;
                string generatedClassName = generatedTypeName.Split('.').Last();
                foreach (string generatedSourceFile in Directory.GetFiles(sourceFolder, "*.cs"))
                {
                    string contents = File.ReadAllText(generatedSourceFile);
                    if (contents.Contains(String.Format("public class {0}", generatedClassName)))
                    {
                        return contents;
                    }
                }
            }
            throw new ApplicationException("Unknown");
        }

        #endregion
    }
}

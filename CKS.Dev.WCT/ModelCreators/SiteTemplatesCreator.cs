using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.Extensions;
using CKS.Dev.WCT.SolutionModel;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.ModelCreators
{
    public class SiteTemplatesCreator
    {
        public WCTContext WCTContext { get; set; }
        public DirectoryInfo TemplatesDir { get; set; }
        public SolutionDefinition Solution { get; set; }

        public Dictionary<string, List<ProjectFile>> fileLookup = new Dictionary<string, List<ProjectFile>>(StringComparer.OrdinalIgnoreCase);


        public SiteTemplatesCreator(WCTContext wctContext, SolutionDefinition solution, DirectoryInfo dir)
        {
            this.WCTContext = wctContext;
            this.Solution = solution;
            this.TemplatesDir = dir;
            SetupFileLookup();
        }

        private void SetupFileLookup()
        {
            foreach (var entry in this.WCTContext.SourceProject.Files)
            {
                ProjectFile file = entry.Value;
                if (file.Info.Name.StartsWithIgnoreCase("webtemp") && file.Info.Name.EndsWithIgnoreCase(".xml") && file.Info.Directory.Name.EqualsIgnoreCase("xml"))
                {
                    List<ProjectFile> fileRefs = null;
                    if (fileLookup.ContainsKey(file.Info.Name))
                    {
                        fileRefs = fileLookup[file.Info.Name];
                    }
                    else
                    {
                        fileRefs = new List<ProjectFile>();
                        fileLookup.AddOrReplace(file.Info.Name, fileRefs);
                    }

                    fileRefs.Add(file);
                }
            }
        }

        public void AddTemplates()
        {

            foreach (DirectoryInfo templateDir in this.TemplatesDir.GetDirectories(this.WCTContext))
            {
                this.Solution.SiteDefinitionManifests = this.Solution.SiteDefinitionManifests.Add(CreateTemplate(templateDir));
            }

        }

  //<SiteDefinitionManifests>
  //  <SiteDefinitionManifest Location="BlankSiteDefinition1">
  //    <WebTempFile Location="1033\xml\webtemp_BlankSiteDefinition1.xml" />
  //  </SiteDefinitionManifest>
  //</SiteDefinitionManifests>

        private SiteDefinitionManifestFileReference CreateTemplate(DirectoryInfo templateDir)
        {
            SiteDefinitionManifestFileReference siteRef = new SiteDefinitionManifestFileReference();
            siteRef.SourceDirectory = templateDir;
            siteRef.Location = templateDir.Name;

            siteRef.VSItem = new VSSiteDinitionItem(siteRef);

            // Include all files in the SiteDefinition template folder.
            IEnumerable<FileInfo> siteFiles = templateDir.GetFiles(this.WCTContext, "*", SearchOption.AllDirectories);
            foreach (FileInfo info in siteFiles)
            {
                siteRef.VSItem.AddProjectFile(this.WCTContext, info.FullName, info.GetLocalName(templateDir.FullName), DeploymentType.TemplateFile);
            }

            // Get the webtemp files.
            string webTempFileName = string.Format("webtemp_{0}.xml", templateDir.Name);
            string webTempFileName2 = string.Format("webtemp{0}.xml", templateDir.Name);

            List<ProjectFile> webtemplist = null;

            if (fileLookup.ContainsKey(webTempFileName))
            {
                webtemplist = fileLookup[webTempFileName];
            }

            if (fileLookup.ContainsKey(webTempFileName2))
            {
                webtemplist = fileLookup[webTempFileName2];
            }

            if(webtemplist != null)
            {
                //List<ProjectFile> list = fileLookup[webTempFileName];
                foreach (var file in webtemplist)
                {
                    WebTempFileDefinition fileDef = new WebTempFileDefinition();
                    string path = String.Format("{0}\\{1}\\{2}", file.Info.Directory.Parent.Name, file.Info.Directory.Name, file.Info.Name);
                    fileDef.Location = path;
                    fileDef.SourceFileInfo = file.Info;
                    siteRef.WebTempFile.Add(fileDef);

                    siteRef.VSItem.AddProjectFile(file);
                }
            }




            return siteRef;
        }
    }
}

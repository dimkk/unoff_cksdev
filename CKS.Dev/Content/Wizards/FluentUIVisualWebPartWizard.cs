using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System.IO;
using System.Xml.Linq;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    /// <summary>
    /// Wizard for the Fluent UI Visual web part SPI.
    /// </summary>
    class FluentUIVisualWebPartWizard : BaseWizard
    {
        #region Fields

        /// <summary>
        /// Field to hold the webpart RESX name.
        /// </summary>
        private string webPartResxFileName;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a flag indicating whether a SharePoint connection is required
        /// </summary>
        /// <value></value>
        protected override bool IsSharePointConnectionRequired
        {
            get { return true; }
        }

        /// <summary>
        /// Should add project item
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>Returns true</returns>
        public override bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the project properties
        /// </summary>
        /// <param name="project">The project</param>
        public override void SetProjectProperties(Project project)
        {
            //Do nothing
        }

        /// <summary>
        /// Create the wizard form
        /// </summary>
        /// <param name="designTimeEnvironment">The design time environment</param>
        /// <param name="runKind">The wizard run kind</param>
        /// <returns>The IWizardFormExtension</returns>
        public override IWizardFormExtension CreateWizardForm(DTE designTimeEnvironment, WizardRunKind runKind)
        {
            //We have no need for a UI so return null
            return null;
        }

        /// <summary>
        /// Run project item finished generating
        /// </summary>
        /// <param name="projectItem">The project item</param>
        public override void RunProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            base.RunProjectItemFinishedGenerating(projectItem);

            if (projectItem.Name.EndsWith("WebPart.resx"))
            {
                webPartResxFileName = projectItem.Name;
            }
        }

        /// <summary>
        /// Run wizard finsihed
        /// </summary>
        public override void RunWizardFinished()
        {
            base.RunWizardFinished();

            AlterOrCreateLayoutsSPData();
            AlterOrCreateAppGlobalResourcesSPData();
        }

        /// <summary>
        /// Intialise the form wizard data
        /// </summary>
        /// <param name="replacementsDictionary">The replacements dictionary</param>
        public override void InitializeFromWizardData(Dictionary<string, string> replacementsDictionary)
        {
            base.InitializeFromWizardData(replacementsDictionary);

            if (replacementsDictionary != null)
            {
                if (replacementsDictionary.ContainsKey("$rootname$"))
                {
                    replacementsDictionary.Add("$subnamespace$", WizardHelpers.MakeNameCompliant(replacementsDictionary["$rootname$"]));
                }
            }

        }

        /// <summary>
        /// Alters the or create layouts SP data.
        /// </summary>
        private void AlterOrCreateLayoutsSPData()
        {
            EnvDTE.ProjectItem layoutsfolderProjectItem = DTEManager.FindItemByName(DTEManager.ActiveProject.ProjectItems, "Layouts", true);
            if (layoutsfolderProjectItem != null)
            {
                if (DTEManager.FindItemByName(layoutsfolderProjectItem.ProjectItems, "SharePointProjectItem.spdata", true) == null)
                {
                    //Create the encoding definition
                    XDeclaration declaration = new XDeclaration("1.0", Encoding.UTF8.WebName, null);

                    XNamespace sharepointToolsNamespace = @"http://schemas.microsoft.com/VisualStudio/2010/SharePointTools/SharePointProjectItemModel";
                    XNamespace sharepointNamespace = @"http://schemas.microsoft.com/sharepoint/";

                    string correctedDoc = CreateLayoutsSPData(declaration, sharepointToolsNamespace);

                    string tempSPDataFileName = Path.GetTempFileName();
                    using (StreamWriter writer2 = new StreamWriter(tempSPDataFileName, false))
                    {
                        writer2.WriteLine(correctedDoc);
                    }
                    ProjectItem spDataProjectItem = layoutsfolderProjectItem.ProjectItems.AddFromTemplate(tempSPDataFileName, "SharePointProjectItem.spdata");

                    File.Delete(tempSPDataFileName);
                }
            }
        }

        /// <summary>
        /// Creates the layouts SP data.
        /// </summary>
        /// <param name="declaration">The declaration.</param>
        /// <param name="sharepointToolsNamespace">The sharepoint tools namespace.</param>
        /// <returns>A sting representation of the layouts spdata file.</returns>
        private string CreateLayoutsSPData(XDeclaration declaration,
            XNamespace sharepointToolsNamespace)
        {
            #region spdata

            XElement projectItemFolder = new XElement("ProjectItemFolder",
                new XAttribute("Target", "Layouts"),
                new XAttribute("Type", "TemplateFile"));

            XElement projectItem = new XElement(sharepointToolsNamespace + "ProjectItem",
                new XAttribute("Type", "Microsoft.VisualStudio.SharePoint.MappedFolder"),
                new XAttribute("SupportedTrustLevels", "FullTrust"),
                new XAttribute("SupportedDeploymentScopes", "Package"),
                projectItemFolder);

            XDocument firstDoc = new XDocument(declaration, projectItem);

            string correctedDoc = firstDoc.ToString().Replace(@" xmlns=""""", "");

            correctedDoc = firstDoc.Declaration.ToString() + correctedDoc;

            #endregion

            return correctedDoc;
        }

        /// <summary>
        /// Alters the or create app global resources SP data.
        /// </summary>
        private void AlterOrCreateAppGlobalResourcesSPData()
        {
            EnvDTE.ProjectItem appGlobalResourcesFolderProjectItem = DTEManager.FindItemByName(DTEManager.ActiveProject.ProjectItems, "AppGlobalResources", true);
            if (appGlobalResourcesFolderProjectItem != null)
            {
                XNamespace sharepointToolsNamespace = @"http://schemas.microsoft.com/VisualStudio/2010/SharePointTools/SharePointProjectItemModel";
                XNamespace sharepointNamespace = @"http://schemas.microsoft.com/sharepoint/";

                //Create the encoding definition
                XDeclaration declaration = new XDeclaration("1.0", Encoding.UTF8.WebName, null);

                EnvDTE.ProjectItem appGlobalResourcesSPData = DTEManager.FindItemByName(appGlobalResourcesFolderProjectItem.ProjectItems, "SharePointProjectItem.spdata", true);
                if (appGlobalResourcesSPData == null)
                {

                    string correctedDoc = CreateAppGlobalResourcesSPData(declaration, sharepointToolsNamespace, webPartResxFileName);

                    string tempSPDataFileName = Path.GetTempFileName();
                    using (StreamWriter writer2 = new StreamWriter(tempSPDataFileName, false))
                    {
                        writer2.WriteLine(correctedDoc);
                    }
                    ProjectItem spDataProjectItem = appGlobalResourcesFolderProjectItem.ProjectItems.AddFromTemplate(tempSPDataFileName, "SharePointProjectItem.spdata");

                    File.Delete(tempSPDataFileName);
                }
                else
                {
                    string filename = appGlobalResourcesSPData.FileNames[0];

                    XElement c = XElement.Load(filename, LoadOptions.None);

                    // Check out the file if it is under source control
                    if (DTEManager.DTE.SourceControl.IsItemUnderSCC(filename)
                        && !DTEManager.DTE.SourceControl.IsItemCheckedOut(filename))
                    {
                        DTEManager.DTE.SourceControl.CheckOutItem(filename);
                    }

                    XElement resource1 = new XElement("ProjectItemFile",
                        new XAttribute("Source", webPartResxFileName),
                        new XAttribute("Type", "AppGlobalResource"));

                    c.Element(sharepointToolsNamespace + "Files").Add(resource1);

                    XDocument firstDoc = new XDocument(declaration, c);

                    string correctedDoc = firstDoc.ToString().Replace(@" xmlns=""""", "");

                    correctedDoc = firstDoc.Declaration.ToString() + correctedDoc;

                    appGlobalResourcesSPData.Delete();

                    string tempSPDataFileName = Path.GetTempFileName();
                    using (StreamWriter writer2 = new StreamWriter(tempSPDataFileName, false))
                    {
                        writer2.WriteLine(correctedDoc);
                    }
                    ProjectItem spDataProjectItem = appGlobalResourcesFolderProjectItem.ProjectItems.AddFromTemplate(tempSPDataFileName, "SharePointProjectItem.spdata");

                    File.Delete(tempSPDataFileName);

                }
            }
        }

        /// <summary>
        /// Creates the app global resources SP data.
        /// </summary>
        /// <param name="declaration">The declaration.</param>
        /// <param name="sharepointToolsNamespace">The sharepoint tools namespace.</param>
        /// <param name="webPartResxFileName">Name of the web part RESX file.</param>
        /// <returns>A sting representation of the app global resources spdata file.</returns>
        private string CreateAppGlobalResourcesSPData(XDeclaration declaration,
            XNamespace sharepointToolsNamespace,
            string webPartResxFileName)
        {
            #region spdata

            XElement resource1 = new XElement("ProjectItemFile",
                new XAttribute("Source", webPartResxFileName),
                new XAttribute("Type", "AppGlobalResource"));

            XElement files = new XElement(XNamespace.None + "Files", resource1);

            XElement projectItem = new XElement(sharepointToolsNamespace + "ProjectItem",
                new XAttribute("Type", "Microsoft.VisualStudio.SharePoint.GenericElement"),
                new XAttribute("SupportedTrustLevels", "All"),
                new XAttribute("SupportedDeploymentScopes", "Web, Site, WebApplication, Farm, Package"),
                files);

            XDocument firstDoc = new XDocument(declaration, projectItem);

            string correctedDoc = firstDoc.ToString().Replace(@" xmlns=""""", "");

            correctedDoc = firstDoc.Declaration.ToString() + correctedDoc;

            #endregion

            return correctedDoc;
        }

        #endregion
    }
}

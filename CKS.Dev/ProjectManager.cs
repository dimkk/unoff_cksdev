using System;
using Microsoft.VisualStudio.SharePoint;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using VSLangProj;
using Eval = Microsoft.Build.Evaluation;
using System.Text;

namespace CKS.Dev.VisualStudio.SharePoint
{
    /// <summary>
    /// A utility class to use the Project.
    /// </summary>
    public class ProjectManager
    {
        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        public ISharePointProject Project { get; private set; }

        /// <summary>
        /// Gets or sets the DTE project.
        /// </summary>
        /// <value>The DTE project.</value>
        public EnvDTE.Project DteProject { get; private set; }

        /// <summary>
        /// Gets the project service.
        /// </summary>
        /// <value>The project service.</value>
        public ISharePointProjectService ProjectService
        {
            get { return DTEManager.ProjectService; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectManager"/> class.
        /// </summary>
        /// <param name="project">The project.</param>
        public ProjectManager(ISharePointProject project)
            : this(project, DTEManager.ProjectService.Convert<ISharePointProject, EnvDTE.Project>(project))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectManager"/> class.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="dteProject">The DTE project.</param>
        ProjectManager(ISharePointProject project, EnvDTE.Project dteProject)
        {
            Project = project;
            DteProject = dteProject;
        }

        /// <summary>
        /// Adds the allow partially trusted callers attribute to the AssemblyInfo.
        /// </summary>
        /// <param name="dteProject">The DTE project.</param>
        public static void AddAllowPartiallyTrustedCallersAttribute(EnvDTE.Project dteProject)
        {
            EnvDTE.ProjectItem item = DTEManager.FindItemByName(dteProject.ProjectItems, "assemblyinfo.cs", true);

            bool contains = false;

            FileCodeModel2 model = (FileCodeModel2)item.FileCodeModel;
            foreach (CodeElement codeElement in model.CodeElements)
            {
                if (ExamineCodeElement(codeElement, "allowpartiallytrustedcallers", 3))
                {
                    contains = true;
                    break;
                }
            }

            //The attribute was not found so make sure it gets added.
            if (!contains)
            {
                model.AddAttribute("AllowPartiallyTrustedCallers", null);
            }
        }

        /// <summary>
        /// Examines the code element.
        /// </summary>
        /// <param name="codeElement">The code element.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="tabs">The tabs.</param>
        /// <returns></returns>
        private static bool ExamineCodeElement(CodeElement codeElement, string elementName, int tabs)
        {
            tabs++;
            try
            {
                Console.WriteLine(new string('\t', tabs) + "{0} {1}",
                    codeElement.Name, codeElement.Kind.ToString());

                // if this is a namespace, add a class to it.
                if (codeElement.Kind == vsCMElement.vsCMElementAttribute)
                {
                    if (codeElement.Name.ToLower() == elementName)
                    {
                        return true;
                    }
                }

                foreach (CodeElement childElement in codeElement.Children)
                {
                    if (ExamineCodeElement(childElement, elementName, tabs))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                Console.WriteLine(new string('\t', tabs) + "codeElement without name: {0}", codeElement.Kind.ToString());
            }
            return false;
        }



        /// <summary>
        /// Creates the specified DTE project.
        /// </summary>
        /// <param name="dteProject">The DTE project.</param>
        /// <returns></returns>
        public static ProjectManager Create(EnvDTE.Project dteProject)
        {
            if (DTEManager.IsSharePointProject(dteProject) == false)
            {
                throw new ArgumentException();
            }
            ISharePointProject project = DTEManager.ProjectService.Convert<EnvDTE.Project, ISharePointProject>(dteProject);
            return new ProjectManager(project, dteProject);
        }

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        /// <returns></returns>
        public string GetAssemblyName()
        {
            string outputFullPath = Project.OutputFullPath;
            if (File.Exists(outputFullPath))
            {
                return AssemblyName.GetAssemblyName(outputFullPath).FullName;
            }
            return Path.GetFileNameWithoutExtension(outputFullPath);
        }

        /// <summary>
        /// Gets the type of the items of.
        /// </summary>
        /// <param name="projectItemTypeId">The project item type id.</param>
        /// <returns></returns>
        public IEnumerable<ISharePointProjectItem> GetItemsOfType(string projectItemTypeId)
        {
            return Project.ProjectItems.Where(
                i => i.ProjectItemType.Id == projectItemTypeId);
        }

        /// <summary>
        /// Projects the contains file.
        /// </summary>
        /// <param name="dteProject">The DTE project.</param>
        /// <param name="filename">The filename.</param>
        /// <returns>True if the project contains a file with the saught after name.</returns>
        public static bool ProjectContainsFile(EnvDTE.Project dteProject, string filename)
        {
            return (DTEManager.FindItemByName(dteProject.ProjectItems, filename, true) != null);
        }

        /// <summary>
        /// Adds the reference.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="reference">The reference.</param>
        /// <returns>True if the reference got added, false if not</returns>
        public bool AddReference(EnvDTE.Project project, string reference)
        {
            VSProject proj = project.Object as VSProject;
            System.Diagnostics.Debug.Assert(proj != null); // This project is not a VSProject
            if (proj == null)
            {
                return false;
            }

            try
            {
                proj.References.Add(reference);
            }
            catch (Exception ex)
            {
                string message = String.Format("Could not add {0}. \n Exception: {1}", reference, ex.Message);
                System.Diagnostics.Trace.WriteLine(message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds a file extension to the TokenReplacementFileExtensions project property
        /// </summary>
        /// <param name="project">The project file to be altered</param>
        /// <param name="extension">The file extension to be added excluding and leading period (ie. svc) </param>
        public void AddTokenReplacementFileExtension(EnvDTE.Project project, string extension)
        {
            Eval.Project prj = Eval.ProjectCollection.GlobalProjectCollection.GetLoadedProjects(project.FullName).FirstOrDefault();
            
            if (prj == null)
            {
                prj = new Eval.Project(project.FullName);
            }
            
            Eval.ProjectProperty prop = prj.GetProperty("TokenReplacementFileExtensions");
            
            string val;
            
            if (prop != null)
            {
                List<string> elements = new List<string>(prop.EvaluatedValue.Split(';'));

                List<string> distinctElements = elements.Distinct().ToList();
                distinctElements.RemoveAll(p => p == String.Empty);

                if (!distinctElements.Contains(extension))
                {
                    distinctElements.Add(extension);
                }

                StringBuilder sb = new StringBuilder("$(TokenReplacementFileExtensions);");

                foreach (string item in distinctElements)
                {
                    sb.Append(item + ";");
                }

                val = sb.ToString();
            }
            else
            {
                val = "$(TokenReplacementFileExtensions);" + extension + ";";
            }

            prop = prj.SetProperty("TokenReplacementFileExtensions", val);
        }

        public List<string> GetTokenReplacementFileExtension(EnvDTE.Project project)
        {
            Eval.Project prj = Eval.ProjectCollection.GlobalProjectCollection.GetLoadedProjects(project.FullName).FirstOrDefault();

            if (prj == null)
            {
                prj = new Eval.Project(project.FullName);
            }

            Eval.ProjectProperty prop = prj.GetProperty("TokenReplacementFileExtensions");

            if (prop != null)
            {
                List<string> elements = new List<string>(prop.EvaluatedValue.Split(';'));

                List<string> distinctElements = elements.Distinct().ToList();
                distinctElements.RemoveAll(p => p == String.Empty);

                return distinctElements;
            }
            else
            {
                return null;
            }
        }
    }
}

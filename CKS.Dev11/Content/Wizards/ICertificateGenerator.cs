using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace CKS.Dev11.VisualStudio.SharePoint.Content.Wizards
{
    /// <summary>
    /// Interface to define the Certificate Generator
    /// </summary>
    public interface ICertificateGenerator
    {
        /// <summary>
        /// Adds the key file.
        /// </summary>
        /// <param name="project">The project.</param>
        void AddKeyFile(Project project);

        /// <summary>
        /// Generates the key file.
        /// </summary>
        /// <param name="replacementsDictionary">The replacements dictionary.</param>
        void GenerateKeyFile(Dictionary<string, string> replacementsDictionary);
    }
}

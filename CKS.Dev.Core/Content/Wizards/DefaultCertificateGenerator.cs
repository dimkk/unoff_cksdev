using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Content.Wizards
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Content.Wizards
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Content.Wizards
#else
namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
#endif
{
    /// <summary>
    /// Helper to create certificates
    /// </summary>
    class DefaultCertificateGenerator : ICertificateGenerator
    {
        #region Fields

        /// <summary>
        /// The _strong name generated
        /// </summary>
        bool _strongNameGenerated;

        /// <summary>
        /// The project manager
        /// </summary>
        StrongNameProjectManager projectManager = new StrongNameProjectManager();

        #endregion

        #region Methods

        /// <summary>
        /// Adds the key file.
        /// </summary>
        /// <param name="project">The project.</param>
        public void AddKeyFile(EnvDTE.Project project)
        {
            if (this._strongNameGenerated)
            {
                this.projectManager.AddKeyFileToProject(project);
                this._strongNameGenerated = false;
            }
        }

        /// <summary>
        /// Generates the key file.
        /// </summary>
        /// <param name="replacementsDictionary">The replacements dictionary.</param>
        public void GenerateKeyFile(Dictionary<string, string> replacementsDictionary)
        {
            this.projectManager.GenerateKey();
            this.projectManager.AddKeyToDictionary(replacementsDictionary);
            this._strongNameGenerated = true;
        }

        #endregion
    }
}

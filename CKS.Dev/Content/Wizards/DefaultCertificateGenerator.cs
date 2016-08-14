using System;
using EnvDTE;
using System.Collections.Generic;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    class DefaultCertificateGenerator
        : ICertificateGenerator
    {
        bool _strongNameGenerated;

        StrongNameProjectManager projectManager = new StrongNameProjectManager();

        public void AddKeyFile(EnvDTE.Project project)
        {
            if (this._strongNameGenerated)
            {
                this.projectManager.AddKeyFileToProject(project);
                this._strongNameGenerated = false;
            }
        }

        public void GenerateKeyFile(Dictionary<string, string> replacementsDictionary)
        {
            this.projectManager.GenerateKey();
            this.projectManager.AddKeyToDictionary(replacementsDictionary);
            this._strongNameGenerated = true;
        }
    }
}

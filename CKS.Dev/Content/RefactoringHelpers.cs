using System;
using System.IO;
using Microsoft.VisualStudio.SharePoint;
using EnvDTE;

namespace CKS.Dev.VisualStudio.SharePoint.Content
{
    class RefactoringHelpers
    {
        public static bool IsCodeFile(ISharePointProjectItemFile projectItemfile)
        {
            //TODO: fix this as the class its calling is wrong at the mo
            EnvDTE.Project project = (projectItemfile as ProjectItem).ContainingProject;//)ExtendedSharePointServices.GetProjectService(projectItemfile.Project).Convert<ISharePointProject, EnvDTE.Project>(projectItemfile.Project);
            string extension = Path.GetExtension(projectItemfile.FullPath);
            switch (project.CodeModel.Language)
            {
                case "{B5E9BD34-6D3E-4B5D-925E-8A43B79820B4}":
                    return StringComparer.OrdinalIgnoreCase.Equals(".cs", extension);

                case "{B5E9BD33-6D3E-4B5D-925E-8A43B79820B4}":
                    return StringComparer.OrdinalIgnoreCase.Equals(".vb", extension);
            }
            throw new NotSupportedException();
        }
    }
}

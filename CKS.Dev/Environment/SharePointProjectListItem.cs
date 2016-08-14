using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.VisualStudio.SharePoint.Environment {
    public class SharePointProjectListItem {
        public ISharePointProject Project { get; private set; }

        public SharePointProjectListItem(ISharePointProject project) {
            if (project == null) {
                throw new ArgumentNullException("project");
            }

            Project = project;
        }

        public override string ToString() {
            return Project.Name;
        }
    }
}

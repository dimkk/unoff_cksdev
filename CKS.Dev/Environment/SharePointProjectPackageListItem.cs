using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.VisualStudio.SharePoint.Environment {
    public class SharePointProjectPackageListItem {
        public ISharePointProjectPackage Package { get; private set; }

        public SharePointProjectPackageListItem(ISharePointProjectPackage package) {
            if (package == null) {
                throw new ArgumentNullException("package");
            }

            Package = package;
        }

        public override string ToString() {
            return Package.Model.Name;
        }
    }
}

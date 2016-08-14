using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.VisualStudio.SharePoint.Environment {
    public class SharePointProjectFeatureListItem {
        public ISharePointProjectFeature Feature { get; private set; }

        public SharePointProjectFeatureListItem(ISharePointProjectFeature feature) {
            if (feature == null) {
                throw new ArgumentNullException("feature");
            }

            Feature = feature;
        }

        public override string ToString() {
            return String.Format("{0} ({1}; {2})", Feature.Model.Title, Feature.Model.Scope, Feature.Project.Name);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint;
using CKS.Dev.VisualStudio.SharePoint.Environment.Options;

namespace CKS.Dev.VisualStudio.SharePoint.Environment {
    /// <summary>
    /// Cancel Adding SPIs extension
    /// </summary>
    [Export(typeof(ISharePointProjectExtension))]
    public class CancelAddingSPIProjectExtension : ISharePointProjectExtension {
        private Guid recentlyAddedItem = Guid.Empty;

        public void Initialize(ISharePointProjectService projectService) {
            if (EnabledExtensionsOptionsPage.GetSetting<bool>(EnabledExtensionsOptions.CancelAddingSPIs, true)) {
                foreach (ISharePointProjectItemType type in projectService.ProjectItemTypes.Values) {
                    type.ProjectItemAdded += new EventHandler<SharePointProjectItemEventArgs>(spiType_ProjectItemAdded);
                    type.ProjectItemInitialized += new EventHandler<SharePointProjectItemEventArgs>(spiType_ProjectItemInitialized);
                }
            }
        }

        private void spiType_ProjectItemAdded(object sender, SharePointProjectItemEventArgs e) {
            recentlyAddedItem = e.ProjectItem.Id;
        }

        private void spiType_ProjectItemInitialized(object sender, SharePointProjectItemEventArgs e) {
            if (recentlyAddedItem == e.ProjectItem.Id) {
                recentlyAddedItem = Guid.Empty;
                ISharePointProjectItem spi = e.ProjectItem;
                IEnumerable<ISharePointProjectFeature> source = from ISharePointProjectFeature feature
                                                                in spi.Project.Features
                                                                where feature.ProjectItems.Contains(spi)
                                                                select feature;

                if ((source != null) && (source.Count() > 0)) {
                    foreach (ISharePointProjectFeature feature in source) {
                        feature.ProjectItems.Remove(spi);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.SharePoint;
using CKS.Dev.VisualStudio.SharePoint;

namespace CKS.Dev.VisualStudio.SharePoint.Environment {
    public partial class PackagesViewerForm : Form {
        private List<ISharePointProjectPackage> allPackages;

        public ISharePointProjectPackage SelectedPackage {
            get {
                ISharePointProjectPackage selectedPackage = null;

                SharePointProjectPackageListItem selectedPackageItem = Packages.SelectedItem as SharePointProjectPackageListItem;
                if (selectedPackageItem != null) {
                    selectedPackage = selectedPackageItem.Package;
                }

                return selectedPackage;
            }
        }

        public PackagesViewerForm(ISharePointProject project) {
            if (project == null) {
                throw new ArgumentNullException("project");
            }

            InitializeComponent();
            LoadPackages(project);
            FilterPackagesList();
        }

        private void FilterPackagesList() {
            var packages = from ISharePointProjectPackage p
                           in allPackages
                           select new SharePointProjectPackageListItem(p);

            if (!String.IsNullOrEmpty(Filter.Text)) {
                packages = from SharePointProjectPackageListItem packageItem
                           in packages
                           where packageItem.Package.Model.Name.Contains(Filter.Text, StringComparison.InvariantCultureIgnoreCase)
                           select packageItem;
            }

            Packages.DataSource = packages.OrderBy(p => p.Package.Model.Name).ToList();
        }

        private void LoadPackages(ISharePointProject project) {
            allPackages = new List<ISharePointProjectPackage>();

            foreach (var p in project.ProjectService.Projects) {
                allPackages.Add(p.Package);
            }
        }

        private void Filter_TextChanged(object sender, EventArgs e) {
            FilterPackagesList();
        }
    }
}

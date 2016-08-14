﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.SharePoint;

#if VS2012Build_SYMBOL
using CKS.Dev11.VisualStudio.SharePoint;
using CKS.Dev11.VisualStudio.SharePoint.Environment.Dialogs;
#elif VS2013Build_SYMBOL
using CKS.Dev12.VisualStudio.SharePoint;
using CKS.Dev12.VisualStudio.SharePoint.Environment.Dialogs;
#elif VS2014Build_SYMBOL
using CKS.Dev13.VisualStudio.SharePoint;
using CKS.Dev13.VisualStudio.SharePoint.Environment.Dialogs;
#else
using CKS.Dev.VisualStudio.SharePoint;
using CKS.Dev.VisualStudio.SharePoint.Environment.Dialogs;
#endif


namespace CKS.Dev.Core.VisualStudio.SharePoint.Environment.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PackagesViewerForm : Form {
        private List<ISharePointProjectPackage> allPackages;

        /// <summary>
        /// Gets the selected package.
        /// </summary>
        /// <value>
        /// The selected package.
        /// </value>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="PackagesViewerForm" /> class.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <exception cref="System.ArgumentNullException">project</exception>
        public PackagesViewerForm(ISharePointProject project) {
            if (project == null) {
                throw new ArgumentNullException("project");
            }

            InitializeComponent();
            LoadPackages(project);
            FilterPackagesList();
        }

        /// <summary>
        /// Filters the packages list.
        /// </summary>
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

        /// <summary>
        /// Loads the packages.
        /// </summary>
        /// <param name="project">The project.</param>
        private void LoadPackages(ISharePointProject project) {
            allPackages = new List<ISharePointProjectPackage>();

            foreach (var p in project.ProjectService.Projects) {
                allPackages.Add(p.Package);
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the Filter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Filter_TextChanged(object sender, EventArgs e) {
            FilterPackagesList();
        }
    }
}

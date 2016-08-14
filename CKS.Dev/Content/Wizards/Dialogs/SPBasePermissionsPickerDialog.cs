using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.SharePoint;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Properties;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Dialogs
{
    /// <summary>
    /// A dialog form for the selection of SPBasePermissions.
    /// </summary>
    public partial class SPBasePermissionsPickerDialog : Form
    {
        #region Properties

        /// <summary>
        /// Gets or sets the selected base permissions.
        /// </summary>
        public string SelectedBasePermissions
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Create a new instance of the SPBasePermissionsPickerDialog.
        /// </summary>
        private SPBasePermissionsPickerDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create a new instance of the SPBasePermissionsPickerDialog.
        /// </summary>
        /// <param name="selectedBasePermissions">The SPBasePermissions to pre-load.</param>
        public SPBasePermissionsPickerDialog(string selectedBasePermissions)
        {
            InitializeComponent();
            SelectedBasePermissions = selectedBasePermissions;
        }

        /// <summary>
        /// OnLoad to configure the form and load the list view items.
        /// </summary>
        /// <param name="e">The EventArgs objetc.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Set the label text
            lblIntroduction.Text = Resources.SPBasePermissionsPickerDialog_Introduction;
            lblBasePermissions.Text = Resources.SPBasePermissionsPickerDialog_BasePermissions;
            lnkMSDNArticle.Text = Resources.SPBasePermissionsPickerDialog_MSDNLinkText;

            System.Windows.Forms.LinkLabel.Link link = new System.Windows.Forms.LinkLabel.Link();
            link.Description = Resources.SPBasePermissionsPickerDialog_MSDNLinkText;
            link.LinkData = Resources.SPBasePermissionsPickerDialog_MSDNLinkUrl;
            link.Enabled = true;
            link.Name = "lnkMSDN";
            lnkMSDNArticle.Links.Add(link);

            SetSelectedPermissions();
        }

        /// <summary>
        /// Launch the MSDN link.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The LinkLabelLinkClickedEventArgs object.</param>
        private void lnkMSDNArticle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Display the appropriate link based on the value of the 
            // LinkData property of the Link object.
            string target = e.Link.LinkData as string;

            // If the value looks like a URL, navigate to it.
            if (target != null && target.StartsWith("http"))
            {
                System.Diagnostics.Process.Start(target);
            }
        }
        
        /// <summary>
        /// Get the SPBasePermissions and the descriptions from the SP Command.
        /// </summary>
        /// <returns>A disctionary of SPBasePermission values with a description.</returns>
        private Dictionary<string, string> GetSPBasePermissions()
        {
            ISharePointProjectService sharePointService = DTEManager.ProjectService;

            return sharePointService.SharePointConnection.ExecuteCommand<Dictionary<string, string>>(ObjectModelSharePointCommandIds.GetSPBasePermissions);
        }

        /// <summary>
        /// Set the selected permissions and load the base permissions.
        /// </summary>
        private void SetSelectedPermissions()
        {
            //Call down to the sharepoint commands to get the enum values
            Dictionary<string, string> basePermissions = GetSPBasePermissions();

            if (basePermissions.Count > 0)
            {
                if (!String.IsNullOrWhiteSpace(SelectedBasePermissions))
                {
                    List<string> selected = new List<string>(SelectedBasePermissions.Split(",".ToCharArray()));

                    if (selected.Count > 0)
                    {
                        foreach (var item in basePermissions)
                        {
                            //Find out if the base permission is selected.
                            if (selected.Exists(c => item.Key == c))
                            {
                                ListViewItem newItem = lvwSPBasePermissions.Items.Add(item.Key.ToString());
                                newItem.SubItems.Add(item.Value.ToString());
                                newItem.Selected = true;
                            }
                            else
                            {
                                ListViewItem newItem = lvwSPBasePermissions.Items.Add(item.Key.ToString());
                                newItem.SubItems.Add(item.Value.ToString());
                                newItem.Selected = false;
                            }
                        }
                        //processing complete so jump out this method
                        return;
                    }
                }

                foreach (var item in basePermissions)
                {
                    ListViewItem newItem = lvwSPBasePermissions.Items.Add(item.Key.ToString());
                    newItem.SubItems.Add(item.Value.ToString());
                    newItem.Selected = false;
                }
            }
            else
            {
                //Something should have come back!
                throw new ApplicationException(Resources.SPBasePermissionsPickerDialog_NoPermissionsException);
            }
        }

        /// <summary>
        /// Set the selected permissions property.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The EventArgs object.</param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            string selectedPermissions = String.Empty;
            foreach (var item in lvwSPBasePermissions.SelectedItems)
            {
                if (selectedPermissions != String.Empty)
                {
                    selectedPermissions = selectedPermissions + "," + (item as ListViewItem).Text;
                }
                else
                {
                    selectedPermissions = (item as ListViewItem).Text;
                }
            }
            SelectedBasePermissions = selectedPermissions;
        }
        
    }
}

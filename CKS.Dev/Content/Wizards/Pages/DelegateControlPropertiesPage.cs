using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using System.Windows.Forms;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.WizardProperties;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models;
using CKS.Dev.VisualStudio.SharePoint.Properties;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Pages
{
    /// <summary>
    /// The second page of the Delegate Control wizard used to capture properties.
    /// </summary>
    class DelegateControlPropertiesPage : BaseWizardPage
    {
        #region Controls

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox grpAddNewProperty;
        private Label lblPropertyName;
        private TextBox txtPropertyName;
        private Label lblPropertyValue;
        private TextBox txtPropertyValue;
        private Button btnAddNewProperty;
        private GroupBox grpProperties;
        private ListView lviDelegateControlProperties;
        private ColumnHeader colName;
        private ColumnHeader colValue;
        private Button btnRemoveSelectedProperties;
        private Label lblIntroduction;
        private LinkLabel lnkMSDNArticle;
        private Label lblRequired;
        private Label lblAddProperty;
        private Label lblProperties;
        private System.ComponentModel.IContainer components;
        private ErrorProvider errMessages;

        #endregion

        #region Fields

        /// <summary>
        /// Field to hold the presentation model
        /// </summary>
        DelegateControlPresentationModel _model;

        #endregion

        #region Methods

        /// <summary>
        /// Initialises a new DelegateControlPropertiesPage
        /// </summary>
        /// <param name="wizard">The ArtifactWizardForm object</param>
        /// <param name="model">The DelegateControlPresentationModel object</param>
        public DelegateControlPropertiesPage(ArtifactWizardForm wizard, DelegateControlPresentationModel model)
            : base(wizard)
        {
            _model = model;
            InitializeComponent();
            base.LoadSettings();
            base.Headline = Resources.DelegateControlPropertiesPage_StepTitle;
            base.HelpKeyword = "VS.SharePointTools.SPE.DelegateControl";
        }

        /// <summary>
        /// Load the settings from the presentation model
        /// </summary>
        protected override void LoadSettingsFromPresentationModel()
        {
            base.Skippable = _model.IsOptional;
            lviDelegateControlProperties.Items.Clear();
            //Do the databind for the properties to the listview
            foreach (DelegateControlPropertyProperties item in _model.ControlProperties)
            {
                ListViewItem parentItem = new ListViewItem();
                parentItem.Text = txtPropertyName.Text;
                parentItem.SubItems.Add(new ListViewItem.ListViewSubItem(parentItem, txtPropertyValue.Text));
                lviDelegateControlProperties.Items.Add(parentItem);
                lviDelegateControlProperties.Refresh();
            }
        }

        /// <summary>
        /// Do the databinding from the model to the form components
        /// </summary>
        /// <returns>True</returns>
        public override bool OnActivate()
        {
            //Do any databinding required here

            lviDelegateControlProperties.Items.Clear();
            //Do the databind for the properties to the listview
            foreach (DelegateControlPropertyProperties item in _model.ControlProperties)
            {
                ListViewItem parentItem = new ListViewItem();
                parentItem.Text = txtPropertyName.Text;
                parentItem.SubItems.Add(new ListViewItem.ListViewSubItem(parentItem, txtPropertyValue.Text));
                lviDelegateControlProperties.Items.Add(parentItem);
                lviDelegateControlProperties.Refresh();
            }


            return base.OnActivate();
        }

        /// <summary>
        /// Onload to set the text and values.
        /// </summary>
        /// <param name="e">The EventArgs object.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Set the label text
            lblIntroduction.Text = Resources.DelegateControlPropertiesPage_Introduction;
            lblRequired.Text = Resources.DelegateControlPropertiesPage_Required;
            lnkMSDNArticle.Text = Resources.DelegateControlPropertiesPage_MSDNLinkText;
            lblProperties.Text = Resources.DelegateControlPropertiesPage_Properties;
            lblAddProperty.Text = Resources.DelegateControlPropertiesPage_AddProperty;

            System.Windows.Forms.LinkLabel.Link link = new System.Windows.Forms.LinkLabel.Link();
            link.Description = Resources.DelegateControlPropertiesPage_MSDNLinkText;
            link.LinkData = Resources.DelegateControlPropertiesPage_MSDNLinkUrl;
            link.Enabled = true;
            link.Name = "lnkMSDN";
            lnkMSDNArticle.Links.Add(link);
        }

        /// <summary>
        /// Save the changes.
        /// </summary>
        /// <returns>True.</returns>
        public override bool OnDeactivate()
        {
            this.ValidateChildren();
            if (!base.Wizard.MovingPrevious)
            {
                return base.OnDeactivate();
            }
            if (!IsCompletelyValid())
            {
                base.Visited = false;
                base.Skippable = false;
            }
            return true;
        }

        /// <summary>
        /// Apply the changes to the model and invoke its save changes
        /// </summary>
        protected override void ApplyChangesToPresentationModel()
        {
            _model.SaveChanges();
        }

        /// <summary>
        /// Add the new property.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The EventArgs object.</param>
        private void btnAddNewProperty_Click(object sender, EventArgs e)
        {
            DelegateControlPropertyProperties property = new DelegateControlPropertyProperties();
            property.Name = txtPropertyName.Text;
            property.Value = txtPropertyValue.Text;

            if(!_model.ControlProperties.Exists(v => property.Name == v.Name))
            {
                // Set the error if the name is not valid.
                errMessages.SetError(txtPropertyName, "");
                txtPropertyName.Text = "";
                txtPropertyValue.Text = "";

                _model.ControlProperties.Add(property);

                lviDelegateControlProperties.Items.Clear();
                //Do the databind for the properties to the listview
                foreach (DelegateControlPropertyProperties item in _model.ControlProperties)
                {
                    ListViewItem parentItem = new ListViewItem();
                    parentItem.Text = item.Name;
                    parentItem.SubItems.Add(new ListViewItem.ListViewSubItem(parentItem, item.Value));
                    lviDelegateControlProperties.Items.Add(parentItem);
                    lviDelegateControlProperties.Refresh();
                }
            }
            else
            {
                // Set the error if the name is not valid.
                errMessages.SetError(txtPropertyName, String.Format(Resources.DelegateControlPropertiesPage_AlreadyAdded_Error,  property.Name));
            }            
        }

        /// <summary>
        /// Remove the selected items from the properties collection.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The EventArgs object.</param>
        private void btnRemoveSelectedProperties_Click(object sender, EventArgs e)
        {
            List<DelegateControlPropertyProperties> itemsToRemove = new List<DelegateControlPropertyProperties>();

            foreach (ListViewItem item in lviDelegateControlProperties.SelectedItems)
            {
                DelegateControlPropertyProperties foundItem = _model.ControlProperties.Find(lv => lv.Name == item.Text);
                if (foundItem != null)
                {
                    itemsToRemove.Add(foundItem);
                }
            }

            foreach (DelegateControlPropertyProperties item in itemsToRemove)
            {
                _model.ControlProperties.Remove(item);
            }

            lviDelegateControlProperties.Items.Clear();
            //Do the databind for the properties to the listview
            foreach (DelegateControlPropertyProperties item in _model.ControlProperties)
            {
                ListViewItem parentItem = new ListViewItem();
                parentItem.Text = item.Name;
                parentItem.SubItems.Add(new ListViewItem.ListViewSubItem(parentItem, item.Value));
                lviDelegateControlProperties.Items.Add(parentItem);
                lviDelegateControlProperties.Refresh();
            }
        }

        /// <summary>
        /// Launch the MSDN link.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The LinkLabelLinkClickedEventArgs object.</param>
        private void lnkMSDNArticle_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
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

        #endregion
        
        #region Designer Generated

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grpAddNewProperty = new System.Windows.Forms.GroupBox();
            this.lblPropertyName = new System.Windows.Forms.Label();
            this.txtPropertyName = new System.Windows.Forms.TextBox();
            this.lblPropertyValue = new System.Windows.Forms.Label();
            this.txtPropertyValue = new System.Windows.Forms.TextBox();
            this.btnAddNewProperty = new System.Windows.Forms.Button();
            this.grpProperties = new System.Windows.Forms.GroupBox();
            this.lviDelegateControlProperties = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRemoveSelectedProperties = new System.Windows.Forms.Button();
            this.lblIntroduction = new System.Windows.Forms.Label();
            this.lnkMSDNArticle = new System.Windows.Forms.LinkLabel();
            this.lblRequired = new System.Windows.Forms.Label();
            this.lblAddProperty = new System.Windows.Forms.Label();
            this.lblProperties = new System.Windows.Forms.Label();
            this.errMessages = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.grpAddNewProperty.SuspendLayout();
            this.grpProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errMessages)).BeginInit();
            this.SuspendLayout();
            // 
            // infoPanel
            // 
            this.infoPanel.BackColor = System.Drawing.SystemColors.Control;
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.None;
            this.infoPanel.Location = new System.Drawing.Point(0, 114);
            this.infoPanel.Size = new System.Drawing.Size(505, 304);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.grpAddNewProperty, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.grpProperties, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblIntroduction, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lnkMSDNArticle, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblRequired, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblAddProperty, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblProperties, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 12;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(508, 333);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // grpAddNewProperty
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.grpAddNewProperty, 2);
            this.grpAddNewProperty.Controls.Add(this.lblPropertyName);
            this.grpAddNewProperty.Controls.Add(this.txtPropertyName);
            this.grpAddNewProperty.Controls.Add(this.lblPropertyValue);
            this.grpAddNewProperty.Controls.Add(this.txtPropertyValue);
            this.grpAddNewProperty.Controls.Add(this.btnAddNewProperty);
            this.grpAddNewProperty.Location = new System.Drawing.Point(3, 214);
            this.grpAddNewProperty.Name = "grpAddNewProperty";
            this.grpAddNewProperty.Size = new System.Drawing.Size(502, 100);
            this.grpAddNewProperty.TabIndex = 1;
            this.grpAddNewProperty.TabStop = false;
            this.grpAddNewProperty.Text = "Add new property";
            // 
            // lblPropertyName
            // 
            this.lblPropertyName.AutoSize = true;
            this.lblPropertyName.Location = new System.Drawing.Point(6, 27);
            this.lblPropertyName.Name = "lblPropertyName";
            this.lblPropertyName.Size = new System.Drawing.Size(80, 13);
            this.lblPropertyName.TabIndex = 23;
            this.lblPropertyName.Text = "Property Name:";
            // 
            // txtPropertyName
            // 
            this.txtPropertyName.Location = new System.Drawing.Point(92, 24);
            this.txtPropertyName.Name = "txtPropertyName";
            this.txtPropertyName.Size = new System.Drawing.Size(296, 20);
            this.txtPropertyName.TabIndex = 0;
            // 
            // lblPropertyValue
            // 
            this.lblPropertyValue.AutoSize = true;
            this.lblPropertyValue.Location = new System.Drawing.Point(6, 56);
            this.lblPropertyValue.Name = "lblPropertyValue";
            this.lblPropertyValue.Size = new System.Drawing.Size(79, 13);
            this.lblPropertyValue.TabIndex = 21;
            this.lblPropertyValue.Text = "Property Value:";
            // 
            // txtPropertyValue
            // 
            this.txtPropertyValue.Location = new System.Drawing.Point(92, 50);
            this.txtPropertyValue.Name = "txtPropertyValue";
            this.txtPropertyValue.Size = new System.Drawing.Size(296, 20);
            this.txtPropertyValue.TabIndex = 1;
            // 
            // btnAddNewProperty
            // 
            this.btnAddNewProperty.Location = new System.Drawing.Point(409, 22);
            this.btnAddNewProperty.Name = "btnAddNewProperty";
            this.btnAddNewProperty.Size = new System.Drawing.Size(75, 23);
            this.btnAddNewProperty.TabIndex = 2;
            this.btnAddNewProperty.Text = "Add property";
            this.btnAddNewProperty.UseVisualStyleBackColor = true;
            this.btnAddNewProperty.Click += new System.EventHandler(this.btnAddNewProperty_Click);
            // 
            // grpProperties
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.grpProperties, 2);
            this.grpProperties.Controls.Add(this.lviDelegateControlProperties);
            this.grpProperties.Controls.Add(this.btnRemoveSelectedProperties);
            this.grpProperties.Location = new System.Drawing.Point(3, 66);
            this.grpProperties.Name = "grpProperties";
            this.grpProperties.Size = new System.Drawing.Size(502, 125);
            this.grpProperties.TabIndex = 0;
            this.grpProperties.TabStop = false;
            this.grpProperties.Text = "Properties";
            // 
            // lviDelegateControlProperties
            // 
            this.lviDelegateControlProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colValue});
            this.lviDelegateControlProperties.FullRowSelect = true;
            this.lviDelegateControlProperties.Location = new System.Drawing.Point(9, 19);
            this.lviDelegateControlProperties.Name = "lviDelegateControlProperties";
            this.lviDelegateControlProperties.Size = new System.Drawing.Size(379, 98);
            this.lviDelegateControlProperties.TabIndex = 0;
            this.lviDelegateControlProperties.UseCompatibleStateImageBehavior = false;
            this.lviDelegateControlProperties.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 186;
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            this.colValue.Width = 189;
            // 
            // btnRemoveSelectedProperties
            // 
            this.btnRemoveSelectedProperties.Location = new System.Drawing.Point(409, 19);
            this.btnRemoveSelectedProperties.Name = "btnRemoveSelectedProperties";
            this.btnRemoveSelectedProperties.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveSelectedProperties.TabIndex = 1;
            this.btnRemoveSelectedProperties.Text = "Remove";
            this.btnRemoveSelectedProperties.UseVisualStyleBackColor = true;
            this.btnRemoveSelectedProperties.Click += new System.EventHandler(this.btnRemoveSelectedProperties_Click);
            // 
            // lblIntroduction
            // 
            this.lblIntroduction.AutoSize = true;
            this.lblIntroduction.Location = new System.Drawing.Point(3, 10);
            this.lblIntroduction.MaximumSize = new System.Drawing.Size(400, 0);
            this.lblIntroduction.Name = "lblIntroduction";
            this.lblIntroduction.Size = new System.Drawing.Size(63, 13);
            this.lblIntroduction.TabIndex = 24;
            this.lblIntroduction.Text = "Introduction";
            // 
            // lnkMSDNArticle
            // 
            this.lnkMSDNArticle.AutoSize = true;
            this.lnkMSDNArticle.Location = new System.Drawing.Point(358, 10);
            this.lnkMSDNArticle.Name = "lnkMSDNArticle";
            this.lnkMSDNArticle.Size = new System.Drawing.Size(39, 13);
            this.lnkMSDNArticle.TabIndex = 25;
            this.lnkMSDNArticle.TabStop = true;
            this.lnkMSDNArticle.Text = "MSDN";
            this.lnkMSDNArticle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMSDNArticle_LinkClicked);
            // 
            // lblRequired
            // 
            this.lblRequired.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblRequired.AutoSize = true;
            this.lblRequired.Location = new System.Drawing.Point(3, 27);
            this.lblRequired.MaximumSize = new System.Drawing.Size(400, 0);
            this.lblRequired.Name = "lblRequired";
            this.lblRequired.Size = new System.Drawing.Size(50, 13);
            this.lblRequired.TabIndex = 26;
            this.lblRequired.Text = "Required";
            // 
            // lblAddProperty
            // 
            this.lblAddProperty.AutoSize = true;
            this.lblAddProperty.Location = new System.Drawing.Point(3, 198);
            this.lblAddProperty.Name = "lblAddProperty";
            this.lblAddProperty.Size = new System.Drawing.Size(99, 13);
            this.lblAddProperty.TabIndex = 91;
            this.lblAddProperty.Text = "Add a new property";
            // 
            // lblProperties
            // 
            this.lblProperties.AutoSize = true;
            this.lblProperties.Location = new System.Drawing.Point(3, 50);
            this.lblProperties.Name = "lblProperties";
            this.lblProperties.Size = new System.Drawing.Size(105, 13);
            this.lblProperties.TabIndex = 90;
            this.lblProperties.Text = "Define the properties";
            // 
            // errMessages
            // 
            this.errMessages.ContainerControl = this;
            // 
            // DelegateControlPropertiesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(0, 360);
            this.Name = "DelegateControlPropertiesPage";
            this.Size = new System.Drawing.Size(508, 421);
            this.Controls.SetChildIndex(this.infoPanel, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.grpAddNewProperty.ResumeLayout(false);
            this.grpAddNewProperty.PerformLayout();
            this.grpProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errMessages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}

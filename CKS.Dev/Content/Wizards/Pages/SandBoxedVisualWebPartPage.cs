using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models;
using CKS.Dev.VisualStudio.SharePoint.Properties;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Pages
{
    /// <summary>
    /// The hide custom action wizard user interface
    /// </summary>
    class SandBoxedVisualWebPartPage : BaseWizardPage
    {
        #region Controls

        private System.Windows.Forms.ErrorProvider errMessages;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblIntroduction;
        private System.Windows.Forms.LinkLabel lnkMSDNArticle;
        private System.Windows.Forms.Label lblRequired;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblGroupId;
        private System.Windows.Forms.TextBox txtGroupId;
        private System.Windows.Forms.Label lblHideActionId;
        private System.Windows.Forms.TextBox txtHideActionId;
        private System.Windows.Forms.Label lblLocation;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.TextBox txtLocation;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the presentation model
        /// </summary>
        protected SandBoxedVisualWebPartPresentationModel CurrentModel
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialises a new SandBoxVisualWebPartPage
        /// </summary>
        /// <param name="wizard">The ArtifactWizardForm object</param>
        /// <param name="model">The SandBoxVisualWebPartPresentationModel object</param>
        public SandBoxedVisualWebPartPage(ArtifactWizardForm wizard, SandBoxedVisualWebPartPresentationModel model)
            : base(wizard)
        {
            CurrentModel = model;
            InitializeComponent();
            base.LoadSettings();
            base.Headline = Resources.SandBoxVisualWebPartPage_StepTitle;
            base.HelpKeyword = "VS.SharePointTools.SPE.SandBoxVisualWebPart";
        }

        /// <summary>
        /// Load the settings from the presentation model
        /// </summary>
        protected override void LoadSettingsFromPresentationModel()
        {
            base.Skippable = CurrentModel.IsOptional;
            txtId.Text = CurrentModel.Id;
            txtGroupId.Text = CurrentModel.GroupId;
            txtHideActionId.Text = CurrentModel.HideActionId;
            txtLocation.Text = CurrentModel.Location;
        }

        /// <summary>
        /// Do the databinding from the model to the form components
        /// </summary>
        /// <returns>True</returns>
        public override bool OnActivate()
        {
            //Do any databinding required here
            return base.OnActivate();
        }

        /// <summary>
        /// On Load sets up the form controls
        /// </summary>
        /// <param name="e">The EventArgs object</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Set the label text
            lblIntroduction.Text = Resources.SandBoxVisualWebPartPage_Introduction;
            lblRequired.Text = Resources.SandBoxVisualWebPartPage_Required;
            lnkMSDNArticle.Text = Resources.SandBoxVisualWebPartPage_MSDNLinkText;

            System.Windows.Forms.LinkLabel.Link link = new System.Windows.Forms.LinkLabel.Link();
            link.Description = Resources.SandBoxVisualWebPartPage_MSDNLinkText;
            link.LinkData = Resources.SandBoxVisualWebPartPage_MSDNLinkUrl;
            link.Enabled = true;
            link.Name = "lnkMSDN";
            lnkMSDNArticle.Links.Add(link);

            lblId.Text = Resources.SandBoxVisualWebPartPage_ID;
            lblGroupId.Text = Resources.SandBoxVisualWebPartPage_GroupId;
            lblHideActionId.Text = Resources.SandBoxVisualWebPartPage_HideActionId;
            lblLocation.Text = Resources.SandBoxVisualWebPartPage_Location;
        }

        /// <summary>
        /// Is the form completely valid? Checks each field for validity
        /// </summary>
        /// <returns>True if all fields are valid</returns>
        protected override bool IsCompletelyValid()
        {
            return base.IsCompletelyValid()
                    && ValidateId()
                    && ValidateGroupId()
                    && ValidateHideActionId()
                    && ValidateLocation();
        }

        /// <summary>
        /// Deactivate to configure the wizard state
        /// </summary>
        /// <returns></returns>
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
            CurrentModel.Id = txtId.Text;
            CurrentModel.GroupId = txtGroupId.Text;
            CurrentModel.HideActionId = txtHideActionId.Text;
            CurrentModel.Location = txtLocation.Text;
            CurrentModel.SaveChanges();
        }

        /// <summary>
        /// Validate the Id
        /// </summary>
        /// <returns>True if the Id is valid</returns>
        protected virtual bool ValidateId()
        {
            return true;
        }

        /// <summary>
        /// Validate the Group Id
        /// </summary>
        /// <returns>True if the Group Id is valid</returns>
        protected virtual bool ValidateGroupId()
        {
            return true;
        }

        /// <summary>
        /// Validate the Hide Action Id
        /// </summary>
        /// <returns>True if the Hide Action Id is valid</returns>
        protected virtual bool ValidateHideActionId()
        {
            return true;
        }

        /// <summary>
        /// Validate the Location
        /// </summary>
        /// <returns>True if the Location is valid</returns>
        protected virtual bool ValidateLocation()
        {
            return true;
        }

        /// <summary>
        /// On link clicked for the msdn article link launches browser to that url
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The LinkLabelLinkClickedEventArgs object</param>
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

        #endregion
        
        #region Designer Generated

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.errMessages = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblIntroduction = new System.Windows.Forms.Label();
            this.lnkMSDNArticle = new System.Windows.Forms.LinkLabel();
            this.lblRequired = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.lblId = new System.Windows.Forms.Label();
            this.lblGroupId = new System.Windows.Forms.Label();
            this.txtGroupId = new System.Windows.Forms.TextBox();
            this.lblHideActionId = new System.Windows.Forms.Label();
            this.txtHideActionId = new System.Windows.Forms.TextBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errMessages)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // infoPanel
            // 
            this.infoPanel.BackColor = System.Drawing.SystemColors.Control;
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.None;
            this.infoPanel.Location = new System.Drawing.Point(0, 238);
            this.infoPanel.Size = new System.Drawing.Size(505, 304);
            // 
            // errMessages
            // 
            this.errMessages.ContainerControl = this;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.lblIntroduction, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lnkMSDNArticle, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblRequired, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtId, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblId, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblGroupId, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.txtGroupId, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.lblHideActionId, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.txtHideActionId, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.lblLocation, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.txtLocation, 0, 15);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 17;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(508, 279);
            this.tableLayoutPanel1.TabIndex = 3;
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
            // txtId
            // 
            this.txtId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtId, 2);
            this.txtId.Location = new System.Drawing.Point(3, 66);
            this.txtId.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(475, 20);
            this.txtId.TabIndex = 28;
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblId, 2);
            this.lblId.Location = new System.Drawing.Point(3, 50);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(19, 13);
            this.lblId.TabIndex = 27;
            this.lblId.Text = "Id:";
            // 
            // lblGroupId
            // 
            this.lblGroupId.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblGroupId, 2);
            this.lblGroupId.Location = new System.Drawing.Point(3, 93);
            this.lblGroupId.Name = "lblGroupId";
            this.lblGroupId.Size = new System.Drawing.Size(51, 13);
            this.lblGroupId.TabIndex = 29;
            this.lblGroupId.Text = "Group Id:";
            // 
            // txtGroupId
            // 
            this.txtGroupId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtGroupId, 2);
            this.txtGroupId.Location = new System.Drawing.Point(3, 109);
            this.txtGroupId.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtGroupId.Name = "txtGroupId";
            this.txtGroupId.Size = new System.Drawing.Size(475, 20);
            this.txtGroupId.TabIndex = 30;
            // 
            // lblHideActionId
            // 
            this.lblHideActionId.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblHideActionId, 2);
            this.lblHideActionId.Location = new System.Drawing.Point(3, 136);
            this.lblHideActionId.Name = "lblHideActionId";
            this.lblHideActionId.Size = new System.Drawing.Size(77, 13);
            this.lblHideActionId.TabIndex = 31;
            this.lblHideActionId.Text = "Hide Action Id:";
            // 
            // txtHideActionId
            // 
            this.txtHideActionId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtHideActionId, 2);
            this.txtHideActionId.Location = new System.Drawing.Point(3, 152);
            this.txtHideActionId.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtHideActionId.Name = "txtHideActionId";
            this.txtHideActionId.Size = new System.Drawing.Size(475, 20);
            this.txtHideActionId.TabIndex = 32;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblLocation, 2);
            this.lblLocation.Location = new System.Drawing.Point(3, 179);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 33;
            this.lblLocation.Text = "Location:";
            // 
            // txtLocation
            // 
            this.txtLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtLocation, 2);
            this.txtLocation.Location = new System.Drawing.Point(3, 195);
            this.txtLocation.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(475, 20);
            this.txtLocation.TabIndex = 34;
            // 
            // SandBoxedVisualWebPartPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SandBoxedVisualWebPartPage";
            this.Size = new System.Drawing.Size(508, 545);
            this.Controls.SetChildIndex(this.infoPanel, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.errMessages)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}

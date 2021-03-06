﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using System.Windows.Forms;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Pages
{
    /// <summary>
    /// The first page of the Custom Action wizard.
    /// </summary>
    class CustomActionPage1 : BaseWizardPage
    {
        #region Controls

        private System.Windows.Forms.ErrorProvider errMessages;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblIntroduction;
        private LinkLabel lnkMSDNArticle;
        private Label lblRequired;
        private Label lblId;
        private TextBox txtId;
        private Label lblTitle;
        private TextBox txtTitle;
        private Label lblDescription;
        private TextBox txtDescription;
        private Label lblGroupId;
        private TextBox txtGroupId;
        private Label lblImageUrl;
        private TextBox txtImageUrl;
        private Label lblLocation;
        private TextBox txtLocation;
        private Label lblSequence;
        private TextBox txtSequence;
        private System.ComponentModel.IContainer components;
        private Button btnGenerateGuid;

        #endregion

        #region Fields

        /// <summary>
        /// Field to hold the presentation model
        /// </summary>
        CustomActionPresentationModel _model;

        #endregion

        #region Methods

        /// <summary>
        /// Initialises a new CustomActionPage1.
        /// </summary>
        /// <param name="wizard">The ArtifactWizardForm object.</param>
        /// <param name="model">The CustomActionPresentationModel object.</param>
        public CustomActionPage1(ArtifactWizardForm wizard, CustomActionPresentationModel model)
            : base(wizard)
        {
            _model = model;
            InitializeComponent();
            base.LoadSettings();
            base.Headline = Resources.CustomActionPage1_StepTitle;
            base.HelpKeyword = "VS.SharePointTools.SPE.CustomAction";
        }

        /// <summary>
        /// Load the settings from the presentation model.
        /// </summary>
        protected override void LoadSettingsFromPresentationModel()
        {
            base.Skippable = _model.IsOptional;
            txtId.Text = _model.Id;
            txtGroupId.Text = _model.GroupId;
            txtDescription.Text = _model.Description;
            txtTitle.Text = _model.Title;
            txtSequence.Text = _model.Sequence.ToString();
            txtLocation.Text = _model.Location;
            txtImageUrl.Text = _model.ImageUrl;
        }

        /// <summary>
        /// Do the databinding from the model to the form components.
        /// </summary>
        /// <returns>True</returns>
        public override bool OnActivate()
        {
            //Do any databinding required here
            return base.OnActivate();
        }

        /// <summary>
        /// Apply the changes to the model and invoke its save changes.
        /// </summary>
        protected override void ApplyChangesToPresentationModel()
        {
            _model.Description = txtDescription.Text;
            _model.GroupId = txtGroupId.Text;
            _model.Id = txtId.Text;
            _model.ImageUrl = txtImageUrl.Text;
            _model.Location = txtLocation.Text;
            if (!String.IsNullOrEmpty(txtSequence.Text))
            {
                _model.Sequence = Convert.ToInt32(txtSequence.Text);
            }
            _model.Title = txtTitle.Text;
            _model.SaveChanges();
        }

        /// <summary>
        /// Onload to configure the form.
        /// </summary>
        /// <param name="e">The EventArgs object.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Set the label text
            lblIntroduction.Text = Resources.CustomActionPage1_Introduction;
            lblRequired.Text = Resources.CustomActionPage1_Required;
            lnkMSDNArticle.Text = Resources.CustomActionPage1_MSDNLinkText;

            LinkLabel.Link link = new LinkLabel.Link();
            link.Description = Resources.CustomActionPage1_MSDNLinkText;
            link.LinkData = Resources.CustomActionPage1_MSDNLinkUrl;
            link.Enabled = true;
            link.Name = "lnkMSDN";
            lnkMSDNArticle.Links.Add(link);

            lblId.Text = Resources.CustomActionPage1_ID;
            lblTitle.Text = Resources.CustomActionPage1_Title;
            lblDescription.Text = Resources.CustomActionPage1_Description;
            lblLocation.Text = Resources.CustomActionPage1_Location;
            lblSequence.Text = Resources.CustomActionPage1_Sequence;
            lblGroupId.Text = Resources.CustomActionPage1_GroupId;
            lblImageUrl.Text = Resources.CustomActionPage1_ImageUrl;
            btnGenerateGuid.Text = Resources.CustomActionPage1_GenerateGuid;
        }

        /// <summary>
        /// Is the page valid?
        /// </summary>
        /// <returns>The result of the validation checks.</returns>
        protected override bool IsCompletelyValid()
        {
            return base.IsCompletelyValid()
                    && ValidateId()
                    && ValidateTitle()
                    && ValidateDescription()
                    && ValidateLocation()
                    && ValidateSequence()
                    && ValidateGroupId()
                    && ValidateImageUrl();
        }

        /// <summary>
        /// Update the model on deactivate.
        /// </summary>
        /// <returns>The result.</returns>
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
        /// Validate the Id.
        /// </summary>
        /// <returns>True if the Id is valid.</returns>
        protected virtual bool ValidateId()
        {
            return true;
        }

        /// <summary>
        /// Validate the Title.
        /// </summary>
        /// <returns>True if the Title is valid.</returns>
        protected virtual bool ValidateTitle()
        {
            return !String.IsNullOrEmpty(txtTitle.Text);
        }

        /// <summary>
        /// Validate the Description.
        /// </summary>
        /// <returns>True if the Description is valid.</returns>
        protected virtual bool ValidateDescription()
        {
            return true;
        }

        /// <summary>
        /// Validate the Location.
        /// </summary>
        /// <returns>True if the Location is valid.</returns>
        protected virtual bool ValidateLocation()
        {
            return true;// !String.IsNullOrEmpty(txtLocation.Text);
        }

        /// <summary>
        /// Validate the Sequence.
        /// </summary>
        /// <returns>True if the Sequence is valid.</returns>
        protected virtual bool ValidateSequence()
        {
            if (!String.IsNullOrEmpty(txtSequence.Text))
            {
                int result = -1;
                return Int32.TryParse(txtSequence.Text, out result);
            }
            return true;
        }

        /// <summary>
        /// Validate the Group Id.
        /// </summary>
        /// <returns>True if the Title is valid.</returns>
        protected virtual bool ValidateGroupId()
        {
            return true;
        }

        /// <summary>
        /// Validate the Image Url.
        /// </summary>
        /// <returns>True if the ImageUrl is valid.</returns>
        protected virtual bool ValidateImageUrl()
        {
            return true;
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
        /// Validate the title textbox entry.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The EventArgs object.</param>
        private void txtTitle_Validated(object sender, EventArgs e)
        {
            if (ValidateTitle())
            {
                // Clear the error, if any, in the error provider.
                errMessages.SetError(txtTitle, "");
            }
            else
            {
                // Set the error if the name is not valid.
                errMessages.SetError(txtTitle, Resources.CustomActionPage1_Title_Error);
            }
        }

        /// <summary>
        /// Validate the sequence textbox entry.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The EventArgs object.</param>
        private void txtSequence_Validated(object sender, EventArgs e)
        {
            if (ValidateSequence())
            {
                // Clear the error, if any, in the error provider.
                errMessages.SetError(txtSequence, "");
            }
            else
            {
                // Set the error if the sequence is not valid.
                errMessages.SetError(txtSequence, Resources.CustomActionPage1_Sequence_Error);
            }
        }

        /// <summary>
        /// Generate a new guid for the Id field.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The EventArgs.</param>
        private void btnGenerateGuid_Click(object sender, EventArgs e)
        {
            txtId.Text = Guid.NewGuid().ToString("D");
        }
        
        #endregion
        
        #region Designer Generated

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.errMessages = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblGroupId = new System.Windows.Forms.Label();
            this.txtGroupId = new System.Windows.Forms.TextBox();
            this.lblImageUrl = new System.Windows.Forms.Label();
            this.txtImageUrl = new System.Windows.Forms.TextBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblIntroduction = new System.Windows.Forms.Label();
            this.lnkMSDNArticle = new System.Windows.Forms.LinkLabel();
            this.lblRequired = new System.Windows.Forms.Label();
            this.lblId = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblSequence = new System.Windows.Forms.Label();
            this.txtSequence = new System.Windows.Forms.TextBox();
            this.btnGenerateGuid = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errMessages)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // infoPanel
            // 
            this.infoPanel.BackColor = System.Drawing.SystemColors.Control;
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.None;
            this.infoPanel.Location = new System.Drawing.Point(0, 180);
            this.infoPanel.Size = new System.Drawing.Size(505, 304);
            // 
            // errMessages
            // 
            this.errMessages.ContainerControl = this;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblDescription, 2);
            this.lblDescription.Location = new System.Drawing.Point(3, 136);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 74;
            this.lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtDescription, 2);
            this.txtDescription.Location = new System.Drawing.Point(3, 152);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(475, 20);
            this.txtDescription.TabIndex = 3;
            // 
            // lblGroupId
            // 
            this.lblGroupId.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblGroupId, 2);
            this.lblGroupId.Location = new System.Drawing.Point(3, 179);
            this.lblGroupId.Name = "lblGroupId";
            this.lblGroupId.Size = new System.Drawing.Size(51, 13);
            this.lblGroupId.TabIndex = 76;
            this.lblGroupId.Text = "Group Id:";
            // 
            // txtGroupId
            // 
            this.txtGroupId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtGroupId, 2);
            this.txtGroupId.Location = new System.Drawing.Point(3, 195);
            this.txtGroupId.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtGroupId.Name = "txtGroupId";
            this.txtGroupId.Size = new System.Drawing.Size(475, 20);
            this.txtGroupId.TabIndex = 4;
            // 
            // lblImageUrl
            // 
            this.lblImageUrl.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblImageUrl, 2);
            this.lblImageUrl.Location = new System.Drawing.Point(3, 222);
            this.lblImageUrl.Name = "lblImageUrl";
            this.lblImageUrl.Size = new System.Drawing.Size(55, 13);
            this.lblImageUrl.TabIndex = 78;
            this.lblImageUrl.Text = "Image Url:";
            // 
            // txtImageUrl
            // 
            this.txtImageUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtImageUrl, 2);
            this.txtImageUrl.Location = new System.Drawing.Point(3, 238);
            this.txtImageUrl.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtImageUrl.Name = "txtImageUrl";
            this.txtImageUrl.Size = new System.Drawing.Size(475, 20);
            this.txtImageUrl.TabIndex = 5;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblLocation, 2);
            this.lblLocation.Location = new System.Drawing.Point(3, 265);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 80;
            this.lblLocation.Text = "Location:";
            // 
            // txtLocation
            // 
            this.txtLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtLocation, 2);
            this.txtLocation.Location = new System.Drawing.Point(3, 281);
            this.txtLocation.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(475, 20);
            this.txtLocation.TabIndex = 6;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.lblIntroduction, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lnkMSDNArticle, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblRequired, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblId, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtId, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblTitle, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.txtTitle, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.lblDescription, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.txtDescription, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.lblGroupId, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.txtGroupId, 0, 15);
            this.tableLayoutPanel1.Controls.Add(this.lblImageUrl, 0, 17);
            this.tableLayoutPanel1.Controls.Add(this.txtImageUrl, 0, 18);
            this.tableLayoutPanel1.Controls.Add(this.lblLocation, 0, 20);
            this.tableLayoutPanel1.Controls.Add(this.txtLocation, 0, 21);
            this.tableLayoutPanel1.Controls.Add(this.lblSequence, 0, 23);
            this.tableLayoutPanel1.Controls.Add(this.txtSequence, 0, 24);
            this.tableLayoutPanel1.Controls.Add(this.btnGenerateGuid, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 26;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(508, 389);
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
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblId, 2);
            this.lblId.Location = new System.Drawing.Point(3, 50);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(19, 13);
            this.lblId.TabIndex = 70;
            this.lblId.Text = "Id:";
            // 
            // txtId
            // 
            this.txtId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtId.Location = new System.Drawing.Point(3, 66);
            this.txtId.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(322, 20);
            this.txtId.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblTitle, 2);
            this.lblTitle.Location = new System.Drawing.Point(3, 93);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(30, 13);
            this.lblTitle.TabIndex = 72;
            this.lblTitle.Text = "Title:";
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtTitle, 2);
            this.txtTitle.Location = new System.Drawing.Point(3, 109);
            this.txtTitle.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(475, 20);
            this.txtTitle.TabIndex = 2;
            this.txtTitle.Validated += new System.EventHandler(this.txtTitle_Validated);
            // 
            // lblSequence
            // 
            this.lblSequence.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblSequence, 2);
            this.lblSequence.Location = new System.Drawing.Point(3, 308);
            this.lblSequence.Name = "lblSequence";
            this.lblSequence.Size = new System.Drawing.Size(59, 13);
            this.lblSequence.TabIndex = 82;
            this.lblSequence.Text = "Sequence:";
            // 
            // txtSequence
            // 
            this.txtSequence.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtSequence, 2);
            this.txtSequence.Location = new System.Drawing.Point(3, 324);
            this.txtSequence.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtSequence.Name = "txtSequence";
            this.txtSequence.Size = new System.Drawing.Size(475, 20);
            this.txtSequence.TabIndex = 7;
            this.txtSequence.Validated += new System.EventHandler(this.txtSequence_Validated);
            // 
            // btnGenerateGuid
            // 
            this.btnGenerateGuid.Location = new System.Drawing.Point(358, 66);
            this.btnGenerateGuid.Name = "btnGenerateGuid";
            this.btnGenerateGuid.Size = new System.Drawing.Size(93, 20);
            this.btnGenerateGuid.TabIndex = 1;
            this.btnGenerateGuid.Text = "Generate Guid";
            this.btnGenerateGuid.UseVisualStyleBackColor = true;
            this.btnGenerateGuid.Click += new System.EventHandler(this.btnGenerateGuid_Click);
            // 
            // CustomActionPage1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CustomActionPage1";
            this.Size = new System.Drawing.Size(508, 487);
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

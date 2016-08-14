using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models;
using CKS.Dev.VisualStudio.SharePoint.Properties;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Pages
{
    /// <summary>
    /// The third page of the Custom Action wizard.
    /// </summary>
    class CustomActionPage3 : BaseWizardPage
    {
        #region Controls

        private System.Windows.Forms.ErrorProvider errMessages;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtUrlAction;
        private System.Windows.Forms.Label lblUrlAction;
        private System.Windows.Forms.TextBox txtControlSrc;
        private System.Windows.Forms.Label lblControlSrc;
        private System.Windows.Forms.TextBox txtControlClass;
        private System.Windows.Forms.Label lblControlClass;
        private System.Windows.Forms.TextBox txtControlAssembly;
        private System.Windows.Forms.Label lblControlAssembly;
        private System.Windows.Forms.Label lblIntroduction;
        private System.Windows.Forms.LinkLabel lnkMSDNArticle;
        private System.Windows.Forms.Label lblRequired;
        private System.ComponentModel.IContainer components;

        #endregion

        #region Fields

        /// <summary>
        /// Field to hold the presentation model
        /// </summary>
        CustomActionPresentationModel _model;

        #endregion

        #region Methods

        /// <summary>
        /// Initialises a new HideCustomActionPage
        /// </summary>
        /// <param name="wizard">The ArtifactWizardForm object</param>
        /// <param name="model">The CustomActionPresentationModel object</param>
        public CustomActionPage3(ArtifactWizardForm wizard, CustomActionPresentationModel model)
            : base(wizard)
        {
            _model = model;
            InitializeComponent();
            base.LoadSettings();
            base.Headline = Resources.CustomActionPage3_StepTitle;
            base.HelpKeyword = "VS.SharePointTools.SPE.CustomAction";
        }

        /// <summary>
        /// Load the settings from the presentation model
        /// </summary>
        protected override void LoadSettingsFromPresentationModel()
        {
            base.Skippable = _model.IsOptional;
            txtControlAssembly.Text = _model.ControlAssembly;
            txtControlClass.Text = _model.ControlClass;
            txtControlSrc.Text = _model.ControlSrc;
            txtUrlAction.Text = _model.UrlAction;
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
        /// Onload to configure the form.
        /// </summary>
        /// <param name="e">The EventArgs object.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Set the label text
            lblIntroduction.Text = Resources.CustomActionPage3_Introduction;
            lblRequired.Text = Resources.CustomActionPage3_Required;
            lnkMSDNArticle.Text = Resources.CustomActionPage3_MSDNLinkText;

            System.Windows.Forms.LinkLabel.Link link = new System.Windows.Forms.LinkLabel.Link();
            link.Description = Resources.CustomActionPage3_MSDNLinkText;
            link.LinkData = Resources.CustomActionPage3_MSDNLinkUrl;
            link.Enabled = true;
            link.Name = "lnkMSDN";
            lnkMSDNArticle.Links.Add(link);

            lblControlAssembly.Text = Resources.CustomActionPage3_ControlAssembly;
            lblControlClass.Text = Resources.CustomActionPage3_ControlClass;
            lblControlSrc.Text = Resources.CustomActionPage3_ControlSrc;
            lblUrlAction.Text = Resources.CustomActionPage3_UrlAction;
        }

        /// <summary>
        /// Is the page valid?
        /// </summary>
        /// <returns>The result of the validation checks.</returns>
        protected override bool IsCompletelyValid()
        {
            return base.IsCompletelyValid()
                    && ValidateControlAssembly()
                    && ValidateControlClass()
                    && ValidateControlSrc()
                    && ValidateUrlAction();
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
        /// Apply the changes to the model and invoke its save changes.
        /// </summary>
        protected override void ApplyChangesToPresentationModel()
        {
            _model.ControlAssembly = txtControlAssembly.Text;
            _model.ControlClass = txtControlClass.Text;
            _model.ControlSrc = txtControlSrc.Text;
            _model.UrlAction = txtUrlAction.Text;
            _model.SaveChanges();
        }

        /// <summary>
        /// Validate the ControlAssembly.
        /// </summary>
        /// <returns>True if the ControlAssembly is valid.</returns>
        protected virtual bool ValidateControlAssembly()
        {
            return true;
        }

        /// <summary>
        /// Validate the ControlClass.
        /// </summary>
        /// <returns>True if the ControlClass is valid.</returns>
        protected virtual bool ValidateControlClass()
        {
            return true;
        }

        /// <summary>
        /// Validate the ControlSrc.
        /// </summary>
        /// <returns>True if the ControlSrc is valid.</returns>
        protected virtual bool ValidateControlSrc()
        {
            return true;
        }

        /// <summary>
        /// Validate the UrlAction.
        /// </summary>
        /// <returns>True if the UrlAction is valid.</returns>
        protected virtual bool ValidateUrlAction()
        {
            return true;
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
            this.errMessages = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtUrlAction = new System.Windows.Forms.TextBox();
            this.lblUrlAction = new System.Windows.Forms.Label();
            this.txtControlSrc = new System.Windows.Forms.TextBox();
            this.lblControlSrc = new System.Windows.Forms.Label();
            this.txtControlClass = new System.Windows.Forms.TextBox();
            this.lblControlClass = new System.Windows.Forms.Label();
            this.txtControlAssembly = new System.Windows.Forms.TextBox();
            this.lblControlAssembly = new System.Windows.Forms.Label();
            this.lblIntroduction = new System.Windows.Forms.Label();
            this.lnkMSDNArticle = new System.Windows.Forms.LinkLabel();
            this.lblRequired = new System.Windows.Forms.Label();
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.txtUrlAction, 0, 15);
            this.tableLayoutPanel1.Controls.Add(this.lblUrlAction, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.txtControlSrc, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.lblControlSrc, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.txtControlClass, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.lblControlClass, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.txtControlAssembly, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblControlAssembly, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblIntroduction, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lnkMSDNArticle, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblRequired, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 18;
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(508, 389);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // txtUrlAction
            // 
            this.txtUrlAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtUrlAction, 2);
            this.txtUrlAction.Location = new System.Drawing.Point(3, 195);
            this.txtUrlAction.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtUrlAction.Name = "txtUrlAction";
            this.txtUrlAction.Size = new System.Drawing.Size(475, 20);
            this.txtUrlAction.TabIndex = 3;
            // 
            // lblUrlAction
            // 
            this.lblUrlAction.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblUrlAction, 2);
            this.lblUrlAction.Location = new System.Drawing.Point(3, 179);
            this.lblUrlAction.Name = "lblUrlAction";
            this.lblUrlAction.Size = new System.Drawing.Size(56, 13);
            this.lblUrlAction.TabIndex = 66;
            this.lblUrlAction.Text = "Url Action:";
            // 
            // txtControlSrc
            // 
            this.txtControlSrc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtControlSrc, 2);
            this.txtControlSrc.Location = new System.Drawing.Point(3, 152);
            this.txtControlSrc.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtControlSrc.Name = "txtControlSrc";
            this.txtControlSrc.Size = new System.Drawing.Size(475, 20);
            this.txtControlSrc.TabIndex = 2;
            // 
            // lblControlSrc
            // 
            this.lblControlSrc.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblControlSrc, 2);
            this.lblControlSrc.Location = new System.Drawing.Point(3, 136);
            this.lblControlSrc.Name = "lblControlSrc";
            this.lblControlSrc.Size = new System.Drawing.Size(62, 13);
            this.lblControlSrc.TabIndex = 17;
            this.lblControlSrc.Text = "Control Src:";
            // 
            // txtControlClass
            // 
            this.txtControlClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtControlClass, 2);
            this.txtControlClass.Location = new System.Drawing.Point(3, 109);
            this.txtControlClass.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtControlClass.Name = "txtControlClass";
            this.txtControlClass.Size = new System.Drawing.Size(475, 20);
            this.txtControlClass.TabIndex = 1;
            // 
            // lblControlClass
            // 
            this.lblControlClass.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblControlClass, 2);
            this.lblControlClass.Location = new System.Drawing.Point(3, 93);
            this.lblControlClass.Name = "lblControlClass";
            this.lblControlClass.Size = new System.Drawing.Size(71, 13);
            this.lblControlClass.TabIndex = 16;
            this.lblControlClass.Text = "Control Class:";
            // 
            // txtControlAssembly
            // 
            this.txtControlAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtControlAssembly, 2);
            this.txtControlAssembly.Location = new System.Drawing.Point(3, 66);
            this.txtControlAssembly.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtControlAssembly.Name = "txtControlAssembly";
            this.txtControlAssembly.Size = new System.Drawing.Size(475, 20);
            this.txtControlAssembly.TabIndex = 0;
            // 
            // lblControlAssembly
            // 
            this.lblControlAssembly.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblControlAssembly, 2);
            this.lblControlAssembly.Location = new System.Drawing.Point(3, 50);
            this.lblControlAssembly.Name = "lblControlAssembly";
            this.lblControlAssembly.Size = new System.Drawing.Size(90, 13);
            this.lblControlAssembly.TabIndex = 15;
            this.lblControlAssembly.Text = "Control Assembly:";
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
            // CustomActionPage3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CustomActionPage3";
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

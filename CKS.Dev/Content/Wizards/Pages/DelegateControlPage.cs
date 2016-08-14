using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Pages
{
    class DelegateControlPage : BaseWizardPage
    {
        #region Controls

        private System.Windows.Forms.ErrorProvider errMessages;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtControlSrc;
        private System.Windows.Forms.Label lblControlSrc;
        private System.Windows.Forms.TextBox txtControlClass;
        private System.Windows.Forms.Label lblControlClass;
        private System.Windows.Forms.TextBox txtControlAssembly;
        private System.Windows.Forms.Label lblControlAssembly;
        private System.Windows.Forms.TextBox txtSequence;
        private System.Windows.Forms.Label lblSequence;
        private System.Windows.Forms.Label lblIntroduction;
        private System.Windows.Forms.LinkLabel lnkMSDNArticle;
        private System.Windows.Forms.Label lblRequired;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.TextBox txtId;

        #endregion

        #region Fields

        /// <summary>
        /// Field to hold the presentation model
        /// </summary>
        DelegateControlPresentationModel _model;

        #endregion

        #region Methods

        /// <summary>
        /// Initialises a new DelegateControlPage
        /// </summary>
        /// <param name="wizard">The ArtifactWizardForm object</param>
        /// <param name="model">The DelegateControlPresentationModel object</param>
        public DelegateControlPage(ArtifactWizardForm wizard, DelegateControlPresentationModel model)
            : base(wizard)
        {
            _model = model;
            InitializeComponent();
            base.LoadSettings();
            base.Headline = Resources.DelegateControlPage_StepTitle;
            base.HelpKeyword = "VS.SharePointTools.SPE.DelegateControl";
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
            txtId.Text = _model.Id;
            txtSequence.Text = _model.Sequence.ToString();
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Set the label text
            lblIntroduction.Text = Resources.DelegateControlPage_Introduction;
            lblRequired.Text = Resources.DelegateControlPage_Required;
            lnkMSDNArticle.Text = Resources.DelegateControlPage_MSDNLinkText;

            System.Windows.Forms.LinkLabel.Link link = new System.Windows.Forms.LinkLabel.Link();
            link.Description = Resources.DelegateControlPage_MSDNLinkText;
            link.LinkData = Resources.DelegateControlPage_MSDNLinkUrl;
            link.Enabled = true;
            link.Name = "lnkMSDN";
            lnkMSDNArticle.Links.Add(link);

            lblId.Text = Resources.DelegateControlPage_ID;
            lblControlAssembly.Text = Resources.DelegateControlPage_ControlAssembly;
            lblControlClass.Text = Resources.DelegateControlPage_ControlClass;
            lblControlSrc.Text = Resources.DelegateControlPage_ControlSrc;
            lblSequence.Text = Resources.DelegateControlPage_Sequence;
        }

        protected override bool IsCompletelyValid()
        {
            return base.IsCompletelyValid()
                    && ValidateId()
                    && ValidateControlAssembly()
                    && ValidateControlClass()
                    && ValidateControlSrc()
                    && ValidateSequence();
        }

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
            _model.Id = txtId.Text;
            if (!String.IsNullOrWhiteSpace(txtSequence.Text))
            {
                _model.Sequence = Convert.ToInt32(txtSequence.Text);
            }
            _model.ControlAssembly = txtControlAssembly.Text;
            _model.ControlClass = txtControlClass.Text;
            _model.ControlSrc = txtControlSrc.Text;
            _model.SaveChanges();
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
        /// Validate the ControlAssembly
        /// </summary>
        /// <returns>True if the ControlAssembly is valid</returns>
        protected virtual bool ValidateControlAssembly()
        {
            return true;
        }

        /// <summary>
        /// Validate the ControlClass
        /// </summary>
        /// <returns>True if the ControlClass is valid</returns>
        protected virtual bool ValidateControlClass()
        {
            return true;
        }

        /// <summary>
        /// Validate the ControlSrc
        /// </summary>
        /// <returns>True if the ControlSrc is valid</returns>
        protected virtual bool ValidateControlSrc()
        {
            return true;
        }

        /// <summary>
        /// Validate the Sequence
        /// </summary>
        /// <returns>True if the Sequence is valid</returns>
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
                // Set the error if the name is not valid.
                errMessages.SetError(txtSequence, Resources.DelegateControlPage_Sequence_Error);
            }
        }

        #endregion

        #region Designer Generated

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.errMessages = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtControlSrc = new System.Windows.Forms.TextBox();
            this.lblControlSrc = new System.Windows.Forms.Label();
            this.txtControlClass = new System.Windows.Forms.TextBox();
            this.lblControlClass = new System.Windows.Forms.Label();
            this.txtControlAssembly = new System.Windows.Forms.TextBox();
            this.lblControlAssembly = new System.Windows.Forms.Label();
            this.txtSequence = new System.Windows.Forms.TextBox();
            this.lblSequence = new System.Windows.Forms.Label();
            this.lblIntroduction = new System.Windows.Forms.Label();
            this.lnkMSDNArticle = new System.Windows.Forms.LinkLabel();
            this.lblRequired = new System.Windows.Forms.Label();
            this.lblId = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errMessages)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // infoPanel
            // 
            this.infoPanel.BackColor = System.Drawing.SystemColors.Control;
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.None;
            this.infoPanel.Location = new System.Drawing.Point(0, 50);
            this.infoPanel.Size = new System.Drawing.Size(505, 389);
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
            this.tableLayoutPanel1.Controls.Add(this.txtControlSrc, 0, 18);
            this.tableLayoutPanel1.Controls.Add(this.lblControlSrc, 0, 17);
            this.tableLayoutPanel1.Controls.Add(this.txtControlClass, 0, 15);
            this.tableLayoutPanel1.Controls.Add(this.lblControlClass, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.txtControlAssembly, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.lblControlAssembly, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.txtSequence, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.lblSequence, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblIntroduction, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lnkMSDNArticle, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblRequired, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblId, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtId, 0, 6);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(508, 372);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // txtControlSrc
            // 
            this.txtControlSrc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtControlSrc, 2);
            this.txtControlSrc.Location = new System.Drawing.Point(3, 238);
            this.txtControlSrc.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtControlSrc.Name = "txtControlSrc";
            this.txtControlSrc.Size = new System.Drawing.Size(475, 20);
            this.txtControlSrc.TabIndex = 4;
            // 
            // lblControlSrc
            // 
            this.lblControlSrc.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblControlSrc, 2);
            this.lblControlSrc.Location = new System.Drawing.Point(3, 222);
            this.lblControlSrc.Name = "lblControlSrc";
            this.lblControlSrc.Size = new System.Drawing.Size(62, 13);
            this.lblControlSrc.TabIndex = 20;
            this.lblControlSrc.Text = "Control Src:";
            // 
            // txtControlClass
            // 
            this.txtControlClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtControlClass, 2);
            this.txtControlClass.Location = new System.Drawing.Point(3, 195);
            this.txtControlClass.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtControlClass.Name = "txtControlClass";
            this.txtControlClass.Size = new System.Drawing.Size(475, 20);
            this.txtControlClass.TabIndex = 3;
            // 
            // lblControlClass
            // 
            this.lblControlClass.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblControlClass, 2);
            this.lblControlClass.Location = new System.Drawing.Point(3, 179);
            this.lblControlClass.Name = "lblControlClass";
            this.lblControlClass.Size = new System.Drawing.Size(71, 13);
            this.lblControlClass.TabIndex = 18;
            this.lblControlClass.Text = "Control Class:";
            // 
            // txtControlAssembly
            // 
            this.txtControlAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtControlAssembly, 2);
            this.txtControlAssembly.Location = new System.Drawing.Point(3, 152);
            this.txtControlAssembly.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtControlAssembly.Name = "txtControlAssembly";
            this.txtControlAssembly.Size = new System.Drawing.Size(475, 20);
            this.txtControlAssembly.TabIndex = 2;
            // 
            // lblControlAssembly
            // 
            this.lblControlAssembly.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblControlAssembly, 2);
            this.lblControlAssembly.Location = new System.Drawing.Point(3, 136);
            this.lblControlAssembly.Name = "lblControlAssembly";
            this.lblControlAssembly.Size = new System.Drawing.Size(90, 13);
            this.lblControlAssembly.TabIndex = 17;
            this.lblControlAssembly.Text = "Control Assembly:";
            // 
            // txtSequence
            // 
            this.txtSequence.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtSequence, 2);
            this.txtSequence.Location = new System.Drawing.Point(3, 109);
            this.txtSequence.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtSequence.Name = "txtSequence";
            this.txtSequence.Size = new System.Drawing.Size(475, 20);
            this.txtSequence.TabIndex = 1;
            this.txtSequence.Validated += new System.EventHandler(this.txtSequence_Validated);
            // 
            // lblSequence
            // 
            this.lblSequence.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblSequence, 2);
            this.lblSequence.Location = new System.Drawing.Point(3, 93);
            this.lblSequence.MaximumSize = new System.Drawing.Size(470, 0);
            this.lblSequence.Name = "lblSequence";
            this.lblSequence.Size = new System.Drawing.Size(59, 13);
            this.lblSequence.TabIndex = 16;
            this.lblSequence.Text = "Sequence:";
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
            this.lnkMSDNArticle.TabIndex = 5;
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
            this.lblId.TabIndex = 97;
            this.lblId.Text = "Id:";
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
            this.txtId.TabIndex = 0;
            // 
            // DelegateControlPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DelegateControlPage";
            this.Size = new System.Drawing.Size(508, 442);
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


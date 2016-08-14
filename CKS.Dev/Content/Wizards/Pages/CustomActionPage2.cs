using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using System.Windows.Forms;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Dialogs;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Pages
{
    /// <summary>
    /// The second page of the Custom Action wizard.
    /// </summary>
    class CustomActionPage2 : BaseWizardPage
    {
        #region Controls

        private System.Windows.Forms.ErrorProvider errMessages;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblShowInSealedContentTypes;
        private Label lblShowInReadOnlyContentTypes;
        private TextBox txtRights;
        private Label lblRights;
        private Label lblRequireSiteAdministrator;
        private Label lblRegistrationType;
        private TextBox txtRegistrationId;
        private Label lblRegistrationId;
        private Label lblIntroduction;
        private LinkLabel lnkMSDNArticle;
        private Label lblRequired;
        private ComboBox cboShowInSealedContentTypes;
        private ComboBox cboShowInReadOnlyContentTypes;
        private ComboBox cboRequireSiteAdministrator;
        private ComboBox cboRegistrationType;
        private Button btnRightsPicker;
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
        /// Initialises a new CustomActionPage2.
        /// </summary>
        /// <param name="wizard">The ArtifactWizardForm object.</param>
        /// <param name="model">The CustomActionPresentationModel object.</param>
        public CustomActionPage2(ArtifactWizardForm wizard, CustomActionPresentationModel model)
            : base(wizard)
        {
            _model = model;
            InitializeComponent();
            base.LoadSettings();
            base.Headline = Resources.CustomActionPage2_StepTitle;
            base.HelpKeyword = "VS.SharePointTools.SPE.CustomAction";
        }

        /// <summary>
        /// Load the settings from the presentation model.
        /// </summary>
        protected override void LoadSettingsFromPresentationModel()
        {
            base.Skippable = _model.IsOptional;           
        }

        /// <summary>
        /// Do the databinding from the model to the form components.
        /// </summary>
        /// <returns>True</returns>
        public override bool OnActivate()
        {
            //Do any databinding required here
            //Registration Type
            cboRegistrationType.DataSource = this._model.AvailableRegistrationTypes;
            cboRegistrationType.DisplayMember = "DisplayMember";
            cboRegistrationType.ValueMember = "ValueMember";

            //Require Site Administrator
            cboRequireSiteAdministrator.DataSource = this._model.RequireSiteAdministratorValues;
            cboRequireSiteAdministrator.DisplayMember = "DisplayMember";
            cboRequireSiteAdministrator.ValueMember = "ValueMember";

            //Registration Type
            cboShowInReadOnlyContentTypes.DataSource = this._model.ShowInReadOnlyContentTypesValues;
            cboShowInReadOnlyContentTypes.DisplayMember = "DisplayMember";
            cboShowInReadOnlyContentTypes.ValueMember = "ValueMember";

            //Registration Type
            cboShowInSealedContentTypes.DataSource = this._model.ShowInSealedContentTypesValues;
            cboShowInSealedContentTypes.DisplayMember = "DisplayMember";
            cboShowInSealedContentTypes.ValueMember = "ValueMember";


            txtRegistrationId.Text = _model.RegistrationId;

            if (String.IsNullOrEmpty(_model.RegistrationType))
            {
                cboRegistrationType.SelectedIndex = 0;
            }
            else
            {
                cboRegistrationType.SelectedIndex = cboRegistrationType.FindStringExact(_model.RegistrationType);
            }

            if (_model.RequireSiteAdministrator == null)
            {
                cboRequireSiteAdministrator.SelectedIndex = 0;
            }
            else
            {
                cboRequireSiteAdministrator.SelectedIndex = cboRequireSiteAdministrator.FindStringExact(_model.RequireSiteAdministrator.ToString());
            }
            txtRights.Text = _model.Rights;

            if (_model.ShowInReadOnlyContentTypes == null)
            {
                cboShowInReadOnlyContentTypes.SelectedIndex = 0;
            }
            else
            {
                cboShowInReadOnlyContentTypes.SelectedIndex = cboShowInReadOnlyContentTypes.FindStringExact(_model.ShowInReadOnlyContentTypes.ToString());
            }

            if (_model.ShowInSealedContentTypes == null)
            {
                cboShowInSealedContentTypes.SelectedIndex = 0;
            }
            else
            {
                cboShowInSealedContentTypes.SelectedIndex = cboShowInSealedContentTypes.FindStringExact(_model.ShowInSealedContentTypes.ToString());
            }

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
            lblIntroduction.Text = Resources.CustomActionPage2_Introduction;
            lblRequired.Text = Resources.CustomActionPage2_Required;
            lnkMSDNArticle.Text = Resources.CustomActionPage2_MSDNLinkText;

            LinkLabel.Link link = new LinkLabel.Link();
            link.Description = Resources.CustomActionPage2_MSDNLinkText;
            link.LinkData = Resources.CustomActionPage2_MSDNLinkUrl;
            link.Enabled = true;
            link.Name = "lnkMSDN";
            lnkMSDNArticle.Links.Add(link);

            lblRegistrationId.Text = Resources.CustomActionPage2_RegistrationId;
            lblRegistrationType.Text = Resources.CustomActionPage2_RegistrationType;
            lblRequireSiteAdministrator.Text = Resources.CustomActionPage2_RequireSiteAdministrator;
            lblRights.Text = Resources.CustomActionPage2_Rights;
            lblShowInReadOnlyContentTypes.Text = Resources.CustomActionPage2_ShowInReadOnlyContentTypes;
            lblShowInSealedContentTypes.Text = Resources.CustomActionPage2_ShowInSealedContentTypes;
        }

        /// <summary>
        /// Is the page valid?
        /// </summary>
        /// <returns>The result of the validation checks.</returns>
        protected override bool IsCompletelyValid()
        {
            return base.IsCompletelyValid()
                    && ValidateRegistrationId()
                    && ValidateRegistrationType()
                    && ValidateRequireSiteAdministrator()
                    && ValidateRights()
                    && ValidateShowInReadOnlyContentTypes()
                    && ValidateShowInSealedContentTypes();
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
            _model.RegistrationId = txtRegistrationId.Text;

            if (cboRegistrationType.SelectedValue.ToString() != "-1")
            {
                _model.RegistrationType = cboRegistrationType.SelectedValue.ToString();
            }
            else
            {
                _model.RegistrationType = String.Empty;
            }
            if (cboRequireSiteAdministrator.SelectedValue.ToString() != "-1")
            {
                _model.RequireSiteAdministrator = Convert.ToBoolean(cboRequireSiteAdministrator.SelectedValue.ToString());
            }
            else
            {
                _model.RequireSiteAdministrator = null;
            }
            _model.Rights = txtRights.Text;
            if (cboShowInReadOnlyContentTypes.SelectedValue.ToString() != "-1")
            {
                _model.ShowInReadOnlyContentTypes = Convert.ToBoolean(cboShowInReadOnlyContentTypes.SelectedValue.ToString());
            }
            else
            {
                _model.ShowInReadOnlyContentTypes = null;
            }
            if (cboShowInSealedContentTypes.SelectedValue.ToString() != "-1")
            {
                _model.ShowInSealedContentTypes = Convert.ToBoolean(cboShowInSealedContentTypes.SelectedValue.ToString());
            }
            else
            {
                _model.ShowInSealedContentTypes = null;
            }
            _model.SaveChanges();
        }

        /// <summary>
        /// Validate the RegistrationId.
        /// </summary>
        /// <returns>True if the RegistrationId is valid.</returns>
        protected virtual bool ValidateRegistrationId()
        {
            return true;
        }

        /// <summary>
        /// Validate the RegistrationType.
        /// </summary>
        /// <returns>True if the RegistrationType is valid.</returns>
        protected virtual bool ValidateRegistrationType()
        {
            return true;
        }

        /// <summary>
        /// Validate the RequireSiteAdministrator.
        /// </summary>
        /// <returns>True if the RequireSiteAdministrator is valid.</returns>
        protected virtual bool ValidateRequireSiteAdministrator()
        {
            return true;
        }

        /// <summary>
        /// Validate the Rights.
        /// </summary>
        /// <returns>True if the Rights is valid.</returns>
        protected virtual bool ValidateRights()
        {
            return true;
        }

        /// <summary>
        /// Validate the ShowInReadOnlyContentTypes.
        /// </summary>
        /// <returns>True if the ShowInReadOnlyContentTypes is valid.</returns>
        protected virtual bool ValidateShowInReadOnlyContentTypes()
        {
            return true;
        }

        /// <summary>
        /// Validate the ShowInSealedContentTypes.
        /// </summary>
        /// <returns>True if the ShowInSealedContentTypes is valid.</returns>
        protected virtual bool ValidateShowInSealedContentTypes()
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
        /// Launch the permissions dialog.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The EventArgs object.</param>
        private void btnRightsPicker_Click(object sender, EventArgs e)
        {
            SPBasePermissionsPickerDialog form = new SPBasePermissionsPickerDialog(txtRights.Text);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog(this);

            if (form.DialogResult == DialogResult.OK)
            {
                txtRights.Text = form.SelectedBasePermissions;
            }
        }

        #endregion
        
        #region Designer Generated

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.errMessages = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblShowInSealedContentTypes = new System.Windows.Forms.Label();
            this.lblShowInReadOnlyContentTypes = new System.Windows.Forms.Label();
            this.txtRights = new System.Windows.Forms.TextBox();
            this.lblRights = new System.Windows.Forms.Label();
            this.lblRequireSiteAdministrator = new System.Windows.Forms.Label();
            this.lblRegistrationType = new System.Windows.Forms.Label();
            this.txtRegistrationId = new System.Windows.Forms.TextBox();
            this.lblRegistrationId = new System.Windows.Forms.Label();
            this.lblIntroduction = new System.Windows.Forms.Label();
            this.lnkMSDNArticle = new System.Windows.Forms.LinkLabel();
            this.lblRequired = new System.Windows.Forms.Label();
            this.cboRequireSiteAdministrator = new System.Windows.Forms.ComboBox();
            this.cboRegistrationType = new System.Windows.Forms.ComboBox();
            this.btnRightsPicker = new System.Windows.Forms.Button();
            this.cboShowInReadOnlyContentTypes = new System.Windows.Forms.ComboBox();
            this.cboShowInSealedContentTypes = new System.Windows.Forms.ComboBox();
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
            this.tableLayoutPanel1.Controls.Add(this.lblShowInSealedContentTypes, 0, 20);
            this.tableLayoutPanel1.Controls.Add(this.lblShowInReadOnlyContentTypes, 0, 17);
            this.tableLayoutPanel1.Controls.Add(this.txtRights, 0, 15);
            this.tableLayoutPanel1.Controls.Add(this.lblRights, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.lblRequireSiteAdministrator, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.lblRegistrationType, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.txtRegistrationId, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblRegistrationId, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblIntroduction, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lnkMSDNArticle, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblRequired, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.cboRequireSiteAdministrator, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.cboRegistrationType, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.btnRightsPicker, 1, 15);
            this.tableLayoutPanel1.Controls.Add(this.cboShowInReadOnlyContentTypes, 0, 18);
            this.tableLayoutPanel1.Controls.Add(this.cboShowInSealedContentTypes, 0, 21);
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
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // lblShowInSealedContentTypes
            // 
            this.lblShowInSealedContentTypes.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblShowInSealedContentTypes, 2);
            this.lblShowInSealedContentTypes.Location = new System.Drawing.Point(3, 271);
            this.lblShowInSealedContentTypes.Name = "lblShowInSealedContentTypes";
            this.lblShowInSealedContentTypes.Size = new System.Drawing.Size(157, 13);
            this.lblShowInSealedContentTypes.TabIndex = 52;
            this.lblShowInSealedContentTypes.Text = "Show In Sealed Content Types:";
            // 
            // lblShowInReadOnlyContentTypes
            // 
            this.lblShowInReadOnlyContentTypes.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblShowInReadOnlyContentTypes, 2);
            this.lblShowInReadOnlyContentTypes.Location = new System.Drawing.Point(3, 227);
            this.lblShowInReadOnlyContentTypes.Name = "lblShowInReadOnlyContentTypes";
            this.lblShowInReadOnlyContentTypes.Size = new System.Drawing.Size(174, 13);
            this.lblShowInReadOnlyContentTypes.TabIndex = 50;
            this.lblShowInReadOnlyContentTypes.Text = "Show In Read Only Content Types:";
            // 
            // txtRights
            // 
            this.txtRights.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRights.Location = new System.Drawing.Point(3, 197);
            this.txtRights.Name = "txtRights";
            this.txtRights.Size = new System.Drawing.Size(349, 20);
            this.txtRights.TabIndex = 3;
            // 
            // lblRights
            // 
            this.lblRights.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblRights, 2);
            this.lblRights.Location = new System.Drawing.Point(3, 181);
            this.lblRights.Name = "lblRights";
            this.lblRights.Size = new System.Drawing.Size(40, 13);
            this.lblRights.TabIndex = 48;
            this.lblRights.Text = "Rights:";
            // 
            // lblRequireSiteAdministrator
            // 
            this.lblRequireSiteAdministrator.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblRequireSiteAdministrator, 2);
            this.lblRequireSiteAdministrator.Location = new System.Drawing.Point(3, 137);
            this.lblRequireSiteAdministrator.Name = "lblRequireSiteAdministrator";
            this.lblRequireSiteAdministrator.Size = new System.Drawing.Size(131, 13);
            this.lblRequireSiteAdministrator.TabIndex = 47;
            this.lblRequireSiteAdministrator.Text = "Require Site Administrator:";
            // 
            // lblRegistrationType
            // 
            this.lblRegistrationType.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblRegistrationType, 2);
            this.lblRegistrationType.Location = new System.Drawing.Point(3, 93);
            this.lblRegistrationType.Name = "lblRegistrationType";
            this.lblRegistrationType.Size = new System.Drawing.Size(93, 13);
            this.lblRegistrationType.TabIndex = 30;
            this.lblRegistrationType.Text = "Registration Type:";
            // 
            // txtRegistrationId
            // 
            this.txtRegistrationId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtRegistrationId, 2);
            this.txtRegistrationId.Location = new System.Drawing.Point(3, 66);
            this.txtRegistrationId.Name = "txtRegistrationId";
            this.txtRegistrationId.Size = new System.Drawing.Size(502, 20);
            this.txtRegistrationId.TabIndex = 0;
            // 
            // lblRegistrationId
            // 
            this.lblRegistrationId.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblRegistrationId, 2);
            this.lblRegistrationId.Location = new System.Drawing.Point(3, 50);
            this.lblRegistrationId.Name = "lblRegistrationId";
            this.lblRegistrationId.Size = new System.Drawing.Size(78, 13);
            this.lblRegistrationId.TabIndex = 28;
            this.lblRegistrationId.Text = "Registration Id:";
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
            // cboRequireSiteAdministrator
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cboRequireSiteAdministrator, 2);
            this.cboRequireSiteAdministrator.FormattingEnabled = true;
            this.cboRequireSiteAdministrator.Location = new System.Drawing.Point(3, 153);
            this.cboRequireSiteAdministrator.Name = "cboRequireSiteAdministrator";
            this.cboRequireSiteAdministrator.Size = new System.Drawing.Size(121, 21);
            this.cboRequireSiteAdministrator.TabIndex = 2;
            // 
            // cboRegistrationType
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cboRegistrationType, 2);
            this.cboRegistrationType.FormattingEnabled = true;
            this.cboRegistrationType.Location = new System.Drawing.Point(3, 109);
            this.cboRegistrationType.Name = "cboRegistrationType";
            this.cboRegistrationType.Size = new System.Drawing.Size(121, 21);
            this.cboRegistrationType.TabIndex = 1;
            // 
            // btnRightsPicker
            // 
            this.btnRightsPicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRightsPicker.Location = new System.Drawing.Point(358, 197);
            this.btnRightsPicker.Name = "btnRightsPicker";
            this.btnRightsPicker.Size = new System.Drawing.Size(25, 23);
            this.btnRightsPicker.TabIndex = 4;
            this.btnRightsPicker.Text = "...";
            this.btnRightsPicker.UseVisualStyleBackColor = true;
            this.btnRightsPicker.Click += new System.EventHandler(this.btnRightsPicker_Click);
            // 
            // cboShowInReadOnlyContentTypes
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cboShowInReadOnlyContentTypes, 2);
            this.cboShowInReadOnlyContentTypes.FormattingEnabled = true;
            this.cboShowInReadOnlyContentTypes.Location = new System.Drawing.Point(3, 243);
            this.cboShowInReadOnlyContentTypes.Name = "cboShowInReadOnlyContentTypes";
            this.cboShowInReadOnlyContentTypes.Size = new System.Drawing.Size(121, 21);
            this.cboShowInReadOnlyContentTypes.TabIndex = 5;
            // 
            // cboShowInSealedContentTypes
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cboShowInSealedContentTypes, 2);
            this.cboShowInSealedContentTypes.FormattingEnabled = true;
            this.cboShowInSealedContentTypes.Location = new System.Drawing.Point(3, 287);
            this.cboShowInSealedContentTypes.Name = "cboShowInSealedContentTypes";
            this.cboShowInSealedContentTypes.Size = new System.Drawing.Size(121, 21);
            this.cboShowInSealedContentTypes.TabIndex = 6;
            // 
            // CustomActionPage2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Location = new System.Drawing.Point(0, 180);
            this.Name = "CustomActionPage2";
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

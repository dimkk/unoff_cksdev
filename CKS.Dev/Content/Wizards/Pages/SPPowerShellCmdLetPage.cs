using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models;
using CKS.Dev.VisualStudio.SharePoint.Properties;
using System.Windows.Forms;
using System.Management.Automation;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Pages
{
    class SPPowerShellCmdLetPage : BaseWizardPage
    {
        #region Controls

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblIntroduction;
        private System.Windows.Forms.LinkLabel lnkMSDNArticle;
        private System.Windows.Forms.Label lblRequired;
        private System.Windows.Forms.Label lblVerb;
        private System.Windows.Forms.Label lblNoun;
        private System.Windows.Forms.TextBox txtNoun;
        private System.Windows.Forms.Label lblSPCmdletAttribute;
        private System.Windows.Forms.ComboBox cboVerb;
        private System.Windows.Forms.ComboBox cboCommonObjects;
        private System.Windows.Forms.CheckedListBox clbSPCmdletAttribute;
        private System.Windows.Forms.Label lblOtherObject;
        private System.Windows.Forms.Label lblObject;
        private System.Windows.Forms.TextBox txtOtherObject;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the presentation model
        /// </summary>
        protected SPPowerShellCmdLetModel CurrentModel
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialises a new SPPowerShellCmdLetPage
        /// </summary>
        /// <param name="wizard">The ArtifactWizardForm object</param>
        /// <param name="model">The SPPowerShellCmdLetModel object</param>
        public SPPowerShellCmdLetPage(ArtifactWizardForm wizard, SPPowerShellCmdLetModel model)
            : base(wizard)
        {
            CurrentModel = model;
            InitializeComponent();
            base.LoadSettings();
            base.Headline = Resources.SPPowerShellCmdLetPage_StepTitle;
            base.HelpKeyword = "VS.SharePointTools.SPE.SPPowerShellCmdLet";
        }

        /// <summary>
        /// Load the settings from the presentation model
        /// </summary>
        protected override void LoadSettingsFromPresentationModel()
        {
            base.Skippable = CurrentModel.IsOptional;
            //txtId.Text = CurrentModel.Id;
            //txtGroupId.Text = CurrentModel.GroupId;
            //txtHideActionId.Text = CurrentModel.HideActionId;
            //txtLocation.Text = CurrentModel.Location;
        }

        /// <summary>
        /// Do the databinding from the model to the form components
        /// </summary>
        /// <returns>True</returns>
        public override bool OnActivate()
        {
            //Do any databinding required here
            //Registration Type
            cboVerb.DataSource = CurrentModel.AvailableVerbs;
            cboVerb.DisplayMember = "DisplayMember";
            cboVerb.ValueMember = "ValueMember";

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
            lblIntroduction.Text = Resources.SPPowerShellCmdLetPage_Introduction;
            lblRequired.Text = Resources.SPPowerShellCmdLetPage_Required;
            lnkMSDNArticle.Text = Resources.SPPowerShellCmdLetPage_MSDNLinkText;

            System.Windows.Forms.LinkLabel.Link link = new System.Windows.Forms.LinkLabel.Link();
            link.Description = Resources.SPPowerShellCmdLetPage_MSDNLinkText;
            link.LinkData = Resources.SPPowerShellCmdLetPage_MSDNLinkUrlDefault;
            link.Enabled = true;
            link.Name = "lnkMSDN";
            lnkMSDNArticle.Links.Add(link);

            //lblId.Text = Resources.SPPowerShellCmdLetPage_ID;
            //lblGroupId.Text = Resources.SPPowerShellCmdLetPage_GroupId;
            //lblHideActionId.Text = Resources.SPPowerShellCmdLetPage_HideActionId;
            //lblLocation.Text = Resources.SPPowerShellCmdLetPage_Location;
            //btnGenerateGuid.Text = Resources.SPPowerShellCmdLetPage_GenerateGuid;
        }

        /// <summary>
        /// Is the form completely valid? Checks each field for validity
        /// </summary>
        /// <returns>True if all fields are valid</returns>
        protected override bool IsCompletelyValid()
        {
            return base.IsCompletelyValid()
                    && ValidateVerb()
                    && ValidateNoun()
                    && ValidateObject()
                    && ValidateRequireLocalFarmExist()
                    && ValidateRequireUserFarmAdmin()
                    && ValidateRequireUserMachineAdmin();
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
            //CurrentModel.Id = txtId.Text;
            //CurrentModel.GroupId = txtGroupId.Text;
            //CurrentModel.HideActionId = txtHideActionId.Text;
            //CurrentModel.Location = txtLocation.Text;
            //CurrentModel.SaveChanges();
        }

        /// <summary>
        /// Validate the Verb
        /// </summary>
        /// <returns>True if the Verb is valid</returns>
        protected virtual bool ValidateVerb()
        {
            return true;
        }

        /// <summary>
        /// Validate the Noun
        /// </summary>
        /// <returns>True if the Noun is valid</returns>
        protected virtual bool ValidateNoun()
        {
            return true;
        }

        /// <summary>
        /// Validate the Object
        /// </summary>
        /// <returns>True if the Object is valid</returns>
        protected virtual bool ValidateObject()
        {
            return true;
        }

        /// <summary>
        /// Validate the RequireLocalFarmExist
        /// </summary>
        /// <returns>True if the RequireLocalFarmExist is valid</returns>
        protected virtual bool ValidateRequireLocalFarmExist()
        {
            return true;
        }

        /// <summary>
        /// Validate the RequireUserFarmAdmin
        /// </summary>
        /// <returns>True if the RequireUserFarmAdmin is valid</returns>
        protected virtual bool ValidateRequireUserFarmAdmin()
        {
            return true;
        }

        /// <summary>
        /// Validate the RequireUserMachineAdmin
        /// </summary>
        /// <returns>True if the RequireUserMachineAdmin is valid</returns>
        protected virtual bool ValidateRequireUserMachineAdmin()
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

        private void cboVerb_SelectedIndexChanged(object sender, EventArgs e)
        {
            lnkMSDNArticle.Links.Clear();
            System.Windows.Forms.LinkLabel.Link link = new System.Windows.Forms.LinkLabel.Link();
            link.Description = Resources.SPPowerShellCmdLetPage_MSDNLinkText;
                    
            link.Enabled = true;
            link.Name = "lnkMSDN";

            switch (cboVerb.SelectedValue.ToString())
            {
                case VerbsCommon.Get:
                    link.LinkData = Resources.SPPowerShellCmdLetPage_MSDNLinkUrlGet;
                    break;
                case VerbsCommon.New:
                    link.LinkData = Resources.SPPowerShellCmdLetPage_MSDNLinkUrlNew;
                    break;
                case VerbsCommon.Remove:
                    link.LinkData = Resources.SPPowerShellCmdLetPage_MSDNLinkUrlRemove;
                    break;
                case VerbsCommon.Set:
                    link.LinkData = Resources.SPPowerShellCmdLetPage_MSDNLinkUrlSet;
                    break;
                default:
                    link.LinkData = Resources.SPPowerShellCmdLetPage_MSDNLinkUrlDefault;
                    break;
            }

            lnkMSDNArticle.Links.Add(link);
        }

        #endregion

        #region Designer Generated

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblIntroduction = new System.Windows.Forms.Label();
            this.lnkMSDNArticle = new System.Windows.Forms.LinkLabel();
            this.lblRequired = new System.Windows.Forms.Label();
            this.lblVerb = new System.Windows.Forms.Label();
            this.lblNoun = new System.Windows.Forms.Label();
            this.txtNoun = new System.Windows.Forms.TextBox();
            this.lblSPCmdletAttribute = new System.Windows.Forms.Label();
            this.cboVerb = new System.Windows.Forms.ComboBox();
            this.cboCommonObjects = new System.Windows.Forms.ComboBox();
            this.clbSPCmdletAttribute = new System.Windows.Forms.CheckedListBox();
            this.lblOtherObject = new System.Windows.Forms.Label();
            this.lblObject = new System.Windows.Forms.Label();
            this.txtOtherObject = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // infoPanel
            // 
            this.infoPanel.Location = new System.Drawing.Point(0, 310);
            this.infoPanel.Size = new System.Drawing.Size(508, 304);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.lblIntroduction, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lnkMSDNArticle, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblRequired, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblVerb, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblNoun, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.txtNoun, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.lblSPCmdletAttribute, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.cboVerb, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.cboCommonObjects, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.clbSPCmdletAttribute, 0, 15);
            this.tableLayoutPanel1.Controls.Add(this.lblOtherObject, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.lblObject, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.txtOtherObject, 1, 12);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 19;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(508, 310);
            this.tableLayoutPanel1.TabIndex = 5;
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
            // lblVerb
            // 
            this.lblVerb.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblVerb, 2);
            this.lblVerb.Location = new System.Drawing.Point(3, 50);
            this.lblVerb.Name = "lblVerb";
            this.lblVerb.Size = new System.Drawing.Size(32, 13);
            this.lblVerb.TabIndex = 70;
            this.lblVerb.Text = "Verb:";
            // 
            // lblNoun
            // 
            this.lblNoun.AutoSize = true;
            this.lblNoun.Location = new System.Drawing.Point(3, 94);
            this.lblNoun.Name = "lblNoun";
            this.lblNoun.Size = new System.Drawing.Size(36, 13);
            this.lblNoun.TabIndex = 72;
            this.lblNoun.Text = "Noun:";
            // 
            // txtNoun
            // 
            this.txtNoun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtNoun, 2);
            this.txtNoun.Location = new System.Drawing.Point(3, 110);
            this.txtNoun.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.txtNoun.Name = "txtNoun";
            this.txtNoun.Size = new System.Drawing.Size(475, 20);
            this.txtNoun.TabIndex = 2;
            // 
            // lblSPCmdletAttribute
            // 
            this.lblSPCmdletAttribute.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblSPCmdletAttribute, 2);
            this.lblSPCmdletAttribute.Location = new System.Drawing.Point(3, 181);
            this.lblSPCmdletAttribute.Name = "lblSPCmdletAttribute";
            this.lblSPCmdletAttribute.Size = new System.Drawing.Size(134, 13);
            this.lblSPCmdletAttribute.TabIndex = 76;
            this.lblSPCmdletAttribute.Text = "SPCmdletAttribute settings:";
            // 
            // cboVerb
            // 
            this.cboVerb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboVerb.FormattingEnabled = true;
            this.cboVerb.Location = new System.Drawing.Point(3, 66);
            this.cboVerb.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.cboVerb.Name = "cboVerb";
            this.cboVerb.Size = new System.Drawing.Size(322, 21);
            this.cboVerb.TabIndex = 83;
            this.cboVerb.SelectedIndexChanged += new System.EventHandler(this.cboVerb_SelectedIndexChanged);
            // 
            // cboCommonObjects
            // 
            this.cboCommonObjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboCommonObjects.FormattingEnabled = true;
            this.cboCommonObjects.Location = new System.Drawing.Point(3, 153);
            this.cboCommonObjects.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.cboCommonObjects.Name = "cboCommonObjects";
            this.cboCommonObjects.Size = new System.Drawing.Size(322, 21);
            this.cboCommonObjects.TabIndex = 84;
            // 
            // clbSPCmdletAttribute
            // 
            this.clbSPCmdletAttribute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clbSPCmdletAttribute.FormattingEnabled = true;
            this.clbSPCmdletAttribute.Location = new System.Drawing.Point(3, 197);
            this.clbSPCmdletAttribute.Margin = new System.Windows.Forms.Padding(3, 3, 30, 3);
            this.clbSPCmdletAttribute.Name = "clbSPCmdletAttribute";
            this.clbSPCmdletAttribute.Size = new System.Drawing.Size(322, 94);
            this.clbSPCmdletAttribute.TabIndex = 85;
            // 
            // lblOtherObject
            // 
            this.lblOtherObject.AutoSize = true;
            this.lblOtherObject.Location = new System.Drawing.Point(358, 137);
            this.lblOtherObject.Name = "lblOtherObject";
            this.lblOtherObject.Size = new System.Drawing.Size(36, 13);
            this.lblOtherObject.TabIndex = 82;
            this.lblOtherObject.Text = "Other:";
            this.lblOtherObject.Visible = false;
            // 
            // lblObject
            // 
            this.lblObject.AutoSize = true;
            this.lblObject.Location = new System.Drawing.Point(3, 137);
            this.lblObject.Name = "lblObject";
            this.lblObject.Size = new System.Drawing.Size(41, 13);
            this.lblObject.TabIndex = 74;
            this.lblObject.Text = "Object:";
            // 
            // txtOtherObject
            // 
            this.txtOtherObject.Location = new System.Drawing.Point(358, 153);
            this.txtOtherObject.Name = "txtOtherObject";
            this.txtOtherObject.Size = new System.Drawing.Size(100, 20);
            this.txtOtherObject.TabIndex = 86;
            this.txtOtherObject.Visible = false;
            // 
            // SPPowerShellCmdLetPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SPPowerShellCmdLetPage";
            this.Size = new System.Drawing.Size(508, 614);
            this.Controls.SetChildIndex(this.infoPanel, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        
    }
}

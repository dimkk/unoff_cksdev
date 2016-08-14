using System;
using System.Windows.Forms;

namespace CKS.Dev.VisualStudio.SharePoint.Environment
{
    partial class SharePointReferenceView
    {
        void InitializeComponent()
        {
            this.nameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pathHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._referencesList = new System.Windows.Forms.ListView();
            this.versionHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // nameHeader
            // 
            this.nameHeader.Text = "Name";
            this.nameHeader.Width = 245;
            // 
            // pathHeader
            // 
            this.pathHeader.Text = "Path";
            this.pathHeader.Width = 700;
            // 
            // _referencesList
            // 
            this._referencesList.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this._referencesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameHeader,
            this.versionHeader,
            this.pathHeader});
            this._referencesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._referencesList.FullRowSelect = true;
            this._referencesList.HideSelection = false;
            this._referencesList.Location = new System.Drawing.Point(12, 12);
            this._referencesList.Name = "_referencesList";
            this._referencesList.Size = new System.Drawing.Size(126, 126);
            this._referencesList.TabIndex = 0;
            this._referencesList.UseCompatibleStateImageBehavior = false;
            this._referencesList.View = System.Windows.Forms.View.Details;
            this._referencesList.SelectedIndexChanged += new System.EventHandler(this.ReferencesList_SelectedIndexChanged);
            this._referencesList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ReferencesList_MouseDoubleClick);
            // 
            // versionHeader
            // 
            this.versionHeader.Text = "Version";
            this.versionHeader.Width = 125;
            // 
            // SharePointReferenceView
            // 
            this.Controls.Add(this._referencesList);
            this.Name = "SharePointReferenceView";
            this.ResumeLayout(false);

        }

        private ListView _referencesList;
        private ColumnHeader nameHeader;
        private ColumnHeader pathHeader;
        private ColumnHeader versionHeader;
    }
}

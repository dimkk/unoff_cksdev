using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WizardFrameworkVS;
using EnvDTE;
using System.Drawing;
using System.ComponentModel;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Content.Wizards
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Content.Wizards
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Content.Wizards
#else
namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
#endif
{
    class ArtifactWizardForm : WizardFormVS, IWizardFormExtension
    {
        // Fields
        private bool _cancelled;

        // Methods
        public ArtifactWizardForm(DTE designTimeEnvironment, string title)
            : base(designTimeEnvironment)
        {
            this._cancelled = true;
            this.InitializeComponent();
            base.Title = title;
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            Font font = null;
            this.GetEnvironmentFont(ref font);
            if (font != null)
            {
                this.Font = font;
            }
            base.ShowOrientationPanel = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ShowIcon = true;
            base.HelpButton = true;
            this.MinimumSize = new Size(600, 400);
            base.HelpKeyword = "SPE.Wizard";
            base.ResumeLayout();
        }

        public void InvalidateRest()
        {
            if (base.ActivePage != null)
            {
                for (int i = base.ActivePageIndex + 1; i < base.PageCount; i++)
                {
                    base.GetPage(i).Visited = false;
                }
                this.OnValidationStateChanged(base.ActivePage);
            }
        }

        void IWizardFormExtension.Launch()
        {
            base.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this._cancelled = true;
        }

        public override void OnFinish()
        {
            base.OnFinish();
            this._cancelled = false;
        }

        // Properties
        public bool Cancelled
        {
            get
            {
                return this._cancelled;
            }
        }
    }


} 


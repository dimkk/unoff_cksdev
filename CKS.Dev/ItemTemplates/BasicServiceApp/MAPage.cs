using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;

namespace $rootnamespace$
{
    public partial class Manage$subnamespace$ApplicationPage : Page
    {
        protected InputFormTextBox InputFormTextBox;

        protected override void OnInit(EventArgs e)
        {
            ((DialogMaster)this.Page.Master).OkButton.Click += OkButton_Click;
            base.OnInit(e);
        }

        void OkButton_Click(object sender, EventArgs e)
        {
            using (SPLongOperation operation = new SPLongOperation(this))
            {
                operation.LeadingHTML = "Performing operations";
                operation.Begin();
                try
                {
                }
                finally
                {
                    operation.EndScript("window.frameElement.commitPopup();");
                }
            }
        }
    }
}

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;

namespace $rootnamespace$
{
    public partial class New$subnamespace$ApplicationPage : Page
    {
        protected InputFormTextBox nameField;
        protected InputFormCheckBox defaultProxyField;

        protected override void OnInit(EventArgs e)
        {
            ((DialogMaster)this.Page.Master).OkButton.Click += OkButton_Click;
            base.OnInit(e);
        }

        void OkButton_Click(object sender, EventArgs e)
        {
            using (SPLongOperation operation = new SPLongOperation(this))
            {
                operation.LeadingHTML = "Creating new $fileinputname$ Application";
                operation.Begin();
                try
                {
                    SPFarm farm = SPFarm.Local;
                    $subnamespace$Service service = farm.Services.GetValue<$subnamespace$Service>();
                    $subnamespace$ServiceProxy serviceProxy = farm.ServiceProxies.GetValue<$subnamespace$ServiceProxy>();
                    
                    string title = nameField.Text;
                    $subnamespace$ServiceApplication application = $subnamespace$ServiceApplication.Create(
                        title, service);
                    application.Provision();
                    $subnamespace$ServiceApplicationProxy applicationProxy = $subnamespace$ServiceApplicationProxy.Create(
                        title, serviceProxy, application.Id);
                    applicationProxy.Provision();
                    if(defaultProxyField.Checked)
                    {
                        SPServiceApplicationProxyGroup.Default.Add(applicationProxy);
                    }
                }
                finally
                {
                    operation.EndScript("window.frameElement.commitPopup();");
                }
            }
        }
    }
}

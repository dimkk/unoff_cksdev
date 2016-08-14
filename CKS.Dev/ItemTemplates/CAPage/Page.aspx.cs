using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.ApplicationPages;
using System.Web.UI.WebControls;

namespace $rootnamespace$
{
    public partial class $safeitemrootname$ : GlobalAdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // wire-up control event handlers  
            btnSubmitTop.Click += btnSubmitTop_Click;
            btnCancelTop.Click += btnCancelTop_Click;

            WebAppSelector.ContextChange += WebAppSelector_ContextChange;

            if (!IsPostBack)
            {
                // default value  
                txtConfigurationSiteURL.Text = "n/a";
            }
        }

        void WebAppSelector_ContextChange(object sender, EventArgs e)
        {
            // user changed the web application selection
            // NOTE: this event also fire when the page first loads
            var wa = ((WebApplicationSelector)sender).CurrentItem;
            if (wa == null)
            {
                litWebAppName.Text = "n/a";
                txtConfigurationSiteURL.Text = "n/a";
                return;
            }

            litWebAppName.Text = string.Format("{0}", string.Format("{0}", wa.Name));
            txtConfigurationSiteURL.Text = wa.Sites.Count > 0
                                                        ? wa.Sites[0].Url
                                                        : "n/a";
        }

        void btnCancelTop_Click(object sender, EventArgs e)
        {
            // go back to Application Management
            Response.Redirect("/default.aspx");
        }

        void btnSubmitTop_Click(object sender, EventArgs e)
        {
            // save page values and go back to Application Management
            Response.Redirect("/default.aspx");
        }

    }
}

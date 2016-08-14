using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace $rootnamespace$.$subnamespace$
{
    [ToolboxItemAttribute(false)]
    public class $WrapperWebPartClassName$ : WebPart
    {
        protected override void CreateChildControls()
        {
            Controls.Add(new ASP.$ascx_generatedName$());
            ChildControlsCreated = true;
        }
    }
}

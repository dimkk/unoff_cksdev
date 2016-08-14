using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace $rootnamespace$.$subnamespace$
{
    [ToolboxItemAttribute(false)]
    public class $safeitemrootname$ : WebPart
    {
        #region Fields

        // $loc_ascxPath_comment$
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/$rootnamespace$/$fileinputname$/$fileinputname$UserControl.ascx";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the custom message.
        /// </summary>
        /// <value>The custom message.</value>
        [WebBrowsable(true)]
        [WebDisplayName("Custom message")]
        [WebDescription("The message to displaty.")]
        [Personalizable(PersonalizationScope.Shared)]
        [Category("Settings")]
        [DefaultValue("")]
        public string CustomMessage
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create 
        /// any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            //user control is of type $fileinputname$UserControl and defined with private scope
            $fileinputname$UserControl control = ($fileinputname$UserControl)this.Page.LoadControl(_ascxPath);
            
            //Set the control properties from the webpart properties
            control.CustomMessage = CustomMessage;
            control.WebPartInstanceId = this.ID;
            //Add the control
            Controls.Add(control);
            base.CreateChildControls();
        }

        #endregion
    }
}

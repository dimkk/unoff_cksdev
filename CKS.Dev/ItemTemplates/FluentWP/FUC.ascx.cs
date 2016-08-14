using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace $rootnamespace$.$subnamespace$
{
    public partial class $safeitemrootname$ : UserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets the custom message.
        /// </summary>
        /// <value>The custom message.</value>
        public string CustomMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the web part instance id.
        /// </summary>
        /// <value>The web part instance id.</value>
        public string WebPartInstanceId
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetDisplayMessage();
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            AddDialogScript();
            SetDialogScriptLink();            
        }

         /// <summary>
        /// Adds the dialog script.
        /// </summary>
        private void AddDialogScript()
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("WebPartDialogScript"))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                    "WebPartDialogScript",
                    HttpContext.GetGlobalResourceObject("$rootnamespace$.$subnamespace$.$fileinputname$WebPart", "WebPartDialogScriptBlock").ToString());
            }
        }

        /// <summary>
        /// Sets the display message.
        /// </summary>
        private void SetDisplayMessage()
        {
            if (String.IsNullOrEmpty(CustomMessage))
            {
                litDisplayMessage.Text = "The message was left blank";
            }
            else
            {
                litDisplayMessage.Text = CustomMessage;
            }
        }

        /// <summary>
        /// Sets the dialog script link.
        /// </summary>
        private void SetDialogScriptLink()
        {
            litChangeSettingsLink.Text = String.Format(HttpContext.GetGlobalResourceObject("$rootnamespace$.$subnamespace$.$fileinputname$WebPart", "DialogScriptTag").ToString(),
                HttpContext.Current.Request.Url,
                WebPartInstanceId.ToString());
                
        }

        #endregion
    }
}

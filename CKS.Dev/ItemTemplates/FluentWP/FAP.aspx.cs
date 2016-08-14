using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web;
using System.Globalization;
using Microsoft.SharePoint.WebPartPages;
using System.Web.UI.WebControls.WebParts;

namespace $rootnamespace$.$subnamespace$.ApplicationPages
{
    /// <summary>
    /// Application page to modify the properties of a $fileinputname$WebPart. Normally untilised from within
    /// a SharePoint client side dialog.
    /// </summary>
    public partial class Edit$fileinputname$ : LayoutsPageBase
    {
        #region Constants

        /// <summary>
        /// The query string name for the page url.
        /// </summary>
        private const string PAGEURLQSKEYNAME = "pageUrl";

        /// <summary>
        /// The query string name for the webpart id.
        /// </summary>
        private const string WEBPARTIDQSKEYNAME = "webPartId";

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Click event of the btnOk control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnOk_Click(object sender, EventArgs e)
        {
            UpdateWebPart();
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            SendPageResponse(0, HttpContext.GetGlobalResourceObject("$rootnamespace$.$subnamespace$.$fileinputname$WebPart", "NotificationMessage_Cancel").ToString());
        }

        /// <summary>
        /// Handles the Load event of the Page control to set the UI with the current webpart property values.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Only set the values if not paostback or callback
            if (!Page.IsPostBack && !Page.IsCallback)
            {
                if (CheckQueryStringValues())
                {
                    GetWebPartProperties(this.Page.Request.QueryString[PAGEURLQSKEYNAME],
                    this.Page.Request.QueryString[WEBPARTIDQSKEYNAME]);
                }
            }
        }

        /// <summary>
        /// Checks the query string values to ensure they can be used. Forms part of the page load guard pattern.
        /// </summary>
        /// <returns>True if all the guards pass.</returns>
        /// <exception cref="ArgumentException">Thrown if the querystring doesn't contain the keys or the values are invalid.</exception>
        private bool CheckQueryStringValues()
        {
            //Check that the page url was supplied
            if(this.Page.Request.QueryString[PAGEURLQSKEYNAME] == null)
            {
                throw new ArgumentException(HttpContext.GetGlobalResourceObject("$rootnamespace$.$subnamespace$.$fileinputname$WebPart", "ExceptionMessage_PageUrlMissing").ToString());
            }

            //Check the page url is valid
            if (String.IsNullOrEmpty(this.Page.Request.QueryString[PAGEURLQSKEYNAME]))
            {
                throw new ArgumentException(HttpContext.GetGlobalResourceObject("$rootnamespace$.$subnamespace$.$fileinputname$WebPart", "ExceptionMessage_PageUrlInvalid").ToString());
            }

            //Check that the webpart id was supplied
            if (this.Page.Request.QueryString[WEBPARTIDQSKEYNAME] == null)
            {
                throw new ArgumentException(HttpContext.GetGlobalResourceObject("$rootnamespace$.$subnamespace$.$fileinputname$WebPart", "ExceptionMessage_WebPartIdMissing").ToString());
            }

            //Check the page url is valid
            if (String.IsNullOrEmpty(this.Page.Request.QueryString[WEBPARTIDQSKEYNAME]))
            {
                throw new ArgumentException(HttpContext.GetGlobalResourceObject("$rootnamespace$.$subnamespace$.$fileinputname$WebPart", "ExceptionMessage_WebPartIdInvalid").ToString());
            }

            return true;
        }


        /// <summary>
        /// Updates the web part with the properties.
        /// </summary>
        private void UpdateWebPart()
        {
            SetWebPartProperties(this.Page.Request.QueryString[PAGEURLQSKEYNAME],
                this.Page.Request.QueryString[WEBPARTIDQSKEYNAME]);
            SendPageResponse(1, HttpContext.GetGlobalResourceObject("$rootnamespace$.$subnamespace$.$fileinputname$WebPart", "NotificationMessage_Success").ToString());
        }

        /// <summary>
        /// Sends the page response to render the require script to close the dialog.
        /// </summary>
        /// <param name="dialogResult">The dialog result.</param>
        /// <param name="message">The message.</param>
        private void SendPageResponse(int dialogResult, string message)
        {
            this.Page.Response.Write(string.Format(CultureInfo.InvariantCulture,
                 "<script type=\"text/javascript\">window.frameElement.commonModalDialogClose({0}, '{1}');</script>", dialogResult.ToString(), message));
            this.Page.Response.End();
        }

        /// <summary>
        /// Sets the web part properties with the values in the UI.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="webPartId">The web part id.</param>
        /// <returns>True always.</returns>
        private bool SetWebPartProperties(string pageUrl, string webPartId)
        {
            // Here you will use the url of the page to get an instance of its
            // WebPartManager - with this object, you will be able to find and
            // update your web part....
            using (SPLimitedWebPartManager wpm = SPContext.Current.Web.GetLimitedWebPartManager(pageUrl, PersonalizationScope.Shared))
            {
                // ... using its id to find it in the WebParts collection of the
                // WebPartManager.
                System.Web.UI.WebControls.WebParts.WebPart wp = wpm.WebParts[webPartId];

                // ... and this setting is important, otherwise it won't accept
                // changes by code. Notice that I'm setting it back to false after
                // the update...
                wpm.Web.AllowUnsafeUpdates = true;

                // Here you update the Personalizable properties of your web part... 
                (wp as $rootnamespace$.$subnamespace$.$fileinputname$).CustomMessage = txtCustomMessage.Text;
                // ... and then you save it...
                wpm.SaveChanges(wp);
                wpm.Web.AllowUnsafeUpdates = false;
            }
            return true;
        }

        /// <summary>
        /// Gets the web part existing property values and set the UI controls with them.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="webPartId">The web part id.</param>
        /// <returns>True always.</returns>
        private bool GetWebPartProperties(string pageUrl, string webPartId)
        {
            // Here you will use the url of the page to get an instance of its
            // WebPartManager - with this object, you will be able to find and
            // update your web part....
            using (SPLimitedWebPartManager wpm = SPContext.Current.Web.GetLimitedWebPartManager(pageUrl, PersonalizationScope.Shared))
            {
                // ... using its id to find it in the WebParts collection of the
                // WebPartManager.
                System.Web.UI.WebControls.WebParts.WebPart wp = wpm.WebParts[webPartId];

                // Here you update the Personalizable properties of your web part... 
                txtCustomMessage.Text = (wp as $rootnamespace$.$subnamespace$.$fileinputname$).CustomMessage;

                litPlaceHolderPageTitle.Text = String.Format(HttpContext.GetGlobalResourceObject("$rootnamespace$.$subnamespace$.$fileinputname$WebPart", "PlaceHolderPageTitle").ToString(), wp.DisplayTitle);
                litPlaceHolderPageTitleInTitleArea.Text = String.Format(HttpContext.GetGlobalResourceObject("$rootnamespace$.$subnamespace$.$fileinputname$WebPart", "PlaceHolderPageTitleInTitleArea").ToString(), wp.DisplayTitle);
            }
            return true;
        }

        #endregion
    }
}

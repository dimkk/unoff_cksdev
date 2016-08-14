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
    [Guid("$guid1$")]
    public class $fileinputname$ : WebPart, IWebPartPageComponentProvider 
    {
        #region Properties

        public WebPartContextualInfo WebPartContextualInfo 
        {
            get 
            {
                WebPartContextualInfo contextInfo = new WebPartContextualInfo();

                contextInfo.ContextualGroups.Add(new WebPartRibbonContextualGroup() {
                    Id = "$rootnamespace$.$fileinputname$.ContextualGroup",
                    VisibilityContext = "$rootnamespace$.$fileinputname$.VisibilityContext",
                    Command = "$rootnamespace$.$fileinputname$.ContextualGroupCommand"
                });

                contextInfo.Tabs.Add(new WebPartRibbonTab() {
                    Id = "$rootnamespace$.$fileinputname$.ContextualGroup.Tab",
                    VisibilityContext = "$rootnamespace$.$fileinputname$.VisibilityContext"
                });

                contextInfo.PageComponentId = SPRibbon.GetWebPartPageComponentId(this);

                return contextInfo;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based 
        /// implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls() 
        {
            // Registers necessary JavaScript files
            ScriptLink.RegisterScriptAfterUI(this.Page, "CUI.js", false, true);
            ScriptLink.RegisterScriptAfterUI(this.Page, "SP.Ribbon.js", false, true);
            ScriptLink.RegisterScriptAfterUI(this.Page, "$fileinputname$/PageComponent.js", false);

            // TODO: Add your implementation here
            this.Controls.Add(new LiteralControl("My Contextual Web Part"));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e) 
        {
            ScriptManager.RegisterStartupScript(this.Page,
                typeof($fileinputname$),
                "$fileinputname$" + this.ClientID,
                string.Format("SP.SOD.executeOrDelayUntilScriptLoaded(" +
                    "function(){{$rootnamespace$.$fileinputname$.create('{0}').register();}}," +
                    "'$fileinputname$/PageComponent.js');",
                    SPRibbon.GetWebPartPageComponentId(this)),
                true);

            base.OnPreRender(e);
        }

        #endregion        
    }
}

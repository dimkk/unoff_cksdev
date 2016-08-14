using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    /// <summary>
    /// Defines the type of welcome page that is used for a site/web template
    /// </summary>
    public enum WelcomePageType
    {
        /// <summary>
        /// A page with web part zones
        /// </summary>
        WebPartPage,
        /// <summary>
        /// A wiki page
        /// </summary>
        WikiPage,
        /// <summary>
        /// A publishing page
        /// </summary>
        PublishingPage
    }
}

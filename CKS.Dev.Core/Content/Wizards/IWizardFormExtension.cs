using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// Interface to define the form
    /// </summary>
    public interface IWizardFormExtension
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IWizardFormExtension" /> is cancelled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cancelled; otherwise, <c>false</c>.
        /// </value>
        bool Cancelled
        {
            get;
        }

        /// <summary>
        /// Launches this instance.
        /// </summary>
        void Launch();
    }
}

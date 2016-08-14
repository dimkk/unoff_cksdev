using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Content.Wizards.Models
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Content.Wizards.Models
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Content.Wizards.Models
#else
namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models
#endif
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BasePresentationModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is optional.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is optional; otherwise, <c>false</c>.
        /// </value>
        public bool IsOptional
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePresentationModel" /> class.
        /// </summary>
        /// <param name="isOptional">if set to <c>true</c> [is optional].</param>
        protected BasePresentationModel(bool isOptional)
        {
            this.IsOptional = isOptional;
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public abstract void SaveChanges();
    }
}

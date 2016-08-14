using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKS.Dev11.VisualStudio.SharePoint.Content.Wizards.Models
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

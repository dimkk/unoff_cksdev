using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Models
{
    /// <summary>
    /// Basic data item used for list controls
    /// </summary>
    class BasicListItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the display text
        /// </summary>
        public string DisplayMember
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value text
        /// </summary>
        public string ValueMember
        {
            get;
            set;
        }

        #endregion
    }
}

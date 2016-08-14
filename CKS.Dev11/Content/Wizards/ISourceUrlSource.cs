using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKS.Dev11.VisualStudio.SharePoint.Content.Wizards
{
    /// <summary>
    /// The interface to define the Source Url Source
    /// </summary>
    public interface ISourceUrlSource : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the source URL.
        /// </summary>
        /// <value>
        /// The source URL.
        /// </value>
        Uri SourceUrl
        {
            get;
        }
    }
}

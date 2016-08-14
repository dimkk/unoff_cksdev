using System;
using System.Collections.Generic;
using System.ComponentModel;
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

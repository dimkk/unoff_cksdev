using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Environment
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Environment
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Environment
#else
namespace CKS.Dev.VisualStudio.SharePoint.Environment
#endif
{
    public interface ICKSDevVSPackage
    {
        object GetServiceInternal(Type type);
    }
}

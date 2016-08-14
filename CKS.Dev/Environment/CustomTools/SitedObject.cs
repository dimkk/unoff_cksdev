using System;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;

namespace CKS.Dev.VisualStudio.SharePoint.Environment.CustomTools
{
    /// <summary>
    /// A Sited object.
    /// </summary>
    public class SitedObject
        : IObjectWithSite
    {
        object _site;
        ServiceProvider _serviceProvider;

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <value>The service provider.</value>
        protected ServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    _serviceProvider = new ServiceProvider((IOleServiceProvider)_site);
                }
                return _serviceProvider;
            }
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        protected object GetService(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }

        /// <summary>
        /// </summary>
        /// <param name="riid"></param>
        /// <param name="ppvSite"></param>
        void IObjectWithSite.GetSite(ref Guid riid, out IntPtr ppvSite)
        {
            if (_site == null)
            {
                throw new COMException("Object not sited", VSConstants.E_FAIL);
            }
            IntPtr unknownSite = Marshal.GetIUnknownForObject(_site);
            IntPtr requestedSite = IntPtr.Zero;
            Marshal.QueryInterface(unknownSite, ref riid, out requestedSite);
            if (requestedSite == IntPtr.Zero)
            {
                throw new COMException("Requested interface not supported", VSConstants.E_NOINTERFACE);
            }
            ppvSite = requestedSite;
        }

        void IObjectWithSite.SetSite(object pUnkSite)
        {
            _site = pUnkSite;
        }
    }
}

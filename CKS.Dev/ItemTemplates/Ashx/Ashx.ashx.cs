using System;
using System.Web;
using System.Runtime.InteropServices;

namespace $rootnamespace$
{
    [Guid("$ashxGuid$")]
	public partial class $safeitemname$ : IHttpHandler
	{
		#region IHttpHandler Members

		/// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
		{
			get { return false; }
		}

		/// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
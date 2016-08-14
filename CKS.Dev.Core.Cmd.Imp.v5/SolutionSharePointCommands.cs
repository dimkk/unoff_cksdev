using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Commands;
using Microsoft.SharePoint;
using System.IO;

#if VS2012Build_SYMBOL
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
#elif VS2013Build_SYMBOL
    using CKS.Dev12.VisualStudio.SharePoint.Commands.Info;
#elif VS2014Build_SYMBOL
    using CKS.Dev13.VisualStudio.SharePoint.Commands.Info;
#else
    using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
#endif

#if VS2012Build_SYMBOL
namespace CKS.Dev11.VisualStudio.SharePoint.Commands
#elif VS2013Build_SYMBOL
    namespace CKS.Dev12.VisualStudio.SharePoint.Commands
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Commands
#else
    namespace CKS.Dev.VisualStudio.SharePoint.Commands
#endif
{
    /// <summary>
    /// The solution commands.
    /// </summary>
    internal class SolutionSharePointCommands
    {
        #region Methods

        /// <summary>
        /// Gets additional property data for a specific solution.
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="nodeInfo">The node info</param>
        /// <returns>The properties</returns>
        [SharePointCommand(SolutionSharePointCommandIds.GetSolutionProperties)]
        private static Dictionary<string, string> GetSolutionProperties(ISharePointCommandContext context,
            FileNodeInfo nodeInfo)
        {
            SPList solutions = context.Site.GetCatalog(SPListTemplateType.SolutionCatalog);
            SPListItem solution = solutions.Items[nodeInfo.UniqueId];

            return SharePointCommandServices.GetProperties(solution);
        }

        #endregion
    }
}

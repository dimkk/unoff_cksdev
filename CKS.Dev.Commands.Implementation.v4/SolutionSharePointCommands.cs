using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Commands;
using Microsoft.SharePoint;
using System.IO;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;

namespace CKS.Dev11.VisualStudio.SharePoint.Commands
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

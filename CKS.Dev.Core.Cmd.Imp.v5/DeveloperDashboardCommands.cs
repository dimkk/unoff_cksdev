using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Commands;
using Microsoft.SharePoint.Administration;
using CKS.Dev.Core.Cmd.Imp.v5.Properties;

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
    /// The developer dashboard commands
    /// </summary>
    internal class DeveloperDashboardCommands
    {
        /// <summary>
        /// Gets current farm developer dashboard display level setting value.
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns>The setting value as a string</returns>
        [SharePointCommand(DeveloperDashboardCommandIds.GetDeveloperDashBoardDisplayLevelSetting)]
        private static string GetDeveloperDashBoardDisplayLevelSetting(ISharePointCommandContext context)
        {
            SPDeveloperDashboardSettings settings = context.Site.WebApplication.WebService.DeveloperDashboardSettings;

            return settings.DisplayLevel.ToString();
        }

        /// <summary>
        /// Sets current farm developer dashboard display level setting value.
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="newLevel">The new level to set it to.</param>
        [SharePointCommand(DeveloperDashboardCommandIds.SetDeveloperDashBoardDisplayLevelSetting)]
        private static void SetDeveloperDashBoardDisplayLevelSetting(ISharePointCommandContext context, string newLevel)
        {
            try
            {
                SPDeveloperDashboardLevel level = ((SPDeveloperDashboardLevel)Enum.Parse(typeof(SPDeveloperDashboardLevel), newLevel));

                SPDeveloperDashboardSettings settings = context.Site.WebApplication.WebService.DeveloperDashboardSettings;

                settings.DisplayLevel = level;

                settings.Update(true);
            }
            catch (ArgumentNullException ex)
            {
                context.Logger.WriteLine(String.Format(Resources.DeveloperDashboardCommands_SetException,
                          ex.Message,
                          Environment.NewLine,
                          ex.StackTrace), LogCategory.Error);
            }
            catch (ArgumentException ex)
            {
                context.Logger.WriteLine(String.Format(Resources.DeveloperDashboardCommands_SetException,
                          ex.Message,
                          Environment.NewLine,
                          ex.StackTrace), LogCategory.Error);
            }
            catch (OverflowException ex)
            {
                context.Logger.WriteLine(String.Format(Resources.DeveloperDashboardCommands_SetException,
                          ex.Message,
                          Environment.NewLine,
                          ex.StackTrace), LogCategory.Error);
            }
        }
    }
}

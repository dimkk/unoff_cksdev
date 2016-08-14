using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.VisualStudio.SharePoint.Content
{
    /// <summary>
    /// SPPowerShellCmdLe item provider.
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(ISharePointProjectItemTypeProvider))]
    // Specifies the ID for this new project item type. This string must match the value of the 
    // Type attribute of the ProjectItem element in the .spdata file for the project item.
    [SharePointProjectItemType(ProjectItemIds.PowerShellCmdLet)]
    // Specifies the icon to display with this project item in Solution Explorer.
    [SharePointProjectItemIcon("CKS.Dev.VisualStudio.SharePoint.Resources.SolutionExplorerIcons.PowerShellCmdLet_SolutionExplorer.ico")]
    partial class SPPowerShellCmdLetProvider : ISharePointProjectItemTypeProvider
    {
        public void InitializeType(ISharePointProjectItemTypeDefinition typeDefinition)
        {
            //throw new NotImplementedException();
        }
    }
}

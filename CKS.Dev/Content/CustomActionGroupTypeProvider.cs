using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint;
using CKS.Dev.VisualStudio.SharePoint.Exploration;
using System.ComponentModel.Composition;
using CKS.Dev.VisualStudio.SharePoint.Content;
namespace CKS.Dev.VisualStudio.SharePoint.Content
{
    /// <summary>
    /// CustomActionGroup item provider.
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(ISharePointProjectItemTypeProvider))]
    // Specifies the ID for this new project item type. This string must match the value of the 
    // Type attribute of the ProjectItem element in the .spdata file for the project item.
    [SharePointProjectItemType(ProjectItemIds.CustomActionGroup)]
    // Specifies the icon to display with this project item in Solution Explorer.
    [SharePointProjectItemIcon("CKS.Dev.VisualStudio.SharePoint.Resources.SolutionExplorerIcons.CustomActionGroup_SolutionExplorer.ico")]
    partial class CustomActionGroupTypeProvider : ISharePointProjectItemTypeProvider
    {
        #region Methods

        public void InitializeType(ISharePointProjectItemTypeDefinition typeDefinition)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}

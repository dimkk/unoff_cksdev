﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint;
using System.ComponentModel.Composition;

namespace CKS.Dev.VisualStudio.SharePoint.Content
{
    /// <summary>
    /// Central Administration Page item provider.
    /// </summary>
    // Enables Visual Studio to discover and load this extension.
    [Export(typeof(ISharePointProjectItemTypeProvider))]
    // Specifies the ID for this new project item type. This string must match the value of the 
    // Type attribute of the ProjectItem element in the .spdata file for the project item.
    [SharePointProjectItemType(ProjectItemIds.CentralAdministrationPage)]
    // Specifies the icon to display with this project item in Solution Explorer.
    [SharePointProjectItemIcon("CKS.Dev.VisualStudio.SharePoint.Resources.SolutionExplorerIcons.CentralAdministrationPage_SolutionExplorer.ico")]
    partial class CentralAdminPageTypeProvider : ISharePointProjectItemTypeProvider
    {
        public void InitializeType(ISharePointProjectItemTypeDefinition typeDefinition)
        {
            //throw new NotImplementedException();
        }
    }
}
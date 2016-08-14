using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using CKS.Dev.WCT.Framework.Extensions;

namespace CKS.Dev.WCT.Extensions
{
    public static class ProjectItemsExtensions
    {
        public const string vsProjectItemKindPhysicalFile = "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";
        public const string vsProjectItemKindPhysicalFolder = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";

        public static ProjectItem FindFile(this ProjectItems items, string fullPath)
        {
            return items.FindItem(fullPath, vsProjectItemKindPhysicalFile, true);
        }

        public static ProjectItem FindFolder(this ProjectItems items, string fullPath)
        {
            return items.FindItem(fullPath, vsProjectItemKindPhysicalFolder, true);
        }

        public static ProjectItem FindItem(this ProjectItems items, string fullPath, string kind, bool recursive)
        {
            ProjectItem result = null;

            if (items != null)
            {
                foreach (ProjectItem item in items)
                {

                    string itemPath = item.Properties.Item("FullPath").Value.ToString();
                    if (itemPath.EqualsIgnoreCase(fullPath))
                    {
                        result = item;
                    }
                    else
                    {
                        if (recursive)
                        {
                            result = FindItem(item.ProjectItems, fullPath, kind, recursive);
                        }
                    }

                    if (result != null)
                    {
                        break;
                    }
                }
            }

            return result;
        }

    }
}

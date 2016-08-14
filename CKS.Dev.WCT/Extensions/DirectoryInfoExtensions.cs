using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CKS.Dev.WCT.SolutionModel;

namespace CKS.Dev.WCT.Extensions
{
    public static class DirectoryInfoExtensions
    {

        public static IList<DirectoryInfo> GetDirectories(this DirectoryInfo parent, WCTContext context, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            List<DirectoryInfo> result = new List<DirectoryInfo>();

            foreach (var item in parent.GetDirectories(searchPattern, searchOption))
            {
                if (!context.ExcludedFolders.Contains(item.Name, StringComparer.OrdinalIgnoreCase))
                {
                    result.Add(item);
                }
            }

            return result;
        }


        public static IEnumerable<FileInfo> GetFiles(this DirectoryInfo dir, WCTContext context, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            List<FileInfo> result = new List<FileInfo>();

            foreach (var item in dir.GetFiles(searchPattern, SearchOption.TopDirectoryOnly))
            {
                if (!context.ExcludedFileExtensions.Contains(item.Extension, StringComparer.OrdinalIgnoreCase))
                {
                    result.Add(item);
                }

            }

            if (searchOption == SearchOption.AllDirectories)
            {
                foreach (var childDir in dir.GetDirectories(context, "*", SearchOption.TopDirectoryOnly))
                {
                    result.AddRange(childDir.GetFiles(context, searchPattern, searchOption));
                }
            }

            return result;
        }
    }
}

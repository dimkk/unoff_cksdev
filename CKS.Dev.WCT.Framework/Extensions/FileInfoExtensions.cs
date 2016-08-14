using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CKS.Dev.WCT.Framework.Extensions
{
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Returns the local name of the file, starting from the end of the "Path". 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetLocalName(this FileInfo info, string path)
        {
            if(info == null)
            {
                return null;
            }

            string result = info.FullName;

            if (!String.IsNullOrWhiteSpace(path))
            {
                result = result.ReplaceIgnoreCase(path, string.Empty);
                if (result.StartsWithIgnoreCase("/") || result.StartsWithIgnoreCase("\\"))
                {
                    result = result.Substring(1);
                }
            }

            return result;
        }
    }
}

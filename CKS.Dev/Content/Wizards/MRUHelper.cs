using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Globalization;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    /// <summary>
    /// Taken from Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards, Version=10.0.0.0
    /// All rights acknowledged.
    /// </summary>
    class MRUHelper
    { 
        // Fields
        private List<string> _mruUrlList;
        private const int MaxEntries = 10;
        private const string MRUKey = @"Software\Microsoft\VisualStudio\10.0\SharePointTools";
        private const string UrlFormat = "SpUrl{0}";

        // Methods
        private void AddToTopOfMruList(string urlString)
        {
            if (this.MruUrlList.IndexOf(urlString) != 0)
            {
                this.MruUrlList.Remove(urlString);
                while (this.MruUrlList.Count >= 10)
                {
                    this.MruUrlList.RemoveAt(this.MruUrlList.Count - 1);
                }
                this.MruUrlList.Insert(0, urlString);
            }
        }

        private void ClearMruEntriesFromRegistry()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\VisualStudio\10.0\SharePointTools", true))
            {
                if (key != null)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        string str = string.Format(CultureInfo.InvariantCulture, "SpUrl{0}", new object[] { i });
                        key.DeleteValue(str, false);
                    }
                }
            }
        }

        public string GetDefaultUrl()
        {
            return this.MruUrlList[0];
        }

        private static string GetLocalHostUrl()
        {
            UriBuilder builder = new UriBuilder("http", System.Environment.MachineName.ToLowerInvariant());
            return builder.ToString();
        }

        private List<string> GetUrlsFromRegistry()
        {
            string str = string.Empty;
            List<string> list = new List<string>();
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\VisualStudio\10.0\SharePointTools", false))
            {
                if (key != null)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        string str2 = string.Format(CultureInfo.InvariantCulture, "SpUrl{0}", new object[] { i });
                        string str3 = key.GetValue(str2, str) as string;
                        if (((str3 != null) && (str3 != str)) && Uri.IsWellFormedUriString(str3, UriKind.Absolute))
                        {
                            list.Add(str3);
                        }
                    }
                }
            }
            if (list.Count == 0)
            {
                list.Add(GetLocalHostUrl());
            }
            return list;
        }

        private void SaveMruListToRegistry()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\VisualStudio\10.0\SharePointTools"))
            {
                if (key != null)
                {
                    int num = 1;
                    foreach (string str in this.MruUrlList)
                    {
                        string str2 = string.Format(CultureInfo.InvariantCulture, "SpUrl{0}", new object[] { num++ });
                        key.SetValue(str2, str, RegistryValueKind.String);
                    }
                }
            }
        }

        public void SaveUrlToMruList(Uri url)
        {
            string urlString = url.AbsoluteUri;
            this.AddToTopOfMruList(urlString);
            this.ClearMruEntriesFromRegistry();
            this.SaveMruListToRegistry();
        }

        // Properties
        public List<string> MruUrlList
        {
            get
            {
                if (this._mruUrlList == null)
                {
                    this._mruUrlList = this.GetUrlsFromRegistry();
                }
                return this._mruUrlList;
            }
        }
    }
}


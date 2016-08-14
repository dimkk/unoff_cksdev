using System.IO;
using System;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.Deployment;
using Microsoft.VisualStudio.SharePoint.Packages;
using System.ComponentModel.Composition;
using System.DirectoryServices;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Xml;
using CKS.Dev.VisualStudio.SharePoint.Commands;


namespace CKS.Dev.VisualStudio.SharePoint.Deployment.QuickDeployment
{
    /// <summary>
    /// Recycle utilities.
    /// </summary>
    public class RecycleUtilities
    {
        /// <summary>
        /// Recycles all application pools.
        /// </summary>
        /// <param name="service">The service.</param>
        public static void RecycleAllApplicationPools(ISharePointProjectService service)
        {
            string[] names = GetAllApplicationPoolNames(service);
            foreach (string name in names)
            {
                RecycleApplicationPool(name);
            }
        }

        /// <summary>
        /// Recycles the application pool.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static bool RecycleApplicationPool(ISharePointProjectService service, string url)
        {
            string name = GetApplicationPoolName(service, url);
            if (String.IsNullOrEmpty(name))
            {
                return false;
            }
            else
            {
                RecycleApplicationPool(name);
                return true;
            }
        }

        /// <summary>
        /// Recycles the application pool.
        /// </summary>
        /// <param name="name">The name.</param>
        public static void RecycleApplicationPool(string name)
        {
            try
            {
                using (DirectoryEntry appPool = new DirectoryEntry(GetApplicationPoolPath(name)))
                {
                    //appPool.Invoke("Recycle", null);
                    appPool.Invoke("Stop", null);
                    appPool.Invoke("Start", null);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Gets the application pool path.
        /// </summary>
        /// <param name="appPoolName">Name of the app pool.</param>
        /// <returns></returns>
        private static string GetApplicationPoolPath(string appPoolName)
        {
            string path = "IIS://localhost/w3svc/apppools/" + appPoolName;
            return path;
        }

        /// <summary>
        /// Gets the name of the application pool.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string GetApplicationPoolName(ISharePointProjectService service, string url)
        {
            string name = service.SharePointConnection.ExecuteCommand<string, string>(DeploymentSharePointCommandIds.GetApplicationPoolName, url);
            return name;
        }

        /// <summary>
        /// Gets all application pool names.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public static string[] GetAllApplicationPoolNames(ISharePointProjectService service)
        {
            string[] names = service.SharePointConnection.ExecuteCommand<string[]>(DeploymentSharePointCommandIds.GetAllApplicationPoolNames);
            return names;
        }

        /// <summary>
        /// Restarts the IIS.
        /// </summary>
        public static void RestartIIS()
        {
            Process process = new Process();
            process.StartInfo.FileName = System.Environment.SystemDirectory + Path.DirectorySeparatorChar + "iisreset.exe";
            process.StartInfo.Arguments = "localhost";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Build.Evaluation;

namespace CKS.Dev.VisualStudio.SharePoint
{
    /// <summary>
    /// Project utilities.
    /// </summary>
    static class ProjectUtilities
    { 
        /// <summary>
        /// Gets the office server install root.
        /// </summary>
        /// <returns></returns>
        public static string GetOfficeServerInstallRoot()
        {
            return GetHKLMRegistryKeyValue(@"SOFTWARE\Microsoft\Office Server\14.0",
                "InstallPath",
                @"C:\Program Files\Microsoft Office Servers\14.0\");
        }

        /// <summary>
        /// Gets the office server template path.
        /// </summary>
        /// <returns></returns>
        public static string GetOfficeServerTemplatePath()
        {
            return GetHKLMRegistryKeyValue(@"SOFTWARE\Microsoft\Office Server\14.0",
                "TemplatePath",
                @"C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\Template");
        }

        /// <summary>
        /// Gets the office server install root.
        /// </summary>
        /// <returns></returns>
        public static string GetSharePointDesignerInstallRoot()
        {
            return GetHKLMRegistryKeyValue(@"SOFTWARE\Microsoft\Office\14.0\SharePoint Designer\InstallRoot",
                "Path",
                @"C:\Program Files\Microsoft Office\Office14\");
        }

        /// <summary>
        /// Gets the HKLM registry key value.
        /// </summary>
        /// <param name="subKeyName">Name of the sub key.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        private static string GetHKLMRegistryKeyValue(string subKeyName,
            string valueName,
            string defaultValue)
        {
            string value64 = string.Empty;
            string value32 = string.Empty;
            RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
            localKey = localKey.OpenSubKey(subKeyName);
            if (localKey != null)
            {
                value64 = localKey.GetValue(valueName).ToString();
            }

            RegistryKey localKey32 = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
            localKey32 = localKey32.OpenSubKey(subKeyName);
            if (localKey32 != null)
            {
                value32 = localKey32.GetValue(valueName).ToString();
            }

            if (!String.IsNullOrEmpty(value64))
            {
                return value64;
            }
            else if (!String.IsNullOrEmpty(value32))
            {
                return value32;
            }
            else
            {
                return defaultValue;
            }
        }

        public static string GetSafeFileName(string fileName)
        {
            string safeName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fileName);
            safeName = new Regex(@"[^\w]").Replace(safeName, "");
            return safeName;
        }

        public static List<string> GetValueFromCurrentProject(ISharePointProject sharePointProject, string projectPropertyName) {
            List<string> value = null;

            Microsoft.Build.Evaluation.Project project = GetCurrentProject(sharePointProject.FullPath);
            if (project != null) {
                string rawValue = project.GetPropertyValue(projectPropertyName);
                if (!String.IsNullOrEmpty(rawValue)) {
                    value = rawValue.Split(';').ToList();
                }
            }

            return value;
        }

        public static void StoreValueInCurrentProject(List<string> selectedFeaturesIds, ISharePointProject sharePointProject, string projectPropertyName) {
            string value = String.Empty;

            if (selectedFeaturesIds != null && selectedFeaturesIds.Count > 0) {
                value = String.Join(";", selectedFeaturesIds.ToArray());
            }

            Microsoft.Build.Evaluation.Project project = GetCurrentProject(sharePointProject.FullPath);
            if (project != null) {
                project.SetProperty(projectPropertyName, value as string);
            }
        }

        public static Microsoft.Build.Evaluation.Project GetCurrentProject(string projectFilePath) {
            return ProjectCollection.GlobalProjectCollection.GetLoadedProjects(projectFilePath).FirstOrDefault();
        }

        public static ISharePointProjectFeature GetFeature(ISharePointProject project, string projectPath, Guid itemId) {
            ISharePointProjectFeature feature = null;

            ISharePointProject featureProject = project.ProjectService.Projects[projectPath];
            if (featureProject != null) {
                feature = (from ISharePointProjectFeature f
                           in featureProject.Features
                           where f.Id == itemId
                           select f).FirstOrDefault();
            }

            return feature;
        }

        public static IEnumerable<ISharePointProjectFeature> GetFeaturesFromFeatureRefs(ISharePointProject project, IEnumerable<ISharePointProjectMemberReference> features) {
            string currentProjectPath = Path.GetDirectoryName(project.FullPath);

            List<ISharePointProjectFeature> featuresFromPackage = new List<ISharePointProjectFeature>(features.Count());
            foreach (ISharePointProjectMemberReference featureRef in features) {
                string featureProjectPath = String.IsNullOrEmpty(featureRef.ProjectPath) ? project.FullPath : Path.Combine(currentProjectPath, featureRef.ProjectPath);
                featureProjectPath = new DirectoryInfo(featureProjectPath).FullName; // required to get rid of ..\..\ in the project path
                ISharePointProjectFeature feature = GetFeature(project, featureProjectPath, featureRef.ItemId);

                if (feature != null) {
                    featuresFromPackage.Add(feature);
                }
            }

            return featuresFromPackage;
        }

        //TODO: confirm this is ok in the extensions
        //public static string UnTokenize(ISharePointProjectFeature feature, string tokenString)
        //{
        //    if (feature != null)
        //    {
        //        tokenString = tokenString.Replace("$SharePoint.Project.FileNameWithoutExtension$", Path.GetFileNameWithoutExtension(feature.Project.FullPath));
        //        tokenString = tokenString.Replace("$SharePoint.Feature.FileNameWithoutExtension$", Path.GetFileNameWithoutExtension(feature.FullPath));
        //    }
        //    return tokenString;
        //}
    }
}

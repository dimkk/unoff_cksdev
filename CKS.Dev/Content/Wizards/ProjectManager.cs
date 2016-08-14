﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using System.IO;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    internal class StrongNameProjectManager
    {
        StrongNameKey key;
        const string KEY_FILENAME = "key.snk";
        const string PUBLIC_KEY_TOKEN_REPLACEMENT_KEY = "$publickeytoken$";

        internal void AddKeyFileToProject(EnvDTE.Project project)
        {
            if (this.key != null)
            {
                string destinationFileName = Path.Combine(Path.GetDirectoryName(project.FullName), "key.snk");
                this.key.SaveTo(destinationFileName);
                project.ProjectItems.AddFromFile(destinationFileName);
                EnvDTE.Properties properties = project.Properties;
                properties.Item("SignAssembly").Value = true;
                properties.Item("AssemblyOriginatorKeyFile").Value = "key.snk";
            }
        }

        internal void AddKeyToDictionary(Dictionary<string, string> replacementsDictionary)
        {
            string publicKeyToken = this.key.GetPublicKeyToken();
            replacementsDictionary.Add("$publickeytoken$", publicKeyToken);
        }

        internal void GenerateKey()
        {
            this.key = StrongNameKey.CreateNewKeyPair();
        }

        internal bool GetPublicKey(EnvDTE.Project project)
        {
            EnvDTE.Properties projectProps = project.Properties;
            if (!HasPublicKey(projectProps))
            {
                return false;
            }
            string str = (string)projectProps.Item("AssemblyOriginatorKeyFile").Value;
            if (str != null)
            {
                string fullPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(project.FileName), str));
                this.key = StrongNameKey.Load(fullPath);
            }
            else
            {
                string keyContainer = (string)projectProps.Item("AssemblyKeyContainerName").Value;
                this.key = StrongNameKey.LoadContainer(keyContainer);
            }
            return true;
        }

        private static bool HasPublicKey(EnvDTE.Properties projectProps)
        {
            if (projectProps.Item("AssemblyOriginatorKeyFile").Value == null)
            {
                return (projectProps.Item("AssemblyKeyContainerName").Value != null);
            }
            return true;
        }
    }
}


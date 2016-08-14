using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev.VisualStudio.SharePoint.Commands
{
    class TagInfo
    {
        public string AssemblyName { get; set; }
        public string Name { get; set; }
        public string NamespaceName { get; set; }
        public string TagName
        {
            get
            {
                string tagName = Name;
                if (ContentTypeSharePointCommands.CustomTagPrefixMappings.ContainsKey(NamespaceName))
                {
                    tagName = ContentTypeSharePointCommands.CustomTagPrefixMappings[NamespaceName];
                }
                else if (ContentTypeSharePointCommands.TagPrefixMappings.ContainsKey(NamespaceName))
                {
                    tagName = ContentTypeSharePointCommands.TagPrefixMappings[NamespaceName];
                }

                return tagName;
            }
        }

        public TagInfo(string name, string assemblyName, string namespaceName)
        {
            Name = name;
            AssemblyName = assemblyName;
            NamespaceName = namespaceName;
        }

        public override string ToString()
        {
            return String.Format("<%@ Register Tagprefix=\"{0}\" Namespace=\"{1}\" Assembly=\"{2}\"  %>", TagName, NamespaceName, AssemblyName);
        }
    }
}

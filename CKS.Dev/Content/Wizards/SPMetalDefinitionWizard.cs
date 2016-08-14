using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System.IO;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    class SPMetalDefinitionWizard : BaseWizard
    {
        Dictionary<string, string> _customItemProperties;



        public override void SetProjectProperties(EnvDTE.Project project)
        {
            
        }

        protected override bool IsSharePointConnectionRequired
        {
            get { return true; }
        }

        public override IWizardFormExtension CreateWizardForm(DTE designTimeEnvironment, WizardRunKind runKind)
        {
            //We have no need for a UI so return null
            return null;
        }




        public override void RunProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            base.RunProjectItemFinishedGenerating(projectItem);

            if (projectItem != null && _customItemProperties != null)
            {
                if (String.Equals(Path.GetExtension(projectItem.Name), ".xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    EnvDTE.Properties properties = projectItem.Properties;
                    if (properties != null)
                    {
                        foreach (KeyValuePair<string, string> customProperty in _customItemProperties)
                        {
                            EnvDTE.Property property = properties.Item(customProperty.Key);
                            property.Value = customProperty.Value;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Should add project item
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>Returns true</returns>
        public override bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public override void InitializeFromWizardData(Dictionary<string, string> replacementsDictionary)
        {
            base.InitializeFromWizardData(replacementsDictionary);

            _customItemProperties = null;
            if (replacementsDictionary != null)
            {
                if (replacementsDictionary.ContainsKey("$rootname$"))
                {
                    replacementsDictionary.Add("$subnamespace$", WizardHelpers.MakeNameCompliant(replacementsDictionary["$rootname$"]));
                }
                if (replacementsDictionary.ContainsKey("$itemproperties$"))
                {
                    _customItemProperties = new Dictionary<string, string>();
                    foreach (string propertyName in replacementsDictionary["$itemproperties$"].Split(','))
                    {
                        string propertyValueKey = "$" + propertyName + "$";
                        if (replacementsDictionary.ContainsKey(propertyValueKey))
                        {
                            _customItemProperties.Add(propertyName, replacementsDictionary[propertyValueKey]);
                        }
                    }
                }
            }

        }
    }
}

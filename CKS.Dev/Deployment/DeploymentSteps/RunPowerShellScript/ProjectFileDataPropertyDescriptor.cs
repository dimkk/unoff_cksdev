using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Evaluation;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps.RunPowerShellScript
{
    internal class ProjectFileDataPropertyDescriptor : PropertyDescriptor
    {
        private string value;
        private string defaultValue;
        private bool valueRetrieved = false;
        private ProjectFileDataPropertyInfo propertyInfo;
        private static Regex invalidMsBuildChars = new Regex("[^a-zA-Z0-9_]", RegexOptions.Compiled);

        public ProjectFileDataPropertyDescriptor(ProjectFileDataPropertyInfo propertyInfo)
            : base(propertyInfo.DisplayName, propertyInfo.Attributes)
        {
            this.propertyInfo = propertyInfo;
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override void ResetValue(object component)
        {
            SetValue(component, propertyInfo.Value);
        }

        public override object GetValue(object component)
        {
            if (!valueRetrieved && propertyInfo.Project != null)
            {
                Microsoft.Build.Evaluation.Project project = GetCurrentProject(propertyInfo.Project.FullPath);
                if (project != null)
                {
                    value = project.GetPropertyValue(EscapeMsBuildString(propertyInfo.Name));
                    defaultValue = value;
                }

                valueRetrieved = true;
            }

            return value;
        }

        public static Microsoft.Build.Evaluation.Project GetCurrentProject(string projectFilePath)
        {
            return ProjectCollection.GlobalProjectCollection.GetLoadedProjects(projectFilePath).FirstOrDefault();
        }

        public override void SetValue(object component, object value)
        {
            if (propertyInfo.Project != null)
            {
                Microsoft.Build.Evaluation.Project project = GetCurrentProject(propertyInfo.Project.FullPath);
                if (project != null)
                {
                    project.SetProperty(EscapeMsBuildString(propertyInfo.Name), value as string);
                    this.value = value as string;
                }
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return value != defaultValue;
        }

        public override Type ComponentType
        {
            get { return typeof(object); }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return typeof(string); }
        }

        public static string EscapeMsBuildString(string value)
        {
            return invalidMsBuildChars.Replace(value, "_");
        }
    }
}

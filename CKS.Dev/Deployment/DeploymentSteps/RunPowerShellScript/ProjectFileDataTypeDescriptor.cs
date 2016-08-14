using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps.RunPowerShellScript
{
    internal class ProjectFileDataTypeDescriptor : CustomTypeDescriptor
    {
        private IEnumerable<ProjectFileDataPropertyInfo> customProperties;
        public string PropertiesCategory { get; set; }

        public ProjectFileDataTypeDescriptor(ICustomTypeDescriptor parent, IEnumerable<ProjectFileDataPropertyInfo> properties)
            : base(parent)
        {
            customProperties = properties;
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection descriptors = new PropertyDescriptorCollection(null);
            if (customProperties != null)
            {
                foreach (ProjectFileDataPropertyInfo property in customProperties)
                {
                    descriptors.Add(new ProjectFileDataPropertyDescriptor(property));
                }
            }

            return descriptors;
        }
    }
}

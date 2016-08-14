using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps.RunPowerShellScript
{
    internal class ProjectFileDataDescriptionProvider : TypeDescriptionProvider
    {
        private IEnumerable<ProjectFileDataPropertyInfo> customProperties;

        public ProjectFileDataDescriptionProvider(IEnumerable<ProjectFileDataPropertyInfo> properties)
            : base(TypeDescriptor.GetProvider(typeof(object)))
        {
            customProperties = properties;
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            if (instance != null)
            {
                return new ProjectFileDataTypeDescriptor(base.GetTypeDescriptor(objectType, instance), customProperties);
            }

            return base.GetTypeDescriptor(objectType, instance);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps.RunPowerShellScript
{
    internal class ProjectFileDataPropertyInfo
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Category { get; set; }
        public ISharePointProject Project { get; set; }
        public Attribute[] Attributes
        {
            get
            {
                List<Attribute> attributes = new List<Attribute>(1);

                if (!String.IsNullOrEmpty(Category))
                {
                    attributes.Add(new CategoryAttribute(Category));
                }

                return attributes.ToArray();
            }
        }
    }
}

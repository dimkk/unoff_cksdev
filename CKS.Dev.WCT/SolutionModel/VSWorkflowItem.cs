using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Common;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.WCT.SolutionModel
{
    public class VSWorkflowItem : VSFeatureItem
    {

        private WorkflowDefinition _workflow = null;
        public WorkflowDefinition Workflow
        {
            get { return _workflow; }
            set
            {
                _workflow = value;
                if (_workflow != null && !String.IsNullOrEmpty(_workflow.Name))
                {
                    this.Name = _workflow.Name;
                }
            }
        }

        public VSWorkflowItem(FeatureDefinition feature)
        {
            this.Feature = feature;
            this.Name = "Workflow_" + Guid.NewGuid();
            this.TypeName = Constants.SPTypeNameWorkflow;
            this.GroupName = "Workflows";
            this.SupportedDeploymentScopes = "Site";
            this.DefaultDeploymentType = DeploymentType.ElementFile;
        }

        public VSWorkflowItem(FeatureDefinition feature, WorkflowDefinition workflow) : this(feature)
        {
            this.Workflow = workflow;
        }


        public override ElementDefinitionCollection GetElementManifest()
        {
            ElementDefinitionCollection elements = new ElementDefinitionCollection();

            elements.Items = new object[] { this.Workflow };

            return elements;
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.SharePoint;
using CKS.Dev.VisualStudio.SharePoint.Properties;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps.RunPowerShellScript
{
    /// <summary>
    /// Project extension for running powershell scripts
    /// </summary>
    [Export(typeof(ISharePointProjectExtension))]
    public class RunPowerShellScriptProjectExtension : ISharePointProjectExtension
    {
        /// <summary>
        /// Initializes the SharePoint project extension.
        /// </summary>
        /// <param name="projectService">An instance of SharePoint project service.</param>
        public void Initialize(ISharePointProjectService projectService)
        {
            projectService.ProjectPropertiesRequested += new EventHandler<SharePointProjectPropertiesRequestedEventArgs>(projectService_ProjectPropertiesRequested);
        }

        /// <summary>
        /// Handles the ProjectPropertiesRequested event of the projectService control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.SharePoint.SharePointProjectPropertiesRequestedEventArgs"/> instance containing the event data.</param>
        void projectService_ProjectPropertiesRequested(object sender, SharePointProjectPropertiesRequestedEventArgs e)
        {
            if (e.Project.DeploymentConfigurations.ContainsKey(e.Project.ActiveDeploymentConfiguration)) {
                int numPSTasks = e.Project.DeploymentConfigurations[e.Project.ActiveDeploymentConfiguration].DeploymentSteps.Count(p => p.Equals(CustomDeploymentStepIds.RunPowerShellScriptDeploymentStep));
                if (numPSTasks > 0) {
                    List<ProjectFileDataPropertyInfo> psScripts = new List<ProjectFileDataPropertyInfo>(numPSTasks);
                    for (int i = 0; i < numPSTasks; i++) {
                        int scriptNo = i + 1;
                        psScripts.Add(new ProjectFileDataPropertyInfo {
                            DisplayName = String.Format(Resources.RunPowerShellScriptProjectExtension_StepDisplayName, scriptNo),
                            Name = RunPowerShellScriptDeploymentStep.GetSettingKey(e.Project, scriptNo),
                            Project = e.Project,
                            Category = Resources.RunPowerShellScriptProjectExtension_StepCategory
                        });
                    }

                    e.PropertySources.Add(CreatePropertySourceObject(psScripts));
                }
            }
        }

        /// <summary>
        /// Creates the property source object.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        static object CreatePropertySourceObject(IEnumerable<ProjectFileDataPropertyInfo> properties)
        {
            object instance = new object();
            TypeDescriptor.AddProvider(new ProjectFileDataDescriptionProvider(properties), instance);
            return instance;
        }
    }
}

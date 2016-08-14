using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps
{
    /// <summary>
    /// SharePoint Project Feature
    /// </summary>
    public class SharePointProjectFeature
    {
        /// <summary>
        /// Gets or sets the feature.
        /// </summary>
        /// <value>The feature.</value>
        public ISharePointProjectFeature Feature { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointProjectFeature"/> class.
        /// </summary>
        /// <param name="feature">The feature.</param>
        public SharePointProjectFeature(ISharePointProjectFeature feature)
        {
            Feature = feature;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() 
        {
            return String.Format("{0} ({1})", Feature.Model.Title, Feature.Project.Name);
        }
    }
}

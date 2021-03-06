﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if VS2012Build_SYMBOL
    namespace CKS.Dev11.VisualStudio.SharePoint.Commands.Info
#elif VS2013Build_SYMBOL
namespace CKS.Dev12.VisualStudio.SharePoint.Commands.Info
#elif VS2014Build_SYMBOL
    namespace CKS.Dev13.VisualStudio.SharePoint.Commands.Info
#else
    namespace CKS.Dev.VisualStudio.SharePoint.Commands.Info
#endif
    {
    /// <summary>
    /// Contains basic data about a feature dependancy on the SharePoint site. This class is 
    /// serializable so that instances of it can be sent between the Visual Studio and 
    /// SharePoint command assemblies.
    /// </summary>
    [Serializable]
    public class FeatureDependencyInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the minimum version.
        /// </summary>
        public string MinimumVersion { get; set; }

        /// <summary>
        /// Gets or sets the feature Id.
        /// </summary>
        public Guid FeatureID { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        public FeatureScope Scope { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps;
using Microsoft.VisualStudio.SharePoint;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards.Dialogs 
{
    /// <summary>
    /// The feature picker.
    /// </summary>
    public partial class FeaturesPickerDialog : Form
    {
        /// <summary>
        /// Gets the selected features.
        /// </summary>
        /// <value>The selected features.</value>
        public IEnumerable<ISharePointProjectFeature> SelectedFeatures 
        {
            get 
            {
                List<ISharePointProjectFeature> selectedFeatures = new List<ISharePointProjectFeature>();

                foreach (object item in featuresPicker.SelectedItems) 
                {
                    if (item is SharePointProjectFeature) {
                        selectedFeatures.Add(((SharePointProjectFeature)item).Feature);
                    }
                }

                return selectedFeatures;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeaturesPickerDialog"/> class.
        /// </summary>
        /// <param name="featuresFromPackage">The features from package.</param>
        /// <param name="selectedFeaturesIds">The selected features ids.</param>
        public FeaturesPickerDialog(IEnumerable<ISharePointProjectFeature> featuresFromPackage, 
            IEnumerable<Guid> selectedFeaturesIds) 
        {
            InitializeComponent();

            List<SharePointProjectFeature> features = new List<SharePointProjectFeature>(featuresFromPackage.Count());
            foreach (ISharePointProjectFeature f in featuresFromPackage) 
            {
                features.Add(new SharePointProjectFeature(f));
            }

            FillFeaturesPicker(features, selectedFeaturesIds);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeaturesPickerDialog"/> class.
        /// </summary>
        /// <param name="featuresFromPackage">The features from package.</param>
        /// <param name="selectedFeaturesIds">The selected features ids.</param>
        public FeaturesPickerDialog(IEnumerable<SharePointProjectFeature> featuresFromPackage,
            IEnumerable<Guid> selectedFeaturesIds)
        {
            InitializeComponent();

            FillFeaturesPicker(featuresFromPackage, selectedFeaturesIds);
        }

        /// <summary>
        /// Fills the features picker.
        /// </summary>
        /// <param name="featuresFromPackage">The features from package.</param>
        /// <param name="selectedFeaturesIds">The selected features ids.</param>
        private void FillFeaturesPicker(IEnumerable<SharePointProjectFeature> featuresFromPackage, 
            IEnumerable<Guid> selectedFeaturesIds) 
        {
            List<SharePointProjectFeature> availableFeatures = new List<SharePointProjectFeature>(featuresFromPackage);

            if (selectedFeaturesIds != null) 
            {
                List<SharePointProjectFeature> selectedFeatures = new List<SharePointProjectFeature>(selectedFeaturesIds.Count());

                foreach (Guid featureId in selectedFeaturesIds)
                {
                    SharePointProjectFeature feature = (from SharePointProjectFeature f
                                                        in availableFeatures
                                                        where f.Feature.Id.Equals(featureId)
                                                        select f).FirstOrDefault();

                    if (feature != null) {
                        selectedFeatures.Add(feature);
                        availableFeatures.Remove(feature);
                    }
                }

                featuresPicker.SelectedItems = selectedFeatures;
            }

            featuresPicker.AvailableItems = availableFeatures;
            featuresPicker.SelectedItemsLabel = "Features to activate:";
            featuresPicker.AvailableItemsLabel = "Features in the package:";
        }
    }
}

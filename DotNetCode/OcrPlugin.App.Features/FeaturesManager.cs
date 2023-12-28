using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcrPlugin.App.Features
{
    internal sealed class FeaturesManager : IFeaturesManager
    {
        private readonly ICompanyFeatureProvider _companyFeatureProvider;
        private readonly IEnumerable<Feature> _featuresSettings;

        public FeaturesManager(
            IOptions<FeaturesSettings> options,
            ICompanyFeatureProvider companyFeatureProvider)
        {
            _companyFeatureProvider = companyFeatureProvider;
            _featuresSettings = options.Value.Features?.Select(Enum.Parse<Feature>) ?? Enumerable.Empty<Feature>();
        }

        public async Task<bool> IsEnabled(Feature feature, string companyName)
        {
            var companyFeatures = await _companyFeatureProvider.GetAll(companyName);

            return _featuresSettings.Contains(feature) || companyFeatures.Contains(feature);
        }

        public async Task<bool> IsDisabled(Feature feature, string companyName) => !await IsEnabled(feature, companyName);
    }
}
using OcrPlugin.App.Azure.Storage.Features;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Features
{
    internal sealed class CompanyFeatureProvider : ICompanyFeatureProvider
    {
        private readonly IFeaturesStorage _featuresStorage;

        public CompanyFeatureProvider(IFeaturesStorage featuresStorage)
        {
            _featuresStorage = featuresStorage;
        }

        public async Task<IEnumerable<Feature>> GetAll(string companyName)
        {
            return (await _featuresStorage.GetCompanyFeatures(companyName)).MapToFeatures();
        }
    }
}
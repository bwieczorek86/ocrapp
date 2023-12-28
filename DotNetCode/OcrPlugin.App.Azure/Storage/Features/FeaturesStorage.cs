using Microsoft.Extensions.Logging;
using OcrPlugin.App.Azure.Common.CloudTableStorage;
using OcrPlugin.App.Azure.Common.Constants;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.Features
{
    public class FeaturesStorage : StorageBase, IFeaturesStorage
    {
        public FeaturesStorage(
            ILoggerFactory loggerFactory,
            ICloudTableClientFactoryResolver cloudTableClientFactoryResolver)
            : base(loggerFactory, cloudTableClientFactoryResolver.Resolve(StorageAccountType.Production))
        {
        }

        public async Task<CompanyFeaturesEntity> GetCompanyFeatures(string companyName)
        {
            return await RetrieveEntity<CompanyFeaturesEntity>(PartitionKeys.Features, RowKeys.Features, companyName);
        }
    }
}
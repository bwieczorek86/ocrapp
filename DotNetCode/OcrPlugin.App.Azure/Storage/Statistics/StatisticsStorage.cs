using Microsoft.Extensions.Logging;
using OcrPlugin.App.Azure.Common.CloudTableStorage;
using OcrPlugin.App.Azure.Common.Constants;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.Statistics
{
    public class StatisticsStorage : StorageBase, IStatisticsStorage
    {
        public StatisticsStorage(
            ILoggerFactory loggerFactory,
            ICloudTableClientFactoryResolver cloudTableClientFactoryResolver)
            : base(loggerFactory, cloudTableClientFactoryResolver.Resolve(StorageAccountType.Production))
        {
        }

        public async Task<OcrStatisticsEntity> GetCompanyStatistics(string companyName)
        {
            return await RetrieveEntity<OcrStatisticsEntity>(PartitionKeys.Statistics, RowKeys.OcrStatistics, companyName);
        }

        public async Task Upsert(OcrStatisticsEntity statisticsEntity, string companyName)
        {
            await Upsert<OcrStatisticsEntity>(statisticsEntity, companyName);
        }
    }
}
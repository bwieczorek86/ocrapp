using Microsoft.Extensions.Logging;
using OcrPlugin.App.Azure.Common.CloudTableStorage;
using OcrPlugin.App.Azure.Common.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.Reports
{
    public class ReportsStorage : StorageBase, IReportsStorage
    {
        public ReportsStorage(
            ILoggerFactory loggerFactory,
            ICloudTableClientFactoryResolver cloudTableClientFactoryResolver)
            : base(loggerFactory, cloudTableClientFactoryResolver.Resolve(StorageAccountType.Production))
        {
        }

        public async Task<ReportEntity> GetReportEntity(string reportId, string fileId, string companyName)
        {
            return await RetrieveEntity<ReportEntity>(reportId, fileId, GetTableName(companyName));
        }

        public async Task<IReadOnlyCollection<ReportEntity>> FindOcrResults(string reportId, string companyName)
        {
            return await RetrieveRangeQuery<ReportEntity>("Report", reportId, GetTableName(companyName));
        }

        public async Task Upsert(ReportEntity reportEntity, string companyName)
        {
            await base.Upsert(reportEntity, GetTableName(companyName));
        }

        public async Task Merge(UserDataReportUpdateEntity reportEntity, string companyName)
        {
            await base.Merge(reportEntity, GetTableName(companyName));
        }

        private string GetTableName(string companyName) => $"{companyName}Reports";
    }
}
using Microsoft.Extensions.Logging;
using OcrPlugin.App.Azure.Common.CloudTableStorage;
using OcrPlugin.App.Azure.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.AppBlazor
{
    public class AppBlazorStorage : StorageBase, IAppBlazorStorage
    {
        private string TableName { get; set; } = Table.Base.AppBlazor;

        public AppBlazorStorage(
            ILoggerFactory loggerFactory,
            ICloudTableClientFactoryResolver cloudTableClientFactoryResolver)
            : base(loggerFactory, cloudTableClientFactoryResolver.Resolve(StorageAccountType.Production))
        {
        }

        public async Task<IEnumerable<SoftlexIntegrationConfig>> GetSoftlexIntegrations()
        {
            return (await GetAppBlazorEntity()).SoftlexIntegrations;
        }

        public async Task SetSoftlexLastIntegrationDate(string companyName, DateTime integrationDate)
        {
            var appBlazorEntity = await GetAppBlazorEntity();
            var softlexIntegrationConfig = appBlazorEntity.SoftlexIntegrations.FirstOrDefault(c => c.CompanyName == companyName);

            softlexIntegrationConfig!.LastIntegrationDate = integrationDate;

            await Upsert(appBlazorEntity, companyName);
        }

        private async Task<AppBlazorEntity> GetAppBlazorEntity()
        {
            return (await RetrieveEntity<AppBlazorEntity>(PartitionKeys.AppBlazor.Settings, RowKeys.AppBlazor.IntegrationSoftlex, TableName));
        }
    }
}
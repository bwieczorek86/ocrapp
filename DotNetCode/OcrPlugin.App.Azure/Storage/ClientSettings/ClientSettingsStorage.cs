using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using OcrPlugin.App.Azure.Common.CloudTableStorage;
using OcrPlugin.App.Azure.Common.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.ClientSettings
{
    public class ClientSettingsStorage : StorageBase, IClientSettingsStorage
    {
        public ClientSettingsStorage(
            ILoggerFactory loggerFactory,
            ICloudTableClientFactoryResolver cloudTableClientFactoryResolver)
            : base(loggerFactory, cloudTableClientFactoryResolver.Resolve(StorageAccountType.Production))
        {
        }

        public async Task<CompanySettingsEntity> FindByName(string name, string companyName)
        {
            return await RetrieveEntity<CompanySettingsEntity>(PartitionKeys.Setting, name, companyName);
        }

        public async Task Upsert(CompanySettingsEntity companySettingsEntity, string companyName)
        {
            await base.Upsert(companySettingsEntity, companyName);
        }

        public async Task<IEnumerable<CompanySettingsEntity>> GetAll(string companyName)
        {
            return await RetrieveEntities<CompanySettingsEntity>(PartitionKeys.Setting, companyName);
        }

        public async Task Delete(string property, string companyName)
        {
            await Delete(new TableEntity(PartitionKeys.Setting, property), companyName);
        }
    }
}
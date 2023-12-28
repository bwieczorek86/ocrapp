using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using OcrPlugin.App.Azure.Common.CloudTableStorage;
using OcrPlugin.App.Azure.Common.Constants;
using OcrPlugin.App.Azure.Storage.Templates.MergeObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.Templates
{
    public class TemplatesStorage : StorageBase, ITemplatesStorage
    {
        public TemplatesStorage(
            ILoggerFactory loggerFactory,
            ICloudTableClientFactoryResolver cloudTableClientFactoryResolver)
            : base(loggerFactory, cloudTableClientFactoryResolver.Resolve(StorageAccountType.Production))
        {
        }

        public async Task<TemplateEntity> FindByName(string name, string companyName)
        {
            return await RetrieveEntity<TemplateEntity>(PartitionKeys.TemplateEntity, name, companyName);
        }

        public async Task ChangeName(TemplateEntity template, string newName, string companyName)
        {
            var entity = await RetrieveEntity<TemplateEntity>(template.PartitionKey, template.RowKey, companyName);
            entity.RowKey = newName;
            await Upsert(entity, companyName);

            template.ETag = "*";
            await Delete(template, companyName);
        }

        public async Task Upsert(TemplateEntity templateEntity, string companyName)
        {
            await base.Upsert(templateEntity, companyName);
        }

        public async Task Upsert(UpdateTemplateObject templateEntity, string companyName)
        {
            await Merge(templateEntity, companyName);
        }

        public async Task Activate(string templateName, string companyName)
        {
            var mergeObject = new ActivateTemplateObject(templateName);

            await Merge(mergeObject, companyName);
        }

        public async Task Deactivate(string templateName, string companyName)
        {
            var mergeObject = new DeactivateTemplateObject(templateName);

            await Merge(mergeObject, companyName);
        }

        public async Task<IEnumerable<TemplateEntity>> GetAll(string companyName)
        {
            return await RetrieveEntities<TemplateEntity>(PartitionKeys.TemplateEntity, companyName);
        }

        public async Task Delete(string templateName, string companyName)
        {
            await Delete(new TableEntity(PartitionKeys.TemplateEntity, templateName), companyName);
        }

        public async Task Merge(UpdateTemplateRankObject toTemplateRankUpdateEntity, string companyName)
        {
            await base.Merge(toTemplateRankUpdateEntity, companyName);
        }
    }
}
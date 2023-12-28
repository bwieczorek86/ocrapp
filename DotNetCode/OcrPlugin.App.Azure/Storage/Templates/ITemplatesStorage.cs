using OcrPlugin.App.Azure.Storage.Templates.MergeObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.Templates
{
    public interface ITemplatesStorage
    {
        Task<TemplateEntity> FindByName(string name, string companyName);
        Task ChangeName(TemplateEntity template, string newName, string companyName);
        Task Upsert(TemplateEntity templateEntity, string companyName);
        Task Upsert(UpdateTemplateObject templateEntity, string companyName);
        Task Activate(string templateName, string companyName);
        Task Deactivate(string templateName, string companyName);
        Task<IEnumerable<TemplateEntity>> GetAll(string companyName);
        Task Delete(string templateName, string companyName);
        Task Merge(UpdateTemplateRankObject toTemplateRankUpdateEntity, string companyName);
    }
}
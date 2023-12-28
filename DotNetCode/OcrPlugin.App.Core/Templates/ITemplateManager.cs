using OcrPlugin.App.BlazorClient.Shared.Templates;
using OcrPlugin.App.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.Templates
{
    public interface ITemplateManager
    {
        Task<Template> Get(string name, string companyName);
        Task Create(Template template, string companyName);
        Task Update(Template template, string companyName, string newName = null);
        Task Activate(string templateName, string companyName);
        Task Deactivate(string templateName, string companyName);
        Task Delete(Template template, string companyName);
        Task<IReadOnlyCollection<Template>> GetAll(string companyName);
        Task IncreaseRank(Template template, string companyName);
        Task<IReadOnlyCollection<Template>> GetAllRanked(string companyName);
    }
}
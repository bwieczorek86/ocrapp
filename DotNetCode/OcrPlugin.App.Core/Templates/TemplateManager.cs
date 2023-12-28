using Microsoft.AspNetCore.Http;
using OcrPlugin.App.Azure.Blobs;
using OcrPlugin.App.Azure.Storage.Templates;
using OcrPlugin.App.BlazorClient.Shared.Templates;
using OcrPlugin.App.Common;
using OcrPlugin.App.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.Templates
{
    internal sealed class TemplateManager : ITemplateManager
    {
        private readonly ITemplatesStorage _templatesStorage;
        private readonly IBlobManager _blobManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TemplateManager(
            ITemplatesStorage templatesStorage,
            IBlobManager blobManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _templatesStorage = templatesStorage;
            _blobManager = blobManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Template> Get(string name, string companyName)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var formattedName = name.Replace('+', ' ');

            return (await _templatesStorage.FindByName(formattedName, companyName))?
                .ToTemplate();
        }

        public async Task Create(Template template, string companyName)
        {
            await _templatesStorage.Upsert(template.ToTemplateEntity(), companyName);
        }

        public async Task Update(Template template, string companyName, string newName = null)
        {
            if (newName != null)
            {
                await _templatesStorage.ChangeName(template.ToTemplateEntity(), newName, companyName);
            }
            else
            {
                await _templatesStorage.Upsert(template.ToTemplateUpdateEntity(), companyName);
            }
        }

        public async Task Activate(string templateName, string companyName)
        {
            await _templatesStorage.Activate(templateName, companyName);
        }

        public async Task Deactivate(string templateName, string companyName)
        {
            await _templatesStorage.Deactivate(templateName, companyName);
        }

        public async Task Delete(Template template, string companyName)
        {
            await _templatesStorage.Delete(template.Name, companyName);
            await _blobManager.Delete(template.FileName, companyName);
        }

        public async Task<IReadOnlyCollection<Template>> GetAll(string companyName)
        {
            return (await _templatesStorage.GetAll(companyName))
                .Select(z => z.ToTemplate())
                .ToList();
        }

        public async Task IncreaseRank(Template template, string companyName)
        {
            template.Rank += 1;
            await _templatesStorage.Merge(template.ToTemplateRankUpdateEntity(), companyName);
        }

        public async Task<IReadOnlyCollection<Template>> GetAllRanked(string companyName)
        {
            var collection = (await _templatesStorage.GetAll(companyName))
                .Where(z => z.IsActive)
                .Select(z => z.ToTemplate())
                .OrderByDescending(z => z.Rank).ToList();

            return collection;
        }
    }
}
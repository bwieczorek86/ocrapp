using OcrPlugin.App.Azure.Storage.ClientSettings;
using OcrPlugin.App.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.AppSettings
{
    public class SettingsManager : ISettingsManager
    {
        private readonly IClientSettingsStorage _clientSettingsStorage;

        public SettingsManager(IClientSettingsStorage clientSettingsStorage)
        {
            _clientSettingsStorage = clientSettingsStorage;
        }

        public async Task<CompanySettings> Get(string property, string companyName)
        {
            return (await _clientSettingsStorage.FindByName(property, companyName)).ToCompanySetting();
        }

        public async Task Create(CompanySettings companySettings, string companyName)
        {
            await _clientSettingsStorage.Upsert(companySettings.ToCompanySettingEntity(), companyName);
        }

        public async Task Update(CompanySettings companySettings, string companyName)
        {
            await _clientSettingsStorage.Upsert(companySettings.ToCompanySettingEntity(), companyName);
        }

        public async Task Delete(string property, string companyName)
        {
            await _clientSettingsStorage.Delete(property, companyName);
        }

        public async Task<ICollection<CompanySettings>> GetAll(string companyName)
        {
            return (await _clientSettingsStorage.GetAll(companyName)).Select(SettingsMapper.ToCompanySetting).ToList();
        }
    }
}
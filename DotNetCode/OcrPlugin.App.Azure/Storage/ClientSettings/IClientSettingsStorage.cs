using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.ClientSettings
{
    public interface IClientSettingsStorage
    {
        Task<CompanySettingsEntity> FindByName(string name, string companyName);
        Task Upsert(CompanySettingsEntity companySettingsEntity, string companyName);
        Task<IEnumerable<CompanySettingsEntity>> GetAll(string companyName);
        Task Delete(string property, string companyName);
    }
}
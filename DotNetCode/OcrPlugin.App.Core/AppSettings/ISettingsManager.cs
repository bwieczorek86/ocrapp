using OcrPlugin.App.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.AppSettings
{
    public interface ISettingsManager
    {
        Task<CompanySettings> Get(string property, string companyName);
        Task Create(CompanySettings companySettings, string companyName);
        Task Update(CompanySettings companySettings, string companyName);
        Task Delete(string property, string companyName);
        Task<ICollection<CompanySettings>> GetAll(string companyName);
    }
}
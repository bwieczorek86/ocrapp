using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.Features
{
    public interface IFeaturesStorage
    {
        Task<CompanyFeaturesEntity> GetCompanyFeatures(string companyName);
    }
}
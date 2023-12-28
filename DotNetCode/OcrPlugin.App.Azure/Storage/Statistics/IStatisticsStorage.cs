using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.Statistics
{
    public interface IStatisticsStorage
    {
        Task<OcrStatisticsEntity> GetCompanyStatistics(string companyName);
        Task Upsert(OcrStatisticsEntity statisticsEntity, string companyName);
    }
}
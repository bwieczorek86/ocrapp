using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.Reports
{
    public interface IReportsStorage
    {
        Task<ReportEntity> GetReportEntity(string reportId, string fileId, string companyName);
        Task<IReadOnlyCollection<ReportEntity>> FindOcrResults(string reportId, string companyName);
        Task Upsert(ReportEntity templateEntity, string companyName);
        Task Merge(UserDataReportUpdateEntity templateEntity, string companyName);
    }
}
using OcrPlugin.App.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.Reports
{
    public interface IReportsManager
    {
        Task<OcrResult> GetOcrResult(string reportId, string fileId, string companyName);
        Task<Report> Get(string reportId, string companyName);
        Task<IReadOnlyCollection<Report>> GetAll(string companyName);
        Task Create(Report report, string companyName);
        Task Create(OcrResult ocrResult, string companyName);
        Task Update(OcrResult ocrResult, string companyName);
        Task<IEnumerable<OcrResult>> GetAllOcrResults(string reportId, string companyName);
    }
}
using Newtonsoft.Json;
using OcrPlugin.App.Azure.Blobs;
using OcrPlugin.App.Azure.Storage.Reports;
using OcrPlugin.App.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.Reports
{
    internal sealed class ReportsManager : IReportsManager
    {
        private static readonly string ContainerSuffix = "-reports";

        private readonly IReportsStorage _reportsStorage;
        private readonly IBlobManager _blobManager;

        public ReportsManager(
            IReportsStorage reportsStorage,
            IBlobManager blobManager)
        {
            _reportsStorage = reportsStorage;
            _blobManager = blobManager;
        }

        public async Task<IReadOnlyCollection<Report>> GetAll(string companyName)
        {
            var binaryDatas = await _blobManager.GetBinaryData(ContainerName(companyName));
            var reports = binaryDatas.Select(binaryData => JsonConvert.DeserializeObject<Report>(binaryData.ToString()));

            return reports.ToList();
        }

        public async Task Create(Report report, string companyName)
        {
            var json = JsonConvert.SerializeObject(report);
            var binaryData = new BinaryData(json);

            await _blobManager.Upload(report.Id, binaryData, ContainerName(companyName));
        }

        public async Task Create(OcrResult ocrResult, string companyName)
        {
            await _reportsStorage.Upsert(ocrResult.ToReportEntity(), companyName);
        }

        public async Task Update(OcrResult ocrResult, string companyName)
        {
            await _reportsStorage.Merge(ocrResult.ToUserDataReportUpdateEntity(), companyName);
        }

        public async Task<Report> Get(string reportId, string companyName)
        {
            var reportData = await _blobManager.GetBinaryData(reportId, ContainerName(companyName));
            var @string = reportData.ToString();

            return JsonConvert.DeserializeObject<Report>(@string);
        }

        public async Task<OcrResult> GetOcrResult(string reportId, string fileId, string companyName)
        {
            var ocrResult = (await _reportsStorage.GetReportEntity(reportId, fileId, companyName)).ToOcrResult();

            return ocrResult;
        }

        public async Task<IEnumerable<OcrResult>> GetAllOcrResults(string reportId, string companyName)
        {
            var ocrResults = (await _reportsStorage.FindOcrResults(reportId, companyName)).Select(z => z.ToOcrResult());

            return ocrResults;
        }

        private string ContainerName(string tableName)
        {
            return $"{tableName.ToLower()}{ContainerSuffix}";
        }
    }
}
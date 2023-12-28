using OcrPlugin.App.Azure.Queue;
using OcrPlugin.App.Core.Reports;
using OcrPlugin.App.Spelling;
using System.Collections.Generic;

namespace OcrPlugin.App.Core.Models
{
    public class OcrResult
    {
        public int Id { get; set; }
        public string ReportId { get; set; }
        public string FileName { get; set; }
        public string TemplateName { get; set; }
        public ICollection<QueueFiles> QueueFiles { get; set; }
        public OcrDocumentError ErrorMessage { get; set; }
        public ICollection<CorrectedModel> CorrectedModels { get; set; }
        public IEnumerable<DebtorCase> Contracts { get; set; }
    }
}
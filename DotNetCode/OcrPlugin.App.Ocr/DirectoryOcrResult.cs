using OcrPlugin.App.Core.Models;
using OcrPlugin.App.Core.Reports;
using OcrPlugin.App.Spelling;

namespace OcrPlugin.App.Ocr
{
    public class DirectoryOcrResult
    {
        public string FileName { get; set; }
        public string TemplateName { get; set; }
        public ICollection<CorrectedModel> CorrectedModels { get; set; } = new List<CorrectedModel>();
        public IEnumerable<DebtorCase> Contracts { get; set; } = new List<DebtorCase>();

        public OcrDocumentError? OcrDocumentError { get; set; }
        public DirectoryOcrResult(string fileName, string templateName, OcrDocumentError? ocrDocumentError)
        {
            FileName = fileName;
            TemplateName = templateName;
            OcrDocumentError = ocrDocumentError;
        }
    }
}
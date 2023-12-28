using OcrPlugin.App.Core.Models;
using OcrPlugin.App.Ocr.Models;

namespace OcrPlugin.App.Ocr
{
    public interface IOcrFlow
    {
        public Task<DirectoryOcrResult> OcrWithFlow(Template template, OcrFile ocrFile, string fileName, string companyName);
    }
}
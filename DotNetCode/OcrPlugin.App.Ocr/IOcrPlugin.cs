using OcrPlugin.App.Core.Models;
using OcrPlugin.App.Ocr.Models;

namespace OcrPlugin.App.Ocr
{
    public interface IOcrPlugin
    {
        Task<DirectoryOcrResult> SingleDocument(string fileName, string templateName, OcrFile ocrFile, string companyName);
        Task<IDictionary<string, string>> OcrBeforeSave(IEnumerable<Property> properties, OcrFile ocrFile);
    }
}
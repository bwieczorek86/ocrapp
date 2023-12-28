using OcrPlugin.App.Spelling;

namespace OcrPlugin.App.Ocr.TextSanitizing
{
    internal interface ITextSanitizer
    {
        bool DoesApply(string type);
        IReadOnlyCollection<OcredModel> Sanitize(IReadOnlyCollection<OcredModel> generalType);
    }
}
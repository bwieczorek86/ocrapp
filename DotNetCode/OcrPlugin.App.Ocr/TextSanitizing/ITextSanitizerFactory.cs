namespace OcrPlugin.App.Ocr.TextSanitizing
{
    internal interface ITextSanitizerFactory
    {
        ITextSanitizer GetSanitizer(string typeName);
    }
}
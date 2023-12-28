namespace OcrPlugin.App.Ocr.TextSanitizing
{
    internal class TextSanitizerFactory : ITextSanitizerFactory
    {
        private readonly IEnumerable<ITextSanitizer> _textSanitizers;

        public TextSanitizerFactory(IEnumerable<ITextSanitizer> textSanitizers)
        {
            _textSanitizers = textSanitizers;
        }

        public ITextSanitizer GetSanitizer(string type)
        {
            return _textSanitizers.First(template => template.DoesApply(type));
        }
    }
}
namespace OcrPlugin.App.Ocr.Models
{
    public class OcrFile
    {
        private readonly string _contentType;
        public byte[] Content { get; set; }
        public string ContentType
        {
            get => _contentType;
            init => _contentType = value.ToUpperInvariant().Contains("PDF")
                ? OcrFileContentType.Pdf
                : OcrFileContentType.Image;
        }
    }
}
using System.Drawing;

namespace OcrPlugin.App.Ocr
{
    public interface IOcrEngine
    {
        Task<string> ReadText(byte[] image, Rectangle contentArea, string contentType, bool replaceNewLines);
    }
}
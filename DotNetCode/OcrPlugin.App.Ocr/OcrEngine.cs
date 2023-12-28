using IronOcr;
using OcrPlugin.App.Ocr.Models;
using System.Drawing;

namespace OcrPlugin.App.Ocr
{
    internal sealed class OcrEngine : IOcrEngine
    {
        public async Task<string> ReadText(byte[] image, Rectangle contentArea, string contentType, bool replaceNewLines)
        {
            using var input = new OcrInput();

            if (contentType == OcrFileContentType.Pdf)
            {
                const int firstPageIndex = 0;
                input.AddPdfPages(image, ContentArea: contentArea, Pages: new[] { firstPageIndex });
            }
            else
            {
                input.AddImage(image, contentArea);
            }

            try
            {
                var result = await GetOcr().ReadAsync(input);
                if (replaceNewLines)
                {
                    return result.Text.Replace("\n", " ").Replace("\r", string.Empty);
                }

                return result.Text.Replace("\r", string.Empty);
            }
            catch (Exception ex)
            {
                // dodać loggowanie błędów i wyrzucenie z własnego exceptiona
                throw new OcrEngineException("Some shit fucked up ex:", ex);
            }
        }

        private static IronTesseract GetOcr()
        {
            var ironTesseract = new IronTesseract
            {
                Language = OcrLanguage.PolishBest,
            };

            return ironTesseract;
        }
    }
}
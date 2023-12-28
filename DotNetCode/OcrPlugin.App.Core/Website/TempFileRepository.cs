using System.IO;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.Website
{
    internal sealed class TempFileRepository : ITempFileRepository
    {
        private const string DestinationDirectory = @"files\temp";
        private string _wwwDestinationDirectory = $"wwwroot\\{DestinationDirectory}";

        public async Task SaveImageToTempFolder(string templateName, byte[] convertedImage)
        {
            var destinationPath = Path.Combine(_wwwDestinationDirectory, templateName);
            var destinationFilePath = Path.Combine(_wwwDestinationDirectory, templateName, "image-for-template.jpg");
            if (!File.Exists(destinationFilePath))
            {
                Directory.CreateDirectory(destinationPath);
                await File.WriteAllBytesAsync(destinationFilePath, convertedImage);
            }
        }

        public FileInfo GetFile(string templateName)
        {
            var destinationPath = Path.Combine(_wwwDestinationDirectory, templateName, "image-for-template.jpg");
            return File.Exists(destinationPath)
                ? new FileInfo(destinationPath)
                : default;
        }

        public string GetRelativePath(string templateName)
        {
            return Path.Combine(DestinationDirectory, templateName, "image-for-template.jpg");
        }
    }
}
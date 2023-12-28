using System.IO;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.Website
{
    internal interface ITempFileRepository
    {
        Task SaveImageToTempFolder(string templateName, byte[] convertedImage);
        FileInfo GetFile(string templateName);
        string GetRelativePath(string templateName);
    }
}

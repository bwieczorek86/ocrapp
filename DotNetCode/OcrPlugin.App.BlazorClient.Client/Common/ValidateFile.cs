using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System.IO.Pipelines;
using System.Linq;

namespace OcrPlugin.App.BlazorClient.Client.Common;

public static class ValidateFile
{
    public static readonly List<string> ImageExtensions = new List<string> { "image/jpg", "image/jpeg", "image/jpe", "image/bmp", "image/gif", "image/png", "image/TIFF", "application/pdf" };
    private static long maxFileSize = 1024000;

    public static bool IsFileValid(IBrowserFile file)
    {
        try
        {
            using var content = new MultipartFormDataContent();
            _ = new StreamContent(file.OpenReadStream(maxFileSize));
        }
        catch
        {
            return false;
        }

        return ImageExtensions.Contains(file.ContentType);
    }
}
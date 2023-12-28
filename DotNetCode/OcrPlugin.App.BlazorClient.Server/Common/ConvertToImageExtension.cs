using OcrPlugin.App.BlazorClient.Server.Components.Templates;
using System.Drawing;

namespace OcrPlugin.App.BlazorClient.Server.Common;

public static class ConvertToImageExtension
{
    public static Stream ConvertPdfToImage(this IFormFile myFile)
    {
        using var stream = myFile.OpenReadStream();

        var buff = Freeware.Pdf2Png.Convert(stream, 1, 300);
        var ms = new MemoryStream(buff);
        var img = Image.FromStream(ms);

        // img.Save(imageFilePath, System.Drawing.Imaging.ImageFormat.Bmp);
        return img.ToStream();
    }
}
using System.Drawing;
using System.Drawing.Imaging;

namespace OcrPlugin.App.BlazorClient.Server.Components.Templates
{
    public static class ImageExtension
    {
        public static Stream ToStream(this Image image)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;
            return stream;
        }
    }
}
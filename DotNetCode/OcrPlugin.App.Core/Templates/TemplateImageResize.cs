using OcrPlugin.App.Core.Models;
using System.Drawing;
using System.IO;

namespace OcrPlugin.App.Core.Templates
{
    public class TemplateImageResize : ITemplateImageResize
    {
        public void ImageResize(Template imageToOcr, byte[] bytes)
        {
            var imageToOcrSize = GetImageSize(bytes);
            var imageSizeFactor = new ImageFactor
            {
                HeightFactor = imageToOcr.TemplateImageSize.Height / (double)imageToOcrSize.Height,
                WidthFactor = imageToOcr.TemplateImageSize.Width / (double)imageToOcrSize.Width
            };

            foreach (var property in imageToOcr.Properties)
            {
                property.CordsStartX = (int)(property.CordsStartX / imageSizeFactor.WidthFactor);
                property.CordsEndX = (int)(property.CordsEndX / imageSizeFactor.WidthFactor);
                property.CordsStartY = (int)(property.CordsStartY / imageSizeFactor.HeightFactor);
                property.CordsEndY = (int)(property.CordsEndY / imageSizeFactor.HeightFactor);
            }
        }

        private TemplateImageSize GetImageSize(byte[] bytes)
        {
            var templateImageSize = new TemplateImageSize();
            using (var ms = new MemoryStream(bytes))
            {
                var img = Image.FromStream(ms);

                templateImageSize.Height = img.Height;
                templateImageSize.Width = img.Width;
            }

            return templateImageSize;
        }

        public class ImageFactor
        {
            public double WidthFactor { get; set; }
            public double HeightFactor { get; set; }
        }
    }
}
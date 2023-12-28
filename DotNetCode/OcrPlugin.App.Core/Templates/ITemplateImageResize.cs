using OcrPlugin.App.Core.Models;

namespace OcrPlugin.App.Core.Templates
{
    public interface ITemplateImageResize
    {
        void ImageResize(Template imageToOcr, byte[] fileContent);
    }
}

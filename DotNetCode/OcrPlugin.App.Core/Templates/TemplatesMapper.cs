using OcrPlugin.App.Azure.Storage.Templates;
using OcrPlugin.App.Core.Models;

namespace OcrPlugin.App.Core.Templates
{
    public static class TemplatesMapper
    {
        public static Property ToProperty(this PropertyEntity entity)
        {
            return new()
            {
                Name = entity.Name,
                CordsEndX = entity.CordsEndX,
                CordsEndY = entity.CordsEndY,
                CordsStartX = entity.CordsStartX,
                CordsStartY = entity.CordsStartY
            };
        }

        public static TemplateSettings ToTemplateSettings(this TemplateSettingsEntity entity)
        {
            return new()
            {
                HasPublicId = entity.HasPublicId
            };
        }

        public static TitleTemplateMappings ToTitleTemplateMappings(this TitleTemplateMappingsEntity titleTemplateMappings)
        {
            return new()
            {
                Title = titleTemplateMappings.Title
            };
        }

        public static TemplateImageSize ToTemplateImageSize(this TemplateImageSizeEntity templateImageSizeEntity)
        {
            return new()
            {
                Width = templateImageSizeEntity.Width,
                Height = templateImageSizeEntity.Height
            };
        }
    }
}
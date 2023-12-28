using OcrPlugin.App.Azure.Storage.Templates;
using OcrPlugin.App.Azure.Storage.Templates.MergeObjects;
using OcrPlugin.App.Core.Models;
using System.Linq;

namespace OcrPlugin.App.Core.Templates
{
    public static class TemplatesEntityMapper
    {
        public static Template ToTemplate(this TemplateEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new()
            {
                Name = entity.Name,
                Properties = entity.Properties.Select(TemplatesMapper.ToProperty).ToList(),
                TemplateImageSize = entity.TemplateImageSize.ToTemplateImageSize(),
                Settings = entity.TemplateSettings.ToTemplateSettings(),
                TitleTemplateMappings = entity.TitleTemplateMappings.Select(TemplatesMapper.ToTitleTemplateMappings).ToList(),
                Rank = entity.Rank,
                FileName = entity.FileName,
                Type = entity.Type,
                IsActive = entity.IsActive,
            };
        }

        public static UpdateTemplateObject ToTemplateUpdateEntity(this Template template)
        {
            return new(
                template.Name,
                template.Properties.Select(ToPropertyEntity).ToList(),
                template.TemplateImageSize.ToTemplateImageSizeEntity(),
                template.Settings.ToTemplateSettingsEntity(),
                template.Rank,
                template.Type,
                template.IsActive);
        }

        public static TemplateEntity ToTemplateEntity(this Template template)
        {
            return new(template.Name)
            {
                TemplateImageSize = template.TemplateImageSize.ToTemplateImageSizeEntity(),
                TitleTemplateMappings = template.TitleTemplateMappings.Select(ToTitleTemplateMappingsEntity).ToList(),
                Properties = template.Properties.Select(ToPropertyEntity).ToList(),
                TemplateSettings = template.Settings.ToTemplateSettingsEntity(),
                Type = template.Type,
                FileName = template.FileName,
                Rank = template.Rank,
                IsActive = template.IsActive,
            };
        }

        public static PropertyEntity ToPropertyEntity(this Property property)
        {
            return new()
            {
                Name = property.Name,
                CordsEndX = property.CordsEndX,
                CordsEndY = property.CordsEndY,
                CordsStartX = property.CordsStartX,
                CordsStartY = property.CordsStartY
            };
        }

        public static TemplateSettingsEntity ToTemplateSettingsEntity(this TemplateSettings templateSettings)
        {
            return new()
            {
                HasPublicId = templateSettings.HasPublicId
            };
        }

        public static TemplateImageSizeEntity ToTemplateImageSizeEntity(this TemplateImageSize templateImageSize)
        {
            return new()
            {
                Width = templateImageSize.Width,
                Height = templateImageSize.Height
            };
        }

        public static TitleTemplateMappingsEntity ToTitleTemplateMappingsEntity(this TitleTemplateMappings titleTemplateMappings)
        {
            return new()
            {
                Title = titleTemplateMappings.Title
            };
        }

        public static UpdateTemplateRankObject ToTemplateRankUpdateEntity(this Template template)
        {
            return new(
                template.Name,
                template.Rank);
        }
    }
}
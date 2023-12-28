using OcrPlugin.App.Azure.Common.Constants;
using System.Collections.Generic;

namespace OcrPlugin.App.Azure.Storage.Templates.MergeObjects
{
    public class UpdateTemplateObject : CustomTableEntity
    {
        public IEnumerable<PropertyEntity> Properties { get; set; }
        public TemplateImageSizeEntity TemplateImageSize { get; set; }
        public TemplateSettingsEntity TemplateSettings { get; set; }
        public string Type { get; set; }
        public int Rank { get; set; }
        public bool IsActive { get; set; }

        public UpdateTemplateObject(
            string templateName,
            IEnumerable<PropertyEntity> properties,
            TemplateImageSizeEntity templateImageSize,
            TemplateSettingsEntity templateSettings,
            int rank,
            string type,
            bool isActive)
        {
            PartitionKey = PartitionKeys.TemplateEntity;
            RowKey = templateName;

            TemplateImageSize = templateImageSize;
            Properties = properties;
            TemplateSettings = templateSettings;
            Type = type;
            Rank = rank;
            IsActive = isActive;
            ETag = "*";
        }
    }
}
using Microsoft.Azure.Cosmos.Table;
using OcrPlugin.App.Azure.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OcrPlugin.App.Azure.Storage.Templates
{
    public class TemplateEntity : CustomTableEntity
    {
        public bool IsActive { get; set; }
        public string Type { get; set; } = default!;
        public int Rank { get; set; }
        public string FileName { get; set; }
        public TemplateImageSizeEntity TemplateImageSize { get; set; }
        public TemplateSettingsEntity TemplateSettings { get; set; }
        public ICollection<PropertyEntity> Properties { get; set; } = new List<PropertyEntity>();
        public ICollection<TitleTemplateMappingsEntity> TitleTemplateMappings { get; set; } = new List<TitleTemplateMappingsEntity>();

        [IgnoreProperty]
        public string Name => RowKey;
        [IgnoreProperty]
        public IEnumerable<PropertyEntity> FilledProperties => Properties.Where(p => p.CordsStartX != 0 && p.CordsStartY != 0 && p.CordsEndX != 0 && p.CordsEndY != 0).ToList();

        [Obsolete("Do not use, for internal framework usage.", error: true)]
        public TemplateEntity()
        {
        }

        public TemplateEntity(string name)
        {
            PartitionKey = PartitionKeys.TemplateEntity;
            RowKey = name;
        }
    }
}
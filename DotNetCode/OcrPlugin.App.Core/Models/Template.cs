using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OcrPlugin.App.Core.Models
{
    public class Template
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; }
        public int Rank { get; set; }
        public string Type { get; set; } = default!;
        public string FileName { get; set; }
        public TemplateImageSize TemplateImageSize { get; set; }
        public TemplateSettings Settings { get; set; }
        public ICollection<Property> Properties { get; set; } = new List<Property>();
        public ICollection<TitleTemplateMappings> TitleTemplateMappings { get; set; } = new List<TitleTemplateMappings>();

        [NotMapped]
        public IEnumerable<Property> FilledProperties => Properties.Where(p => p.CordsStartX != 0 && p.CordsStartY != 0 && p.CordsEndX != 0 && p.CordsEndY != 0).ToList();
    }
}
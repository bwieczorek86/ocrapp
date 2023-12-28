using System.ComponentModel.DataAnnotations.Schema;

namespace OcrPlugin.App.BlazorClient.Client.DTOs
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
        public IEnumerable<Property> FilledProperties => Properties.Where(p => p.IsNotEmpty()).ToList();
    }
}
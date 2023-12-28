using OcrPlugin.App.BlazorClient.Client.DTOs;

namespace OcrPlugin.App.BlazorClient.Client.modules.TemplateCreate.Cs
{
    public class CreateTemplateModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public TemplateSettings Settings { get; set; } = new();
        public ICollection<TitleTemplateMappingsDto> TitleTemplateMappings { get; set; } = new List<TitleTemplateMappingsDto>();
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
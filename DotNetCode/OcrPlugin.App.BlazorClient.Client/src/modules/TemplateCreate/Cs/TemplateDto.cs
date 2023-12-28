using OcrPlugin.App.BlazorClient.Client.DTOs;

namespace OcrPlugin.App.BlazorClient.Client.modules.TemplateCreate.Cs;

public class TemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }
    public int Rank { get; set; }
    public string Type { get; set; } = default!;
    public string FileName { get; set; }
    public TemplateSettings Settings { get; set; }
    public ICollection<Property> Properties { get; set; } = new List<Property>();
    public ICollection<TitleTemplateMappings> TitleTemplateMappings { get; set; } = new List<TitleTemplateMappings>();
    public string FileBase64 { get; set; }
}
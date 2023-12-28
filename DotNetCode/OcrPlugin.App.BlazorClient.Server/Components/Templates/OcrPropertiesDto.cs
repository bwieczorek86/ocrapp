namespace OcrPlugin.App.BlazorClient.Server.Components.Templates;

public record OcrPropertiesDto(string TemplateName, string CompanyName, ICollection<OcrPropertyDto> OcrProperties);

public record OcrPropertyDto(string Name, int CordsStartX, int CordsStartY, int CordsEndX, int CordsEndY);
namespace OcrPlugin.App.BlazorClient.Shared.Templates;

public record ValidatePropertiesDto(string TemplateName, ICollection<PropertyDto> Properties);
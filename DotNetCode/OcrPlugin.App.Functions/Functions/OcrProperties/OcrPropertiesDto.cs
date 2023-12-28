using System.Collections.Generic;

namespace OcrPlugin.App.Functions.Functions.OcrProperties;

public record OcrPropertiesDto(string TemplateName, string CompanyName, ICollection<OcrPropertyDto> OcrProperties);

public record OcrPropertyDto(string Name, int CordsStartX, int CordsStartY, int CordsEndX, int CordsEndY);
namespace OcrPlugin.App.BlazorClient.Shared.Reports;

public record ReportListDto(int Iterator, string Id, string? TemplateName, string UserName, DateTime DateTime, int NumberOfFiles);
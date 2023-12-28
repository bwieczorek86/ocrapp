namespace OcrPlugin.App.BlazorClient.Shared.Reports;

public record SingleReportDto(string Id, string TemplateName, IList<SingleReportFile> ReportFiles, DateTime DateTime, string UserName);
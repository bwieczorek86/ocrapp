using OcrPlugin.App.BlazorClient.Client.DTOs;
using OcrPlugin.App.Core.Reports;

namespace OcrPlugin.App.BlazorClient.Server.Components.Reports.ReportDTO;

public class OcrResutltUpdateDto
{
    public string ReportId { get; set; }
    public string FileName { get; set; }
    public string TemplateName { get; set; }
    public OcrDocumentError ErrorMessage { get; set; }
    public IEnumerable<DebtorCase> Contracts { get; set; }
    public ICollection<QueueFiles> QueueFiles { get; set; }
}
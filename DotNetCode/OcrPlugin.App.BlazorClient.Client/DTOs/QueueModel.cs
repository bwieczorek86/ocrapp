namespace OcrPlugin.App.BlazorClient.Client.DTOs;

public class QueueModel
{
    public string FileName { get; set; }
    public List<QueueFiles> QueueFiles { get; set; }
    public string TemplateName { get; set; }
    public string BlobContainer { get; set; }
    public string ReportId { get; set; }
    public string Error { get; set; }
}
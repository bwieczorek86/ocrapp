namespace OcrPlugin.App.BlazorClient.Server.Components.Reports;

public partial class ReportsController
{
    public class ReportCreateDto
    {
        public string ReportId { get; set; }
        public string TemplateName { get; set; }
        public IEnumerable<BlobFileName> FileNames { get; set; }
    }

    public class BlobFileName
    {
        public string FileName { get; set; }
        public string FileNameOnDisc { get; set; }
    }
}
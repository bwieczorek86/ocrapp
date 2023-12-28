using Microsoft.AspNetCore.Components.Forms;
using OcrPlugin.App.Common;

namespace OcrPlugin.App.BlazorClient.Client.modules.Ocr;

public class ProcessedFile
{
    public IBrowserFile BrowserFile { get; set; }
    public string ReportId { get; set; }
    public string FileId { get; set; }
    public string Status { get; set; }

    public bool IsStillProcessing()
    {
        return Status == Consts.FileProcessingStatus.Inited
            || Status == Consts.FileProcessingStatus.Processing;
    }
}
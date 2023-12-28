namespace OcrPlugin.App.Azure.Storage.Reports;

public class QueueFilesEntity
{
    public string BlobFileName { get; set; }
    public string FileExtension { get; set; }
    public bool IsOriginal { get; set; }
}
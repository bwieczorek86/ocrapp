namespace OcrPlugin.App.Core.Reports;

public class FrontendErrorDisplay : OcrDocumentError
{
    public override string Message { get; set; } = "Invalid File or Configuration";
}
namespace OcrPlugin.App.BlazorClient.Shared.Reports;

public record OcrResult(
    int Id,
    string ReportId,
    string UserFileName,
    string BlobFileName,
    string TemplateName,
    OcrDocumentErrorDto ErrorMessage,
    ICollection<CorrectedModelDto> CorrectedModels,
    ICollection<DebtorCase> Contracts);
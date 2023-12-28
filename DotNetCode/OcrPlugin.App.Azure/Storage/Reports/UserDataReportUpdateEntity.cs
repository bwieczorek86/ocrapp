using OcrPlugin.App.Azure.Storage.Debtors.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OcrPlugin.App.Azure.Storage.Reports;

public class UserDataReportUpdateEntity : CustomTableEntity
{
    public OcrDocumentErrorEntity ErrorMessage { get; set; }
    public ICollection<ReportCorrectedModelEntity> CorrectedModels { get; set; }
    public IEnumerable<DebtorCaseEntity> Contracts { get; set; }
    public IEnumerable<QueueFilesEntity> QueueFiles { get; set; }

    public UserDataReportUpdateEntity(
        string reportId,
        OcrDocumentErrorEntity errorMessage,
        ICollection<ReportCorrectedModelEntity> correctedModels,
        ICollection<DebtorCaseEntity> contracts,
        ICollection<QueueFilesEntity> queueFiles)
    {
        PartitionKey = reportId;
        RowKey = GetJpgBlob(queueFiles);
        ErrorMessage = errorMessage;
        CorrectedModels = correctedModels;
        Contracts = contracts;
        QueueFiles = queueFiles;
    }

    private string GetJpgBlob(IEnumerable<QueueFilesEntity> queueFiles)
    {
        var jpgBlob = queueFiles
            .Where(x => !x.FileExtension.Contains("pdf"))
            .Select(x => x.BlobFileName).FirstOrDefault();

        var jgpBlobWithoutExt = jpgBlob!.Replace(Path.GetExtension(jpgBlob), string.Empty);

        return jgpBlobWithoutExt;
    }
}
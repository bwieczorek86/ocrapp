using OcrPlugin.App.Azure.Storage.Debtors.Entities;
using System;
using System.Collections.Generic;

namespace OcrPlugin.App.Azure.Storage.Reports
{
    public class ReportEntity : CustomTableEntity
    {
        public string ReportId { get; set; }
        public string FileName { get; set; }
        public string TemplateName { get; set; }
        public List<QueueFilesEntity> QueueFiles { get; set; }
        public OcrDocumentErrorEntity ErrorMessage { get; set; }
        public ICollection<ReportCorrectedModelEntity> CorrectedModels { get; set; }
        public IEnumerable<DebtorCaseEntity> Contracts { get; set; }

        [Obsolete("Do not use, for internal framework usage.", error: true)]
        public ReportEntity()
        {
        }

        public ReportEntity(string reportId, string blobFileName)
        {
            PartitionKey = reportId;
            RowKey = blobFileName;
        }
    }
}
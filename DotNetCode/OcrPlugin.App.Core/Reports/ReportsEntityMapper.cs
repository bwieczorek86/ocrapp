using Microsoft.Extensions.Azure;
using OcrPlugin.App.Azure.Queue;
using OcrPlugin.App.Azure.Storage.Debtors.Entities;
using OcrPlugin.App.Azure.Storage.Reports;
using OcrPlugin.App.Core.Models;
using OcrPlugin.App.Spelling;
using System.IO;
using System.Linq;

namespace OcrPlugin.App.Core.Reports
{
    public static class ReportsEntityMapper
    {
        public static ReportEntity ToReportEntity(this OcrResult entity)
        {
            return new(entity.ReportId, GetJpgBlob(entity))
            {
                ReportId = entity.ReportId,
                FileName = entity.FileName,
                TemplateName = entity.TemplateName,
                QueueFiles = entity.QueueFiles.Select(ToQueueFilesEntity).ToList(),
                ErrorMessage = entity.ErrorMessage.ToOcrDocumentErrorEntity(),
                Contracts = entity.Contracts.Select(ToContractEntity).ToList(),
                CorrectedModels = entity.CorrectedModels.Select(ToCorrectedModelsEntity).ToList(),
            };
        }

        private static string GetJpgBlob(OcrResult entity)
        {
            var jpgBlob = entity.QueueFiles
                .Where(x => !x.FileExtension.Contains("pdf"))
                .Select(x => x.BlobFileName).FirstOrDefault();

            return jpgBlob;
        }

        public static UserDataReportUpdateEntity ToUserDataReportUpdateEntity(this OcrResult entity)
        {
            return new(
                entity.ReportId,
                entity.ErrorMessage.ToOcrDocumentErrorEntity(),
                entity.CorrectedModels.Select(ToCorrectedModelsEntity).ToList(),
                entity.Contracts.Select(ToContractEntity).ToList(),
                entity.QueueFiles.Select(ToQueueFilesEntity).ToList());
        }

        public static OcrDocumentErrorEntity ToOcrDocumentErrorEntity(this OcrDocumentError documentError)
        {
            var entity = new OcrDocumentErrorEntity();

            if (documentError != null)
            {
                entity.Type = documentError.Type;
                entity.Message = documentError.Message;
                return entity;
            }

            return entity;
        }

        public static DebtorCaseEntity ToContractEntity(DebtorCase entity)
        {
            return new DebtorCaseEntity(entity.ContractId)
            {
                ContractId = entity.ContractId,
                DebtorEntities = entity.Debtors.Select(ToDebtorInCaseEntity).ToList(),
            };
        }

        public static DebtorEntityObject ToDebtorInCaseEntity(DebtorInCase entity)
        {
            return new DebtorEntityObject()
            {
                Email = entity.Email,
                Nip = entity.Nip,
                Pesel = entity.Pesel,
                Regon = entity.Regon,
                DebtorName = entity.DebtorName,
                PublicId = entity.PublicId,
                Addresses = entity.Addresses.Select(ToDebtorAddressEntity).ToList()
            };
        }

        private static DebtorAddressEntityObject ToDebtorAddressEntity(DebtorAddress entity)
        {
            return new DebtorAddressEntityObject()
            {
                City = entity.City,
                Street = entity.Street,
                PostalCode = entity.PostalCode
            };
        }

        private static ReportCorrectedModelEntity ToCorrectedModelsEntity(CorrectedModel entity)
        {
            return new ReportCorrectedModelEntity()
            {
                Text = entity.Text,
                CorrectedText = entity.CorrectedText,
                PropertyName = entity.PropertyName,
            };
        }

        private static QueueFilesEntity ToQueueFilesEntity(QueueFiles entity)
        {
            return new QueueFilesEntity()
            {
                BlobFileName = entity.BlobFileName,
                FileExtension = entity.FileExtension,
                IsOriginal = entity.IsOriginal
            };
        }
    }
}
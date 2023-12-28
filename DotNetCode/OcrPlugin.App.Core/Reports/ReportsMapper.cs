using OcrPlugin.App.Azure.Queue;
using OcrPlugin.App.Azure.Storage.Debtors.Entities;
using OcrPlugin.App.Azure.Storage.Reports;
using OcrPlugin.App.Core.Models;
using OcrPlugin.App.Spelling;
using System.Linq;

namespace OcrPlugin.App.Core.Reports
{
    public static class ReportsMapper
    {
        public static DebtorCase ToContract(DebtorCaseEntity entity)
        {
            return new DebtorCase()
            {
                ContractId = entity.ContractId,
                Debtors = entity.DebtorEntities.Select(ToDebtorInCase).ToList()
            };
        }

        public static OcrResult ToOcrResult(this ReportEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var ocrResult = new OcrResult
            {
                ReportId = entity.ReportId,
                FileName = entity.FileName,
                TemplateName = entity.TemplateName,
                QueueFiles = entity.QueueFiles.Select(ToQueueFile).ToList(),
                Contracts = entity.Contracts.Select(ToContract).ToList(),
                CorrectedModels = entity.CorrectedModels.Select(ToCorrectedModels).ToList(),
            };

            if (entity.ErrorMessage != null)
            {
                ocrResult.ErrorMessage = new FrontendErrorDisplay
                {
                    Message = entity.ErrorMessage.Message,
                    Type = entity.ErrorMessage.Type
                };
            }

            return ocrResult;
        }

        public static QueueFiles ToQueueFile(QueueFilesEntity entity)
        {
            return new QueueFiles()
            {
                BlobFileName = entity.BlobFileName,
                FileExtension = entity.FileExtension,
                IsOriginal = entity.IsOriginal
            };
        }

        public static DebtorInCase ToDebtorInCase(DebtorEntityObject entity)
        {
            return new DebtorInCase()
            {
                Email = entity.Email,
                Nip = entity.Nip,
                Pesel = entity.Pesel,
                Regon = entity.Regon,
                DebtorName = entity.DebtorName,
                PublicId = entity.PublicId,
                Addresses = entity.Addresses.Select(ToDebtorAddress).ToList()
            };
        }

        private static DebtorAddress ToDebtorAddress(DebtorAddressEntityObject entity)
        {
            return new DebtorAddress
            {
                City = entity.City,
                Street = entity.Street,
                PostalCode = entity.PostalCode
            };
        }

        private static CorrectedModel ToCorrectedModels(ReportCorrectedModelEntity entity)
        {
            return new CorrectedModel
            {
                Text = entity.Text,
                CorrectedText = entity.CorrectedText,
                PropertyName = entity.PropertyName,
            };
        }
    }
}
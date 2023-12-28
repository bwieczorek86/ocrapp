using Microsoft.Extensions.Logging;
using OcrPlugin.App.Azure.Common.CloudTableStorage;
using OcrPlugin.App.Azure.Common.Constants;
using OcrPlugin.App.Azure.Storage.Debtors.Entities;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.Debtors
{
    public class DebtorStorage : StorageBase, IDebtorStorage
    {
        public DebtorStorage(
            ILoggerFactory loggerFactory,
            ICloudTableClientFactoryResolver cloudTableClientFactoryResolver)
            : base(loggerFactory, cloudTableClientFactoryResolver.Resolve(StorageAccountType.Production))
        {
        }

        public async Task<DebtorCaseEntity> FindDebtorCase(string contractId, string companyName)
        {
            var sanitaizeContractId = contractId.Replace("/", "_").Replace(":", " ").Replace(".", string.Empty).Trim();
            return await RetrieveEntity<DebtorCaseEntity>(PartitionKeys.DebtorEntity, $"{RowKeyPrefixes.Debtor}{sanitaizeContractId}", GetTableName(companyName));
        }

        public async Task<DebtorIdentifierEntity> FindDebtorIdentifier(string debtorId, string companyName)
        {
            return await RetrieveEntity<DebtorIdentifierEntity>(PartitionKeys.DebtorEntity, $"{RowKeyPrefixes.DebtorIdentifier}{debtorId}", GetTableName(companyName));
        }

        public async Task<DebtorCaseEntity> FindDebtorPersonalData(string debtorPd, string companyName)
        {
            return await RetrieveEntity<DebtorCaseEntity>(PartitionKeys.DebtorEntity, $"{RowKeyPrefixes.DebtorPersonalData}{debtorPd}", GetTableName(companyName));
        }

        public async Task UpsertDebtorCase(DebtorCaseEntity debtorCaseEntity, string companyName)
        {
            await Upsert(debtorCaseEntity, GetTableName(companyName));
        }

        public async Task UpsertDebtorIdentifier(DebtorIdentifierEntity debtorIdentifierEntity, string companyName)
        {
            await Upsert(debtorIdentifierEntity, GetTableName(companyName));
        }

        public async Task UpsertDebtorIdentifiers(IEnumerable<DebtorIdentifierEntity> debtorIdentifierEntities, string companyName)
        {
            await Upsert(debtorIdentifierEntities, GetTableName(companyName));
        }

        public async Task UpsertDebtorPersonalData(DebtorPersonalDataEntity debtorPersonalDataEntity, string companyName)
        {
            await Upsert(debtorPersonalDataEntity, GetTableName(companyName));
        }

        public async Task UpsertDebtorPersonalData(IEnumerable<DebtorPersonalDataEntity> debtorPersonalDataEntities, string companyName)
        {
            await Upsert(debtorPersonalDataEntities, GetTableName(companyName));
        }

        private string GetTableName(string companyName) => $"{ToTitleCase(companyName)}Debtors";

        private string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
    }
}
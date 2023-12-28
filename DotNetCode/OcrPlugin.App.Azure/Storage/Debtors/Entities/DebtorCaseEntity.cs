using OcrPlugin.App.Azure.Common.Constants;
using System;
using System.Collections.Generic;

namespace OcrPlugin.App.Azure.Storage.Debtors.Entities
{
    public class DebtorCaseEntity : CustomTableEntity
    {
        public ICollection<DebtorEntityObject> DebtorEntities { get; set; }
        public string ContractId { get; set; }

        [Obsolete("Do not use, for internal framework usage.", error: true)]
        public DebtorCaseEntity()
        {
        }

        public DebtorCaseEntity(string contractId)
        {
            if (string.IsNullOrWhiteSpace(contractId))
            {
                throw new ArgumentNullException(contractId);
            }

            ContractId = contractId;
            PartitionKey = PartitionKeys.DebtorEntity;
            RowKey = $"{RowKeyPrefixes.Debtor}{contractId}";
        }
    }
}
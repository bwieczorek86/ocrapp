using Microsoft.Azure.Cosmos.Table;
using OcrPlugin.App.Azure.Common.Constants;
using System;
using System.Collections.Generic;

namespace OcrPlugin.App.Azure.Storage.Debtors.Entities
{
    public class DebtorPersonalDataEntity : CustomTableEntity
    {
        public string DebtorName { get; set; }
        public string Nip { get; set; }
        public string Pesel { get; set; }
        public string Regon { get; set; }
        public string Email { get; set; }
        public string PublicId { get; set; }

        public ICollection<DebtorAddressEntityObject> Addresses { get; set; }
        public ICollection<string> Cases { get; set; }

        [IgnoreProperty]
        public string PersonalData => RowKey;

        [Obsolete("Do not use, for internal framework usage.", error: true)]
        public DebtorPersonalDataEntity()
        {
        }

        public DebtorPersonalDataEntity(string personalData)
        {
            PartitionKey = PartitionKeys.TemplateEntity;
            RowKey = $"{RowKeyPrefixes.DebtorPersonalData}{personalData}";
        }
    }
}
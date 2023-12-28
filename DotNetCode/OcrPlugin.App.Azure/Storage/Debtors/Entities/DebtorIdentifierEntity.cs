using OcrPlugin.App.Azure.Common.Constants;
using System;
using System.Collections.Generic;

namespace OcrPlugin.App.Azure.Storage.Debtors.Entities
{
    public class DebtorIdentifierEntity : CustomTableEntity
    {
        public string DebtorName { get; set; }
        public string Nip { get; set; }
        public string Pesel { get; set; }
        public string Regon { get; set; }
        public string Email { get; set; }
        public string PublicId { get; set; }
        public string PublicIdType { get; set; }

        public HashSet<string> Cases { get; set; } = new();
        public ICollection<DebtorAddressEntityObject> Addresses { get; set; } = new List<DebtorAddressEntityObject>();

        [Obsolete("Do not use, for internal framework usage.", error: true)]
        public DebtorIdentifierEntity()
        {
        }

        public DebtorIdentifierEntity(string publicId, string publicIdType)
        {
            PartitionKey = PartitionKeys.DebtorEntity;
            RowKey = $"{RowKeyPrefixes.DebtorIdentifier}{publicId}";
            PublicId = publicId;
            PublicIdType = publicIdType;
        }
    }
}
using OcrPlugin.App.Azure.Common.Constants;

namespace OcrPlugin.App.Azure.Storage.Debtors.MergeObjects
{
    public class UpdateDebtorObject : BaseDebtorEntity
    {
        public UpdateDebtorObject(
            string contractId,
            string debtorName,
            string nip,
            string pesel,
            string regon,
            string publicId,
            string street,
            string city,
            string postalCode,
            string email)
        {
            PartitionKey = PartitionKeys.DebtorEntity;
            RowKey = $"{RowKeyPrefixes.Debtor}_{contractId}_{publicId}";

            ContractId = contractId;
            DebtorName = debtorName;
            Nip = nip;
            Pesel = pesel;
            Regon = regon;
            Street = street;
            City = city;
            PostalCode = postalCode;
            PublicId = publicId;
            Email = email;
            ETag = "*";
        }
    }
}
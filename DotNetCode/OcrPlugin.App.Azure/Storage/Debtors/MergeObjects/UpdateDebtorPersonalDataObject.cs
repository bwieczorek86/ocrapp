using OcrPlugin.App.Azure.Common.Constants;
using System;

namespace OcrPlugin.App.Azure.Storage.Debtors.MergeObjects
{
    public class UpdateDebtorPersonalDataObject : BaseDebtorEntity
    {
        public UpdateDebtorPersonalDataObject(
            string contractId,
            string debtorName,
            string nip,
            string pesel,
            string regon,
            string street,
            string city,
            string postalCode,
            string email)
        {
            var personalData = $"{city}_{postalCode}_{street}_{debtorName}_{Guid.NewGuid()}";
            PartitionKey = PartitionKeys.DebtorEntity;
            RowKey = RowKeyPrefixes.DebtorPersonalData + personalData;

            ContractId = contractId;
            DebtorName = debtorName;
            Nip = nip;
            Pesel = pesel;
            Regon = regon;
            Street = street;
            City = city;
            PostalCode = postalCode;
            Email = email;
            ETag = "*";
        }
    }
}
using OcrPlugin.App.Azure.Storage.Debtors.Entities;
using OcrPlugin.App.Core.Models;
using System.Linq;

namespace OcrPlugin.App.Core.Debtors
{
    public static class DebtorsEntityMapper
    {
        public static DebtorCaseEntity ToDebtorCaseEntity(this DebtorCase debtorCase)
        {
            return new(debtorCase.ContractId)
            {
                DebtorEntities = debtorCase.Debtors.Select(ToDebtorEntityObject).ToList()
            };

            static DebtorEntityObject ToDebtorEntityObject(DebtorInCase debtor)
            {
                return new()
                {
                    DebtorName = debtor.DebtorName,
                    Nip = debtor.Nip,
                    Pesel = debtor.Pesel,
                    Regon = debtor.Regon,
                    Email = debtor.Email,
                    PublicId = debtor.PublicId,
                    Addresses = debtor.Addresses.Select(ToDebtorAddressEntity).ToList(),
                    Cases = debtor.Cases.Select(ToDebtorCasesEntity).ToList(),
                };
            }
        }

        private static DebtorAddressEntityObject ToDebtorAddressEntity(DebtorAddress entityObject)
        {
            return new()
            {
                City = entityObject.City,
                Street = entityObject.Street,
                PostalCode = entityObject.PostalCode,
            };
        }

        private static DebtorCasesEntityObject ToDebtorCasesEntity(Case entityObject)
        {
            return new()
            {
                ContractId = entityObject.ContractId,
            };
        }

        public static DebtorIdentifierEntity ToDebtorIdentifierEntity(this Debtor debtor)
        {
            return new(debtor.PublicId, debtor.PublicIdType)
            {
                DebtorName = debtor.DebtorName,
                Nip = debtor.Nip,
                Pesel = debtor.Pesel,
                Regon = debtor.Regon,
                Email = debtor.Email,
                PublicId = debtor.PublicId,
                Addresses = debtor.Addresses.Select(ToDebtorAddressEntity).ToList(),
                Cases = debtor.Cases.Select(c => c.ContractId).ToHashSet(),
            };
        }

        public static DebtorIdentifierEntity ToDebtorIdentifierEntity(this DebtorInCase debtor)
        {
            return new(debtor.PublicId, debtor.PublicIdType)
            {
                DebtorName = debtor.DebtorName,
                Nip = debtor.Nip,
                Pesel = debtor.Pesel,
                Regon = debtor.Regon,
                Email = debtor.Email,
                PublicId = debtor.PublicId,
                Addresses = debtor.Addresses.Select(ToDebtorAddressEntity).ToList(),
            };
        }

        // public static IEnumerable<DebtorPersonalDataEntity> ToDebtorPersonalDataEntity(this Debtor debtor)
        // {
        //     // TODO This will return a list of entites;
        //     return default;
        //
        //     // var personalData = $"{debtor.DebtorName}_{debtor.City}_{debtor.PostalCode}_{debtor.Street}";
        //     // return new(personalData)
        //     // {
        //     //     DebtorName = debtor.DebtorName,
        //     //     ContractId = debtor.ContractId,
        //     //     Nip = debtor.Nip,
        //     //     Pesel = debtor.Pesel,
        //     //     Regon = debtor.Regon,
        //     //     Street = debtor.Street,
        //     //     City = debtor.City,
        //     //     PostalCode = debtor.PostalCode,
        //     //     Email = debtor.Email
        //     // };
        // }
    }
}
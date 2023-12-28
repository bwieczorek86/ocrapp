using OcrPlugin.App.Azure.Storage.Debtors.Entities;
using OcrPlugin.App.Core.Models;
using System.Linq;

namespace OcrPlugin.App.Core.Debtors
{
    public static class DebtorsMapper
    {
        public static Debtor ToDebtor(this DebtorIdentifierEntity entity)
        {
            return new()
            {
                DebtorName = entity.DebtorName,
                Nip = entity.Nip,
                Pesel = entity.Pesel,
                Regon = entity.Regon,
                Email = entity.Email,
                Addresses = entity.Addresses.Select(ToDebtorAddressFromObject).ToList(),
                Cases = entity.Cases.Select(contractId => new Case { ContractId = contractId }).ToList(),
                PublicId = entity.PublicId
            };
        }

        public static DebtorCase ToDebtorCase(this DebtorCaseEntity entity)
        {
            return new()
            {
                ContractId = entity.ContractId,
                Debtors = entity.DebtorEntities.Select(ToDebtorFromObject).ToList()
            };

            DebtorInCase ToDebtorFromObject(DebtorEntityObject entityObject)
            {
                var debtor = new DebtorInCase
                {
                    DebtorName = entityObject.DebtorName,
                    Nip = entityObject.Nip,
                    Pesel = entityObject.Pesel,
                    Regon = entityObject.Regon,
                    Email = entityObject.Email,
                    Addresses = entityObject.Addresses?.Select(ToDebtorAddressFromObject).ToList(),
                    Cases = entityObject.Cases?.Select(ToDebtorCaseFromObject).ToList(),
                    PublicId = entityObject.PublicId,
                };

                return debtor;
            }
        }

        public static Debtor ToDebtor(this DebtorPersonalDataEntity entity)
        {
            return new()
            {
                DebtorName = entity.DebtorName,
                Nip = entity.Nip,
                Pesel = entity.Pesel,
                Regon = entity.Regon,
                Email = entity.Email,
                Addresses = entity.Addresses.Select(ToDebtorAddressFromObject).ToList()
            };
        }

        private static DebtorAddress ToDebtorAddressFromObject(DebtorAddressEntityObject entityObject)
        {
            return new()
            {
                City = entityObject.City,
                Street = entityObject.Street,
                PostalCode = entityObject.PostalCode
            };
        }

        private static Case ToDebtorCaseFromObject(DebtorCasesEntityObject entityObject)
        {
            return new()
            {
                ContractId = entityObject.ContractId,
            };
        }
    }
}
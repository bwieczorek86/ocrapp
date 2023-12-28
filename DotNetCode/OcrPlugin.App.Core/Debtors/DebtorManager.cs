using OcrPlugin.App.Azure.Storage.Debtors;
using OcrPlugin.App.Azure.Storage.Debtors.Entities;
using OcrPlugin.App.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.Debtors
{
    internal sealed class DebtorManager : IDebtorManager
    {
        private readonly IDebtorStorage _debtorStorage;

        public DebtorManager(
            IDebtorStorage debtorStorage)
        {
            _debtorStorage = debtorStorage;
        }

        public async Task<DebtorCase> GetByContractId(string contractId, string companyName)
        {
            return (await _debtorStorage.FindDebtorCase(contractId, companyName))?.ToDebtorCase();
        }

        public async Task<Debtor> GetByPublicId(string publicId, string companyName)
        {
            return (await _debtorStorage.FindDebtorIdentifier(publicId, companyName))?.ToDebtor();
        }

        public async Task<DebtorCase> GetDebtorCaseByPersonalData(string personalData, string companyName)
        {
            var debtorInRowKeyFormat = personalData.ToUpper().Replace(" ", "_");

            return (await _debtorStorage.FindDebtorPersonalData(debtorInRowKeyFormat, companyName))?.ToDebtorCase();
        }

        public async Task Upsert(DebtorCase debtorCase, string companyName)
        {
            await _debtorStorage.UpsertDebtorCase(debtorCase.ToDebtorCaseEntity(), companyName);

            foreach (var debtor in debtorCase.Debtors)
            {
                var debtorIdentifier = await _debtorStorage.FindDebtorIdentifier(debtor.PublicId, companyName);
                if (debtorIdentifier != null)
                {
                    debtorIdentifier.Cases.Add(debtorCase.ContractId);
                }
            }

            await _debtorStorage.UpsertDebtorCase(debtorCase.ToDebtorCaseEntity(), companyName);
        }

        public async Task Upsert(IEnumerable<DebtorIdentifierEntity> debtors, string companyName)
        {
            await _debtorStorage.UpsertDebtorIdentifiers(debtors, companyName);
        }
    }
}
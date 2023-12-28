using OcrPlugin.App.Common;
using OcrPlugin.App.Common.Enums;
using OcrPlugin.App.Core.Debtors;
using OcrPlugin.App.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.Matching
{
    public class MatchService : IMatchService
    {
        private readonly IDebtorManager _debtorManager;

        public MatchService(IDebtorManager debtorManager)
        {
            _debtorManager = debtorManager;
        }

        public async Task<IEnumerable<DebtorCase>> Match(MatchObject matchObject, bool hasPublicId, string companyName)
        {
            // TODO invalid ROWKEY chars should be handled
            var fixedContractId = matchObject.ContractId?
                .Replace("/", "_")
                .Replace(":", " ")
                .Replace(".", string.Empty)
                .Trim();

            if (fixedContractId != null)
            {
                var matchByContractId = await _debtorManager.GetByContractId(fixedContractId, companyName);
                if (matchByContractId != null)
                {
                    return new[] { matchByContractId };
                }
            }

            if (hasPublicId)
            {
                var matchDebtorByPublicId = await _debtorManager.GetByPublicId(matchObject.PublicId.ToString(), companyName);

                if (matchDebtorByPublicId != null)
                {
                    return await DebtorCases(matchDebtorByPublicId, companyName);
                }
            }

            if (!string.IsNullOrWhiteSpace(matchObject.Nip?.ToString()))
            {
                var matchDebtorByPublicId = await _debtorManager.GetByPublicId(matchObject.Nip.ToString(), companyName);

                if (matchDebtorByPublicId != null)
                {
                    return await DebtorCases(matchDebtorByPublicId, companyName);
                }
            }

            if (!string.IsNullOrWhiteSpace(matchObject.Pesel?.ToString()))
            {
                var matchDebtorByPublicId = await _debtorManager.GetByPublicId(matchObject.Pesel?.ToString(), companyName);

                if (matchDebtorByPublicId != null)
                {
                    return await DebtorCases(matchDebtorByPublicId, companyName);
                }
            }

            if (!string.IsNullOrWhiteSpace(matchObject.DebtorName))
            {
                var matchDebtorByPublicId = await _debtorManager.GetDebtorCaseByPersonalData(matchObject.DebtorName, companyName);

                if (matchDebtorByPublicId != null)
                {
                    return await MatchDebtorCaseByPersonalData(matchDebtorByPublicId, matchObject, companyName);
                }
            }

            return Enumerable.Empty<DebtorCase>();
        }

        private async Task<IEnumerable<DebtorCase>> DebtorCases(Debtor debtor, string companyName)
        {
            var debtorCasesList = new List<DebtorCase>();
            foreach (var debtorCase in debtor.Cases)
            {
                var getDebtorCase = await _debtorManager.GetByContractId(
                    debtorCase.ContractId, companyName);
                debtorCasesList.Add(getDebtorCase);
            }

            return debtorCasesList;
        }

        private async Task<IEnumerable<DebtorCase>> MatchDebtorCaseByPersonalData(DebtorCase debtors, MatchObject matchObject, string companyName)
        {
            var debtorCasesList = new List<DebtorCase>();
            var contractList = FindContracts(debtors, matchObject);

            foreach (var contract in contractList)
            {
                var getDebtorCase = await _debtorManager.GetByContractId(contract.ContractId, companyName);
                debtorCasesList.Add(getDebtorCase);
            }

            return debtorCasesList;
        }

        private List<Case> FindContracts(DebtorCase debtors, MatchObject matchObject)
        {
            var contractList = new List<Case>();

            foreach (var debtor in debtors.Debtors)
            {
                foreach (var address in debtor.Addresses)
                {
                    var isMatchedPostalCode = address.PostalCode == matchObject.PostalCode;
                    var isMatchedCity = LevenshteinDistance.Compute(address.City, matchObject.City);
                    var isMatchedAddress = LevenshteinDistance.Compute(address.Street, matchObject.Street);

                    if (isMatchedPostalCode &&
                        isMatchedCity < CheckLengthForPossiblyMatch(LevensteinEnums.City, address.City) &&
                        isMatchedAddress < CheckLengthForPossiblyMatch(LevensteinEnums.Street, address.Street))
                    {
                        contractList.AddRange(debtor.Cases);
                    }
                }
            }

            return contractList;
        }

        private int CheckLengthForPossiblyMatch(LevensteinEnums option, string value)
        {
            if (option == LevensteinEnums.City && value.Length < 11)
            {
                return 1;
            }

            if (option == LevensteinEnums.Street && value.Length < 7)
            {
                return 1;
            }

            return 2;
        }
    }
}
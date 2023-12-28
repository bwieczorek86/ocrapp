using OcrPlugin.App.Azure.Storage.Debtors.Entities;
using OcrPlugin.App.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.Debtors
{
    public interface IDebtorManager
    {
        Task<DebtorCase> GetByContractId(string contractId, string companyName);
        Task<Debtor> GetByPublicId(string publicId, string companyName);
        Task<DebtorCase> GetDebtorCaseByPersonalData(string personalData, string companyName);
        Task Upsert(DebtorCase debtor, string companyName);
        Task Upsert(IEnumerable<DebtorIdentifierEntity> debtors, string companyName);
    }
}
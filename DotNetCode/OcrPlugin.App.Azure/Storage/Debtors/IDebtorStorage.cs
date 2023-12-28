using OcrPlugin.App.Azure.Storage.Debtors.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.Debtors
{
    public interface IDebtorStorage
    {
        Task<DebtorCaseEntity> FindDebtorCase(string name, string companyName);
        Task<DebtorIdentifierEntity> FindDebtorIdentifier(string name, string companyName);
        Task<DebtorCaseEntity> FindDebtorPersonalData(string name, string companyName);

        Task UpsertDebtorCase(DebtorCaseEntity debtorCaseEntity, string companyName);
        Task UpsertDebtorIdentifier(DebtorIdentifierEntity debtorEntity, string companyName);
        Task UpsertDebtorIdentifiers(IEnumerable<DebtorIdentifierEntity> debtorIdentifierEntities, string companyName);
        Task UpsertDebtorPersonalData(DebtorPersonalDataEntity debtorPersonalDataEntity, string companyName);
        Task UpsertDebtorPersonalData(IEnumerable<DebtorPersonalDataEntity> debtorPersonalDataEntities, string companyName);
    }
}
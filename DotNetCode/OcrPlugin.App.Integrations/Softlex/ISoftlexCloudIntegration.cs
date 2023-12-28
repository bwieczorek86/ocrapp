using OcrPlugin.App.Core.Integrations.Models;
using System.Threading.Tasks;

namespace OcrPlugin.App.Integrations.Softlex
{
    public interface ISoftlexCloudIntegration
    {
        Task UpdateCase(DebtLetter debtLetter);
        Task SendFileToFtp(DebtLetter debtLetter);
        Task InitData(string companyName);
        Task IncrementData();
    }
}
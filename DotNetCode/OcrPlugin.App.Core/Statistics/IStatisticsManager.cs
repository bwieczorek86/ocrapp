using System.Threading.Tasks;

namespace OcrPlugin.App.Core.Statistics
{
    public interface IStatisticsManager
    {
        Task EnsureInit(string companyName);
        Task IncreaseOcred(string companyName);
        Task IncreaseNotOcred(string companyName);
        Task IncreaseOcredNotSure(string companyName);
    }
}
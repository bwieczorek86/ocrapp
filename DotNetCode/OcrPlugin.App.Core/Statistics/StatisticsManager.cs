using OcrPlugin.App.Azure.Storage.Statistics;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.Statistics
{
    public class StatisticsManager : IStatisticsManager
    {
        private readonly IStatisticsStorage _statisticsStorage;
        private OcrStatisticsEntity _ocrStatisticsEntity;

        public StatisticsManager(IStatisticsStorage statisticsStorage)
        {
            _statisticsStorage = statisticsStorage;
        }

        public async Task EnsureInit(string companyName)
        {
            _ocrStatisticsEntity ??= await _statisticsStorage.GetCompanyStatistics(companyName);
        }

        public async Task IncreaseOcred(string companyName)
        {
            _ocrStatisticsEntity.IncreaseOcred();

            await _statisticsStorage.Upsert(_ocrStatisticsEntity, companyName);
        }

        public async Task IncreaseNotOcred(string companyName)
        {
            _ocrStatisticsEntity.IncreaseNotOcred();

            await _statisticsStorage.Upsert(_ocrStatisticsEntity, companyName);
        }

        public async Task IncreaseOcredNotSure(string companyName)
        {
            _ocrStatisticsEntity.IncreaseOcredNotSure();

            await _statisticsStorage.Upsert(_ocrStatisticsEntity, companyName);
        }
    }
}
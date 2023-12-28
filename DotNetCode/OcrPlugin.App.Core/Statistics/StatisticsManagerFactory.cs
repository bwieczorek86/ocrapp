using Microsoft.AspNetCore.Http;
using OcrPlugin.App.Azure.Storage.Statistics;
using OcrPlugin.App.Common;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OcrPlugin.App.Core.Statistics
{
    public class StatisticsManagerFactory : IStatisticsManagerFactory
    {
        /// <summary>
        /// Dictionary<CompanyName, IStatisticsManager>
        /// </summary>
        private readonly IDictionary<string, IStatisticsManager> _managers = new ConcurrentDictionary<string, IStatisticsManager>();

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStatisticsStorage _statisticsStorage;

        public StatisticsManagerFactory(IHttpContextAccessor httpContextAccessor, IStatisticsStorage statisticsStorage)
        {
            _httpContextAccessor = httpContextAccessor;
            _statisticsStorage = statisticsStorage;
        }

        /// <summary>
        /// Get singleton manager per company
        /// </summary>
        public IStatisticsManager GetManagerForCompany()
        {
            var companyName = _httpContextAccessor.GetCompanyName();
            _managers.TryGetValue(companyName, out var manager);

            return manager ?? CreateStatisticsManager(companyName);
        }

        // TODO this is awful as hell but I have no idea how else we can solve it
        // TODO maybe on the application startup - just init all of them?
        private IStatisticsManager CreateStatisticsManager(string companyName)
        {
            var statisticsManager = new StatisticsManager(_statisticsStorage);
            _managers.Add(new KeyValuePair<string, IStatisticsManager>(companyName, statisticsManager));

            return statisticsManager;
        }
    }
}
namespace OcrPlugin.App.Core.Statistics
{
    public interface IStatisticsManagerFactory
    {
        /// <summary>
        /// Get singleton manager per company
        /// </summary>
        IStatisticsManager GetManagerForCompany();
    }
}
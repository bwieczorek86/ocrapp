using OcrPlugin.App.Azure.Common.Constants;

namespace OcrPlugin.App.Azure.Storage.Statistics
{
    public class OcrStatisticsEntity : CustomTableEntity
    {
        public int Ocred { get; set; }
        public int OcredNotSure { get; set; }
        public int NotOcred { get; set; }

        public OcrStatisticsEntity()
        {
            PartitionKey = PartitionKeys.Statistics;
            RowKey = RowKeys.OcrStatistics;
        }

        public void IncreaseOcred()
        {
            Ocred += 1;
        }

        public void IncreaseOcredNotSure()
        {
            OcredNotSure += 1;
        }

        public void IncreaseNotOcred()
        {
            NotOcred += 1;
        }
    }
}
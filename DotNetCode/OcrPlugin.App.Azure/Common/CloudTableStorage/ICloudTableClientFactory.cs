using Microsoft.Azure.Cosmos.Table;

namespace OcrPlugin.App.Azure.Common.CloudTableStorage
{
    public interface ICloudTableClientFactory
    {
        public string Name { get; }

        public CloudTableClient CreateCloudTableClient();
    }
}
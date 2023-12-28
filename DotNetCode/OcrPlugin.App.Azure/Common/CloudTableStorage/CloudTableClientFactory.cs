using Microsoft.Azure.Cosmos.Table;

namespace OcrPlugin.App.Azure.Common.CloudTableStorage
{
    public class CloudTableClientFactory : ICloudTableClientFactory
    {
        public string Name { get; }

        private readonly CloudTableClient _cloudTableClient;

        private CloudTableClientFactory(string name, CloudTableClient cloudTableClient)
        {
            Name = name;
            _cloudTableClient = cloudTableClient;
        }

        public static CloudTableClientFactory Instance(string name, string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            return new CloudTableClientFactory(name, storageAccount.CreateCloudTableClient(new TableClientConfiguration()));
        }

        public CloudTableClient CreateCloudTableClient() => _cloudTableClient;
    }
}
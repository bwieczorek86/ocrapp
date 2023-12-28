using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Queue
{
    public class QueueManager : IQueueManager
    {
        private readonly IConfiguration _configuration;

        public QueueManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> InsertMessage(string message)
        {
            var connectionString = _configuration.GetConnectionString("AzureStorage");

            var queueClient = new QueueClient(
                connectionString,
                "myqueue-items");

            try
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                await queueClient.SendMessageAsync(Convert.ToBase64String(bytes));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
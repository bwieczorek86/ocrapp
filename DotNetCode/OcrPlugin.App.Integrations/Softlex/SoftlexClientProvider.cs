using OcrPlugin.App.Azure.Storage.AppBlazor;
using ServiceReference1;
using System.ServiceModel;

namespace OcrPlugin.App.Integrations.Softlex
{
    internal class SoftlexClientProvider : ISoftlexClientProvider
    {
        public SoftlexAPIClient SoftlexApiClient(SoftlexIntegrationConfig config)
        {
            var endpoint = new EndpointAddress(config.BaseAddress);
            var client = new SoftlexAPIClient(SoftlexAPIClient.EndpointConfiguration.BasicHttpBinding_ISoftlexAPI, endpoint);

            return client;
        }
    }
}
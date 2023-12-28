using OcrPlugin.App.Azure.Storage.AppBlazor;
using ServiceReference1;

namespace OcrPlugin.App.Integrations.Softlex
{
    internal interface ISoftlexClientProvider
    {
        public SoftlexAPIClient SoftlexApiClient(SoftlexIntegrationConfig config);
    }
}
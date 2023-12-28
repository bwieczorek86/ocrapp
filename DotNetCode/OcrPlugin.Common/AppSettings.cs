namespace OcrPlugin.Common
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string InternalPythonClientUrl { get; set; }
        public string TemplatesBlobBaseUri { get; set; }
        public string AzureFunctionsBaseUri { get; set; }
        public string AzureFunctionsValidateToken { get; set; }
        public string AzureFunctionsOcrToken { get; set; }
        public string AzureFunctionsTitleToken { get; set; }
        public string BingSubscriptionKey { get; set; }
        public string BingBaseUri { get; set; }
    }
}
using System;

namespace OcrPlugin.App.Azure.Storage.AppBlazor
{
    public sealed class SoftlexIntegrationConfig
    {
        public string CompanyName { get; set; }
        public string BaseAddress { get; set; }
        public string FirmIdentifier { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public DateTime LastIntegrationDate { get; set; }
    }
}
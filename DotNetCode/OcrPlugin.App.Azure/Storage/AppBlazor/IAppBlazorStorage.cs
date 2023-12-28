using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.AppBlazor
{
    public interface IAppBlazorStorage
    {
        Task<IEnumerable<SoftlexIntegrationConfig>> GetSoftlexIntegrations();
        Task SetSoftlexLastIntegrationDate(string companyName, DateTime integrationDate);
    }
}
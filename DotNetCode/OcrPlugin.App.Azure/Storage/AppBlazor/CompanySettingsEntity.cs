using OcrPlugin.App.Azure.Common.Constants;
using System;
using System.Collections.Generic;

namespace OcrPlugin.App.Azure.Storage.AppBlazor
{
    public class AppBlazorEntity : CustomTableEntity
    {
        public IEnumerable<SoftlexIntegrationConfig> SoftlexIntegrations { get; set; }

        [Obsolete("Do not use, for internal framework usage.", error: true)]
        public AppBlazorEntity()
        {
        }

        public AppBlazorEntity(string property)
        {
            PartitionKey = PartitionKeys.AppBlazor.Settings;
            RowKey = property;
        }
    }
}
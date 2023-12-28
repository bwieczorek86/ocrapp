using Microsoft.Azure.Cosmos.Table;
using OcrPlugin.App.Azure.Common.Constants;
using System;

namespace OcrPlugin.App.Azure.Storage.ClientSettings
{
    public class CompanySettingsEntity : TableEntity
    {
        [IgnoreProperty]
        public string Property => RowKey;
        public string Value { get; set; }
        public string Type { get; set; }

        [Obsolete("Do not use, for internal framework usage.", error: true)]
        public CompanySettingsEntity()
        {
        }

        public CompanySettingsEntity(string property)
        {
            PartitionKey = PartitionKeys.Setting;
            RowKey = property;
        }
    }
}
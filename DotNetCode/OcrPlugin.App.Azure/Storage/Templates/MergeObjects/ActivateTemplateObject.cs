using Microsoft.Azure.Cosmos.Table;
using OcrPlugin.App.Azure.Common.Constants;

namespace OcrPlugin.App.Azure.Storage.Templates.MergeObjects
{
    public class ActivateTemplateObject : TableEntity
    {
        public bool IsActive { get; set; }

        public ActivateTemplateObject(string templateName)
        {
            PartitionKey = PartitionKeys.TemplateEntity;
            RowKey = templateName;
            IsActive = true;
            ETag = "*";
        }
    }
}
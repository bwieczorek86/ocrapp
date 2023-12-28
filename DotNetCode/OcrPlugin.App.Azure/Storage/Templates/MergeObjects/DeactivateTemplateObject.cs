using Microsoft.Azure.Cosmos.Table;
using OcrPlugin.App.Azure.Common.Constants;

namespace OcrPlugin.App.Azure.Storage.Templates.MergeObjects
{
    public class DeactivateTemplateObject : TableEntity
    {
        public bool IsActive { get; set; }

        public DeactivateTemplateObject(string templateName)
        {
            PartitionKey = PartitionKeys.TemplateEntity;
            RowKey = templateName;
            IsActive = false;
            ETag = "*";
        }
    }
}
using OcrPlugin.App.Azure.Common.Constants;

namespace OcrPlugin.App.Azure.Storage.Templates.MergeObjects
{
    public class UpdateTemplateImageSizeObject : CustomTableEntity
    {
        public TemplateImageSizeEntity TemplateImageSize { get; set; }

        public UpdateTemplateImageSizeObject(
            string templateName,
            TemplateImageSizeEntity templateImageSize)
        {
            PartitionKey = PartitionKeys.TemplateEntity;
            RowKey = templateName;

            TemplateImageSize = templateImageSize;
            ETag = "*";
        }
    }
}

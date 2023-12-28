using OcrPlugin.App.Azure.Common.Constants;

namespace OcrPlugin.App.Azure.Storage.Templates.MergeObjects
{
    public class UpdateTemplateRankObject : CustomTableEntity
    {
        public int Rank { get; set; }

        public UpdateTemplateRankObject(
            string templateName,
            int rank)
        {
            PartitionKey = PartitionKeys.TemplateEntity;
            RowKey = templateName;

            Rank = rank;
            ETag = "*";
        }
    }
}

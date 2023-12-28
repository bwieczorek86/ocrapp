using Microsoft.Azure.Cosmos.Table;
using OcrPlugin.App.Azure.Common.Constants;
using System;

namespace OcrPlugin.App.Azure.Storage.OcredDocuments
{
    public class OcredDocument : TableEntity
    {
        public string Hash { get; set; }

        [Obsolete("Do not use, for internal framework usage.", error: true)]
        public OcredDocument()
        {
        }

        public OcredDocument(string hash)
        {
            PartitionKey = PartitionKeys.OcredDocumentsEntity;
            RowKey = hash;
        }
    }
}
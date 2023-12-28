using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Common.Extensions
{
    public static class BindingsExtensions
    {
        private static readonly JsonSerializerSettings CamelCaseSerializerSettings =
            new()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

        public static async Task UploadAsJson<T>(this ICloudBlob cloudBlob, T content)
        {
            cloudBlob.Properties.ContentType = "application/json";

            await using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(content, CamelCaseSerializerSettings))))
            {
                await cloudBlob.UploadFromStreamAsync(stream);
            }
        }

        public static async Task Upsert<T>(this CloudTable cloudTable, T entity)
            where T : ITableEntity
        {
            var insertOrReplace = TableOperation.InsertOrReplace(entity);

            await cloudTable.ExecuteAsync(insertOrReplace);
        }
    }
}
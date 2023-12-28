using Microsoft.Extensions.Logging;
using OcrPlugin.App.Azure.Common.CloudTableStorage;
using OcrPlugin.App.Azure.Common.Constants;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.OcredDocuments
{
    public class OcredDocumentsStorage : StorageBase, IOcredDocumentsStorage
    {
        public OcredDocumentsStorage(
            ILoggerFactory loggerFactory,
            ICloudTableClientFactoryResolver cloudTableClientFactoryResolver)
            : base(loggerFactory, cloudTableClientFactoryResolver.Resolve(StorageAccountType.Production))
        {
        }

        public async Task<bool> WasOcred(string documentHash, string companyName)
        {
            return await Exists<OcredDocument>(documentHash, documentHash, companyName);
        }

        public async Task Create(string documentHash, string companyName)
        {
            await Upsert(new OcredDocument(documentHash), companyName);
        }

        public async Task Delete(string documentHash, string companyName)
        {
            await Delete(new OcredDocument(documentHash), companyName);
        }
    }
}
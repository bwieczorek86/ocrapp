using OcrPlugin.App.Azure.Storage.OcredDocuments;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.NoRepetition
{
    public class NoRepetitionService : INoRepetitionService
    {
        private readonly IOcredDocumentsStorage _ocredDocumentsStorage;

        public NoRepetitionService(IOcredDocumentsStorage ocredDocumentsStorage)
        {
            _ocredDocumentsStorage = ocredDocumentsStorage;
        }

        public async Task<bool> WasOcred(byte[] image, string companyName)
        {
            var hash = GetHash(image);

            return await _ocredDocumentsStorage.WasOcred(hash, companyName);
        }

        private string GetHash(byte[] image)
        {
            using var md5 = MD5.Create();
            var computeHash = md5.ComputeHash(image);

            return BitConverter.ToString(computeHash)
                .Replace("-", string.Empty)
                .ToLower();
        }
    }
}
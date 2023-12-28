using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.OcredDocuments
{
    public interface IOcredDocumentsStorage
    {
        Task<bool> WasOcred(string documentHash, string companyName);
        Task Create(string documentHash, string companyName);
        Task Delete(string documentHash, string companyName);
    }
}
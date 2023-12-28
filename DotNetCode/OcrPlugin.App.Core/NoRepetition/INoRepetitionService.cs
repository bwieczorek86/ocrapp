using System.Threading.Tasks;

namespace OcrPlugin.App.Core.NoRepetition
{
    public interface INoRepetitionService
    {
        Task<bool> WasOcred(byte[] image, string companyName);
    }
}
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Queue
{
    public interface IQueueManager
    {
        Task<bool> InsertMessage(string message);
    }
}

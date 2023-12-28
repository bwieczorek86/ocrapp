using System.Threading.Tasks;

namespace OcrPlugin.App.Features
{
    public interface IFeaturesManager
    {
        Task<bool> IsEnabled(Feature feature, string companyName);
        Task<bool> IsDisabled(Feature feature, string companyName);
    }
}
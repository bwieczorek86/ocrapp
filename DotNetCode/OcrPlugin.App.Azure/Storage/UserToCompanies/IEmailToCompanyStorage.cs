using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.UserToCompanies
{
    public interface IEmailToCompanyStorage
    {
        Task<ApplicationUserEntity> FindByName(string userName);
        Task IncreaseFailedCount(ApplicationUserEntity applicationUserEntity);
        Task ResetFailedCount(ApplicationUserEntity applicationUserEntity);
    }
}
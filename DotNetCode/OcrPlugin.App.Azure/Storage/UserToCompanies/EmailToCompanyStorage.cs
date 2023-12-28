using Microsoft.Extensions.Logging;
using OcrPlugin.App.Azure.Common.CloudTableStorage;
using OcrPlugin.App.Azure.Common.Constants;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Storage.UserToCompanies
{
    public class EmailToCompanyStorage : StorageBase, IEmailToCompanyStorage
    {
        private string TableName { get; set; } = Table.Base.EmailToCompany;

        public EmailToCompanyStorage(ILoggerFactory loggerFactory, ICloudTableClientFactoryResolver cloudTableClientFactoryResolver)
            : base(loggerFactory, cloudTableClientFactoryResolver.Resolve(StorageAccountType.Production))
        {
        }

        public async Task<ApplicationUserEntity> FindByName(string userName)
        {
            if (userName is null)
            {
                return null;
            }

            return await RetrieveEntity<ApplicationUserEntity>(userName, userName, TableName);
        }

        public async Task IncreaseFailedCount(ApplicationUserEntity applicationUserEntity)
        {
            applicationUserEntity.FailedLogInCount++;

            await Upsert(applicationUserEntity, TableName);
        }

        public async Task ResetFailedCount(ApplicationUserEntity applicationUserEntity)
        {
            applicationUserEntity.FailedLogInCount = 0;

            await Upsert(applicationUserEntity, TableName);
        }
    }
}
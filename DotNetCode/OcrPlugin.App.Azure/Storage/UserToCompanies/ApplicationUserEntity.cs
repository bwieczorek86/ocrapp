using Microsoft.Azure.Cosmos.Table;
using System;

namespace OcrPlugin.App.Azure.Storage.UserToCompanies
{
    public class ApplicationUserEntity : TableEntity
    {
        [IgnoreProperty]
        public string Email => PartitionKey;

        public string CompanyTable { get; set; }
        public string PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public int FailedLogInCount { get; set; }
        public string Role { get; set; }

        [Obsolete("Do not use, for internal framework usage.", error: true)]
        public ApplicationUserEntity()
        {
        }
    }
}
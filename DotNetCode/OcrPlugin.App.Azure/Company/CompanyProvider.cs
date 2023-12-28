using Microsoft.Extensions.Configuration;

namespace OcrPlugin.App.Azure.Company
{
    public class CompanyProvider : ICompanyProvider
    {
        private readonly IConfiguration _configuration;

        public CompanyProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Company GetCompanyId(string name)
        {
            return new Company();
        }
    }
}
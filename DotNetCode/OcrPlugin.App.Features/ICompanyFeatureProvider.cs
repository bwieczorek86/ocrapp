using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Features
{
    internal interface ICompanyFeatureProvider
    {
        Task<IEnumerable<Feature>> GetAll(string companyName);
    }
}
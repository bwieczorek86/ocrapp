using OcrPlugin.App.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Core.Matching
{
    public interface IMatchService
    {
        Task<IEnumerable<DebtorCase>> Match(MatchObject matchObject, bool hasPublicId, string companyName);
    }
}
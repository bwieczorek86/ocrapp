using System.Collections.Generic;
using System.Threading.Tasks;

namespace OcrPlugin.App.Spelling
{
    public interface ISpellingCorrector
    {
        Task<IEnumerable<CorrectedModel>> Correct(IReadOnlyCollection<CorrectModel> correctModels);
    }
}
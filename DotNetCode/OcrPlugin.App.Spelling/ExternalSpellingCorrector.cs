using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcrPlugin.App.Spelling
{
    internal sealed class ExternalSpellingCorrector : ISpellingCorrector
    {
        private readonly BingSpellCheck _bingSpellCheck;

        public ExternalSpellingCorrector(BingSpellCheck bingSpellCheck)
        {
            _bingSpellCheck = bingSpellCheck;
        }

        public async Task<IEnumerable<CorrectedModel>> Correct(IReadOnlyCollection<CorrectModel> correctModels)
        {
            return await _bingSpellCheck.Correct(correctModels);
        }
    }
}
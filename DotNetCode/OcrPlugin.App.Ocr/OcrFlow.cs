using OcrPlugin.App.Core.Extensions;
using OcrPlugin.App.Core.Matching;
using OcrPlugin.App.Core.Models;
using OcrPlugin.App.Core.Reports;
using OcrPlugin.App.Core.SplitOcredProperties;
using OcrPlugin.App.Core.Templates.TemplateTypes;
using OcrPlugin.App.Ocr.Models;
using OcrPlugin.App.Ocr.TextSanitizing;
using OcrPlugin.App.Spelling;
using System.Drawing;

namespace OcrPlugin.App.Ocr
{
    internal sealed class OcrFlow : IOcrFlow
    {
        private static readonly ICollection<string> AllPropertiesGroup = new List<string>
            { "ContractId", "NIP", "Regon", "Pesel", "PublicId", "DebtorName", "Street", "City", "PostalCode" };
        private static readonly ICollection<string> NameAndAddressPropertiesGroup = new List<string>
            { "DebtorName", "Street", "City"};
        private static readonly ICollection<string> ContractAndIdPropertiesGroup = new List<string>
            {"ContractId", "NIP", "Regon", "Pesel", "PostalCode"};

        private readonly IOcrEngine _ocrEngine;
        private readonly ISpellingCorrector _spellingCorrector;
        private readonly IMatchService _matchService;
        private readonly ITextSanitizerFactory _textSanitizerFactory;
        private readonly ISplitOcredProperties _splitOcredProperties;

        public OcrFlow(
            IOcrEngine ocrEngine,
            ISpellingCorrector spellingCorrector,
            IMatchService matchService,
            ITextSanitizerFactory textSanitizerFactory,
            ISplitOcredProperties splitOcredProperties)
        {
            _ocrEngine = ocrEngine;
            _spellingCorrector = spellingCorrector;
            _matchService = matchService;
            _textSanitizerFactory = textSanitizerFactory;
            _splitOcredProperties = splitOcredProperties;
            _ocrEngine = ocrEngine;
        }

        public async Task<DirectoryOcrResult> OcrWithFlow(Template template, OcrFile ocrFile, string fileName, string companyName)
        {
            return await OcrByPropertiesGroup(template, ocrFile, fileName, AllPropertiesGroup, companyName);
        }

        private async Task<DirectoryOcrResult> OcrByPropertiesGroup(
            Template template,
            OcrFile ocrFile,
            string fileName,
            IEnumerable<string> propertiesGroup,
            string companyName)
        {
            var ocredModels = await OcrPropertiesGroup(template, ocrFile, propertiesGroup);
            var splitOcredModels = _splitOcredProperties.SplitDebtorAddressDataList(ocredModels);
            var sanitizedProperties = _textSanitizerFactory.GetSanitizer(template.Type).Sanitize(splitOcredModels);
            var result = new DirectoryOcrResult(fileName, template.Name, null);

            var correctModelList = new List<CorrectModel>();
            foreach (var ocredModel in sanitizedProperties.Where(x => NameAndAddressPropertiesGroup.Contains(x.PropertyName)))
            {
                correctModelList.Add(new CorrectModel(ocredModel.PropertyName, ocredModel.Text));
            }

            var correctedModel = await _spellingCorrector.Correct(correctModelList);

            foreach (var model in correctedModel)
            {
                result.CorrectedModels.Add(model);
            }

            foreach (var model in sanitizedProperties.Where(x => ContractAndIdPropertiesGroup.Contains(x.PropertyName)))
            {
                result.CorrectedModels.Add(new CorrectedModel
                {
                    Text = model.Text,
                    PropertyName = model.PropertyName
                });
            }

            if (!result.CorrectedModels.Any())
            {
                result.OcrDocumentError = new TemplatesFieldNotFoundError();
                return result;
            }

            result.Contracts = await _matchService.Match(GetMatchObject(result.CorrectedModels), template.Settings.HasPublicId, $"{companyName}");

            if (!result.Contracts.Any())
            {
                result.OcrDocumentError = new ContractsNotFoundError();
                return result;
            }

            return result;
        }

        private async Task<IReadOnlyCollection<OcredModel>> OcrPropertiesGroup(
            Template template,
            OcrFile ocrFile,
            IEnumerable<string> ocrGroup)
        {
            var ocredModels = new List<OcredModel>();

            // przesłać całą liste propertisów a nie pojedyńczy
            foreach (var property in template.FilledProperties.Where(x => ocrGroup.Contains(x.Name)))
            {
                var contentArea = property.CreateRectangle();
                var ocredModel = await Single(property.Name, ocrFile, contentArea);

                ocredModels.Add(ocredModel);
            }

            return ocredModels;
        }

        private async Task<OcredModel> Single(string propertyName, OcrFile ocrFile, Rectangle contentArea)
        {
            var replaceNewLines = propertyName != "DebtorName";
            var ocredText = await _ocrEngine.ReadText(ocrFile.Content, contentArea, ocrFile.ContentType, replaceNewLines);

            return new OcredModel
            {
                Text = ocredText,
                PropertyName = propertyName
            };
        }

        private MatchObject GetMatchObject(ICollection<CorrectedModel> resultCorrectedModels)
        {
            long.TryParse(resultCorrectedModels.FirstOrDefault(c => c.PropertyName == nameof(GeneralType.Nip))?.GetText(), out var nip);
            long.TryParse(resultCorrectedModels.FirstOrDefault(c => c.PropertyName == nameof(GeneralType.Pesel))?.GetText(), out var pesel);
            long.TryParse(resultCorrectedModels.FirstOrDefault(c => c.PropertyName == nameof(GeneralType.PublicId))?.GetText(), out var publicId);

            return new()
            {
                DebtorName = resultCorrectedModels.FirstOrDefault(c => c.PropertyName == nameof(GeneralType.DebtorName))?.GetText(),
                City = resultCorrectedModels.FirstOrDefault(c => c.PropertyName == nameof(GeneralType.City))?.GetText(),
                Nip = nip,
                Pesel = pesel,
                PostalCode = resultCorrectedModels.FirstOrDefault(c => c.PropertyName == nameof(GeneralType.PostalCode))?.GetText(),
                Street = resultCorrectedModels.FirstOrDefault(c => c.PropertyName == nameof(GeneralType.Street))?.GetText(),
                ContractId = resultCorrectedModels.FirstOrDefault(c => c.PropertyName == nameof(GeneralType.ContractId))?.GetText(),
                PublicId = publicId
            };
        }
    }
}
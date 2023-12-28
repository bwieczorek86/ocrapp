using OcrPlugin.App.Azure.Blobs;
using OcrPlugin.App.Common;
using OcrPlugin.App.Core.Extensions;
using OcrPlugin.App.Core.Models;
using OcrPlugin.App.Core.NoRepetition;
using OcrPlugin.App.Core.Reports;
using OcrPlugin.App.Core.SplitOcredProperties;
using OcrPlugin.App.Core.Templates;
using OcrPlugin.App.Features;
using OcrPlugin.App.Ocr.Models;
using OcrPlugin.App.Spelling;
using System.Drawing;

namespace OcrPlugin.App.Ocr
{
    public class OcrPlugin : IOcrPlugin
    {
        private readonly IOcrEngine _ocrEngine;
        private readonly INoRepetitionService _noRepetitionService;
        private readonly IFeaturesManager _featuresManager;
        private readonly ITemplateImageResize _templateImageResize;
        private readonly ITemplateManager _templateManager;
        private readonly IOcrFlow _ocrFlow;
        private readonly ISplitOcredProperties _splitOcredProperties;

        public OcrPlugin(
            IOcrEngine ocrEngine,
            INoRepetitionService noRepetitionService,
            IFeaturesManager featuresManager,
            ITemplateImageResize templateImageResize,
            ITemplateManager templateManager,
            IOcrFlow ocrFlow,
            ISplitOcredProperties splitOcredProperties)
        {
            _ocrEngine = ocrEngine;
            _noRepetitionService = noRepetitionService;
            _featuresManager = featuresManager;
            _templateImageResize = templateImageResize;
            _templateManager = templateManager;
            _ocrFlow = ocrFlow;
            _splitOcredProperties = splitOcredProperties;
        }

        public async Task<DirectoryOcrResult> SingleDocument(string fileName, string templateName, OcrFile ocrFile, string companyName)
        {
            var template = await _templateManager.Get(templateName, companyName);
            if (template == null)
            {
                template = await FindTemplateBasedOnTitle(ocrFile, companyName);
                if (template == null)
                {
                    return new DirectoryOcrResult(fileName, string.Empty, new TemplateTitleNotFoundError());
                }
            }

            if (await _featuresManager.IsEnabled(Feature.WasOcred, companyName))
            {
                if (await _noRepetitionService.WasOcred(ocrFile.Content, companyName))
                {
                    throw new NotImplementedException("If it was ocred already, we need to somehow decide what we display and what do we do");
                }
            }

            if (ocrFile.ContentType == OcrFileContentType.Image)
            {
                _templateImageResize.ImageResize(template, ocrFile.Content);
            }

            var result = await _ocrFlow.OcrWithFlow(template, ocrFile, fileName, companyName);

            await _templateManager.IncreaseRank(template, companyName);

            return result;
        }

        public async Task<OcredModel> Single(string propertyName, OcrFile ocrFile, Rectangle contentArea)
        {
            var replaceNewLines = propertyName != "DebtorName";
            var ocredText = await _ocrEngine.ReadText(ocrFile.Content, contentArea, ocrFile.ContentType, replaceNewLines);

            return new OcredModel
            {
                Text = ocredText,
                PropertyName = propertyName
            };
        }

        private async Task<CorrectModel> SingleWithoutCorrection(string propertyName, OcrFile ocrFile, Rectangle contentArea, string companyName)
        {
            if (await _featuresManager.IsEnabled(Feature.WasOcred, companyName))
            {
                if (await _noRepetitionService.WasOcred(ocrFile.Content, companyName))
                {
                    throw new NotImplementedException("If it was ocred already, we need to somehow decide what we display and what do we do");
                }
            }

            var replaceNewLines = propertyName == "DebtorName";
            var ocredText = await _ocrEngine.ReadText(ocrFile.Content, contentArea, ocrFile.ContentType, replaceNewLines);

            var ocrValue = new CorrectModel(propertyName, ocredText);
            return ocrValue;
        }

        private async Task<Template?> FindTemplateBasedOnTitle(OcrFile ocrFile, string companyName)
        {
            foreach (var template in await _templateManager.GetAllRanked(companyName))
            {
                var titleToOcr = template.Properties.FirstOrDefault(x => x.Name == "Title");
                if (titleToOcr == null)
                {
                    continue;
                }

                _templateImageResize.ImageResize(template, ocrFile.Content);
                var contentArea = titleToOcr.CreateRectangle();
                var correctModel = await SingleWithoutCorrection(titleToOcr.Name, ocrFile, contentArea, companyName);
                var titleFromTemplate = template.TitleTemplateMappings.Select(x => x.Title.ToUpper());
                foreach (var title in titleFromTemplate)
                {
                    var isFound = LevenshteinDistance.Compute(title, correctModel.Text.ToUpper());
                    if (isFound < 6)
                    {
                        var returnTemplate = await _templateManager.Get(template.Name, companyName);
                        return returnTemplate;
                    }
                }
            }

            return null;
        }

        public async Task<IDictionary<string, string>> OcrBeforeSave(IEnumerable<Property> properties, OcrFile ocrFile)
        {
            var result = await OcrPropertiesGroup(properties, ocrFile);

            return result.ToDictionary(c => c.PropertyName, c => c.Text);
        }

        private async Task<IReadOnlyCollection<OcredModel>> OcrPropertiesGroup(
            IEnumerable<Property> properties,
            OcrFile ocrFile)
        {
            var ocredModels = new List<OcredModel>();
            foreach (var property in properties)
            {
                var contentArea = property.CreateRectangle();
                var ocredModel = await Single(property.Name, ocrFile, contentArea);
                if (property.Name == "DebtorName")
                {
                    var splitedList = _splitOcredProperties.SplitDebtorAddressData(ocredModel);
                    foreach (var model in splitedList)
                    {
                        ocredModels.Add(model);
                    }
                }
                else
                {
                    ocredModels.Add(ocredModel);
                }
            }

            return ocredModels;
        }
    }
}
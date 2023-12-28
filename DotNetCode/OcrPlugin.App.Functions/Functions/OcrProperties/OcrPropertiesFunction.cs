using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OcrPlugin.App.Azure.Blobs;
using OcrPlugin.App.Azure.Storage.Features;
using OcrPlugin.App.Core.Debtors;
using OcrPlugin.App.Core.Models;
using OcrPlugin.App.Core.Templates;
using OcrPlugin.App.Ocr;
using OcrPlugin.App.Ocr.Models;

namespace OcrPlugin.App.Functions.Functions.OcrProperties;

public class OcrPropertiesFunction
{
    private readonly IFeaturesStorage _featuresStorage;
    private readonly ITemplateManager _templateManager;
    private readonly IDebtorManager _debtorManger;
    private readonly IBlobManager _blobManager;
    private readonly IOcrPlugin _ocrPlugin;
    private readonly ILogger _logger;

    public OcrPropertiesFunction(
        IFeaturesStorage featuresStorage,
        ITemplateManager templateManager,
        IDebtorManager debtorManager,
        IBlobManager blobManager,
        ILoggerProvider logger,
        IOcrPlugin ocrPlugin)
    {
        _featuresStorage = featuresStorage;
        _templateManager = templateManager;
        _debtorManger = debtorManager;
        _blobManager = blobManager;
        _ocrPlugin = ocrPlugin;
        _logger = logger.CreateLogger(nameof(Ocr));
    }

    [Function("ValidateProperties")]
    public async Task<IDictionary<string, string>> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req)
    {
        var ocrData = await GetDtoFromRequestData(req);
        if (ocrData == null)
        {
            throw new ArgumentException("Request does not contain any data.");
        }

        var template = await _templateManager.Get(ocrData.TemplateName, ocrData.CompanyName);
        var myBlob = await _blobManager.Get(template.FileName, ocrData.CompanyName);
        var ocrResult = await _ocrPlugin.OcrBeforeSave(ocrData.OcrProperties.Select(Map), new OcrFile
        {
            Content = myBlob.Data,
            ContentType = "jpg",
        });

        return ocrResult;
    }

    private static Property Map(OcrPropertyDto ocrPropertyDto) => new()
    {
        Name = ocrPropertyDto.Name,
        CordsEndX = ocrPropertyDto.CordsEndX,
        CordsEndY = ocrPropertyDto.CordsEndY,
        CordsStartX = ocrPropertyDto.CordsStartX,
        CordsStartY = ocrPropertyDto.CordsStartY,
    };

    private static async Task<OcrPropertiesDto> GetDtoFromRequestData(HttpRequestData req)
    {
        var content = await new StreamReader(req.Body).ReadToEndAsync();
        var ocrData = JsonConvert.DeserializeObject<OcrPropertiesDto>(content);
        return ocrData;
    }
}
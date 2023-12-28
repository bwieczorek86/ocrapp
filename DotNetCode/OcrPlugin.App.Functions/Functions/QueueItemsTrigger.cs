using Duende.IdentityServer.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OcrPlugin.App.Azure.Blobs;
using OcrPlugin.App.Azure.Queue;
using OcrPlugin.App.Core.Models;
using OcrPlugin.App.Core.Reports;
using OcrPlugin.App.Ocr;
using OcrPlugin.App.Ocr.Models;

namespace OcrPlugin.App.Functions.Functions;

public class QueueItemsTrigger
{
    private readonly IBlobManager _blobManager;
    private readonly IOcrPlugin _ocrPlugin;
    private readonly IReportsManager _reportsManager;
    private readonly ILogger _logger;

    public QueueItemsTrigger(
        IOcrPlugin ocrPlugin,
        IReportsManager reportsManager,
        IBlobManager blobManager,
        ILogger<QueueItemsTrigger> logger)
    {
        _ocrPlugin = ocrPlugin;
        _reportsManager = reportsManager;
        _blobManager = blobManager;
        _logger = logger;
    }

    [Function("QueueItemsTrigger")]
    public async Task Run(
        [QueueTrigger("myqueue-items", Connection = "AzureStorage")] string myQueueItem,
        FunctionContext context)
    {
        var queueModel = JsonConvert.DeserializeObject<QueueModel>(myQueueItem);
        var imageBlob = queueModel!.QueueFiles.FirstOrDefault(x => x.FileExtension.Contains("pdf"));
        var blob = await _blobManager.Get($"{imageBlob!.BlobFileName}.{imageBlob.FileExtension}", queueModel!.BlobContainer);
        var ocrFile = MapToOcrFile(blob, imageBlob.FileExtension);
        var companyName = queueModel.BlobContainer.Split("-")[0];

        try
        {
            if (!queueModel.Error.IsNullOrEmpty())
            {
                await AddFileErrorReport(queueModel, companyName);
                return;
            }

            var ocrResult = await _ocrPlugin.SingleDocument(queueModel.FileName, queueModel.TemplateName, ocrFile, companyName);
            await _reportsManager.Create(CreateReport(queueModel, ocrResult), companyName);
        }
        catch (Exception ex)
        {
            await AddFileErrorReport(queueModel, companyName, ex);
        }
    }

    private async Task AddFileErrorReport(QueueModel queueModel, string companyName, Exception? ex = null)
    {
        var invalidFieldError = new InvalidFileOrConfigurationError();
        await _reportsManager.Create(CreateReport(queueModel, new DirectoryOcrResult(fileName: queueModel.FileName, string.Empty, invalidFieldError)), companyName);

        if (ex != null)
        {
            _logger.LogError($"{invalidFieldError.Message}: {ex}");
        }
    }

    private OcrFile MapToOcrFile(IBlobFile<byte[]> blob, string blobExtension)
    {
        return new OcrFile
        {
            Content = blob.Data,
            ContentType = blobExtension
        };
    }

    private OcrResult CreateReport(QueueModel queueModel, DirectoryOcrResult ocrResult)
    {
        var report = new OcrResult
        {
            ReportId = queueModel.ReportId,
            FileName = queueModel.FileName,
            TemplateName = ocrResult.TemplateName,
            QueueFiles = queueModel.QueueFiles,
            Contracts = ocrResult.Contracts,
            CorrectedModels = ocrResult.CorrectedModels,
            ErrorMessage = ocrResult.OcrDocumentError
        };

        return report;
    }
}
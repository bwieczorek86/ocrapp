using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OcrPlugin.App.Azure.Blobs;
using OcrPlugin.App.Azure.Queue;
using OcrPlugin.App.BlazorClient.Server.Common;
using OcrPlugin.App.Common;
using OcrPlugin.App.Core.Models;
using OcrPlugin.App.Core.Reports;
using System.Web.Http;

namespace OcrPlugin.App.BlazorClient.Server.Components.Ocr
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcrController : ControllerBase
    {
        private readonly IQueueManager _queueManager;
        private readonly IBlobManager _blobManager;
        private readonly IReportsManager _reportsManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OcrController(
            IQueueManager queueManager,
            IBlobManager blobManager,
            IReportsManager reportsManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _queueManager = queueManager;
            _blobManager = blobManager;
            _reportsManager = reportsManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("upload-file")]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            if (!Request.Form.ContainsKey("FileData") || (!Request.Form.Files.Any()))
            {
                return BadRequest();
            }

            var jsonModel = Request.Form.First(f => f.Key == "FileData").Value;
            var json = JsonConvert.DeserializeObject<QueueModel>(jsonModel);
            var blobContainerName = json.BlobContainer;

            IFormFile myFile = Request.Form.Files.First();

            try
            {
                if (!ValidateFileSignature.IsFileSignatureValid(myFile))
                {
                    json.Error = "Invalid File or Configuration";
                    jsonModel = JsonConvert.SerializeObject(json);
                    await _queueManager.InsertMessage(jsonModel);
                    return Ok();
                }

                var pdfFile = json.QueueFiles.FirstOrDefault(x => x.FileExtension.Contains("pdf"));
                if (pdfFile != null)
                {
                    var convertedPdf = myFile.ConvertPdfToImage();
                    var convertedPdfQueueFile = new QueueFiles
                    {
                        BlobFileName = pdfFile.BlobFileName,
                        FileExtension = "jpg",
                        IsOriginal = false
                    };
                    pdfFile.BlobFileName = $"{Guid.NewGuid().ToString()}";

                    await _blobManager.Upload(
                        $"{convertedPdfQueueFile.BlobFileName}.{convertedPdfQueueFile.FileExtension}",
                        convertedPdf,
                        blobContainerName);

                    json.QueueFiles.Add(convertedPdfQueueFile);
                    jsonModel = JsonConvert.SerializeObject(json);
                }

                var originalBlobFileName = json.QueueFiles.FirstOrDefault(x => x.IsOriginal);
                await UploadBlobToOcr($"{originalBlobFileName!.BlobFileName}.{originalBlobFileName.FileExtension}", blobContainerName, myFile);
                await _queueManager.InsertMessage(jsonModel);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("get-report-by-id/{reportId}")]
        [Authorize]
        public async Task<IEnumerable<OcrResult>> GetReportById([FromUri] string reportId)
        {
            var report = await _reportsManager.GetAllOcrResults(reportId, _httpContextAccessor.GetCompanyName());

            return report;
        }

        [HttpPost("add-queue")]
        [Authorize]
        public async Task<IActionResult> AddQueue([FromBody] QueueModel queueModel)
        {
            var json = JsonConvert.SerializeObject(queueModel);
            var queueSuccess = await _queueManager.InsertMessage(json);

            if (queueSuccess)
            {
                return Ok();
            }

            return BadRequest();
        }

        private async Task UploadBlobToOcr(string fileName, string companyName, IFormFile file)
        {
            using var stream = file.OpenReadStream();
            await _blobManager.Upload(fileName, stream, companyName);
        }
    }
}
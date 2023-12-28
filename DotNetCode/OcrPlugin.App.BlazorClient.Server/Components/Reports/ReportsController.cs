using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OcrPlugin.App.BlazorClient.Server.Components.Reports.Mappings;
using OcrPlugin.App.BlazorClient.Server.Components.Reports.ReportDTO;
using OcrPlugin.App.BlazorClient.Shared.Reports;
using OcrPlugin.App.Common;
using OcrPlugin.App.Core.Models;
using OcrPlugin.App.Core.Reports;
using System.Web.Http;
using OcrResult = OcrPlugin.App.Core.Models.OcrResult;

namespace OcrPlugin.App.BlazorClient.Server.Components.Reports;

[Route("api/[controller]")]
[ApiController]
public partial class ReportsController : ControllerBase
{
    private readonly IReportsManager _reportsManager;
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReportsController(
        IReportsManager reportsManager,
        IMemoryCache memoryCache,
        IHttpContextAccessor httpContextAccessor)
    {
        _reportsManager = reportsManager;
        _memoryCache = memoryCache;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<ReportListDto>> Get()
    {
        var reports = await _memoryCache.GetOrCreateAsync("reports", cacheEntry =>
        {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
            return _reportsManager.GetAll(_httpContextAccessor.GetCompanyName());
        });

        return reports.OrderByDescending(report => report.DateTime)
                      .Select((report, iterator) => new ReportListDto(
                          Iterator: reports.Count() - iterator,
                          report.Id,
                          report.TemplateName,
                          report.UserName,
                          report.DateTime,
                          NumberOfFiles: report.ReportFiles.Count()))
                      .ToList();
    }

    [HttpGet("{reportId}")]
    [Authorize]
    public async Task<IActionResult> GetReport([FromUri] string reportId)
    {
        var report = await _memoryCache.GetOrCreateAsync($"reports{reportId}", cacheEntry =>
        {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
            return _reportsManager.Get(reportId, _httpContextAccessor.GetCompanyName());
        });

        if (report == null)
        {
            return NotFound();
        }

        foreach (var reportFile in report.ReportFiles)
        {
            var file = await _reportsManager.GetOcrResult(reportId, reportFile.FileNameOnDisc, _httpContextAccessor.GetCompanyName());
            reportFile.Status = GetStatus(file);
        }

        return Ok(new SingleReportDto(
            report.Id,
            report.TemplateName,
            report.ReportFiles.Select((reportFile, iterator) => new SingleReportFile(
                Iterator: report.ReportFiles.Count() - iterator,
                reportFile.FileName,
                reportFile.FileNameOnDisc,
                reportFile.Status)).ToList(),
            report.DateTime,
            report.UserName));
    }

    private static string GetStatus(OcrResult file) => file switch
    {
        null => Consts.FileProcessingStatus.Processing,
        _ => file.ErrorMessage.Type == null
            ? Consts.FileProcessingStatus.Processed
            : "Do weryfikacji"
    };

    [HttpGet("{reportId}/{fileId}")]
    [Authorize]
    public async Task<IActionResult> GetOcrResult([FromUri] string reportId, [FromUri] string fileId)
    {
        var report = await _memoryCache.GetOrCreateAsync($"reports{reportId}{fileId}", cacheEntry =>
        {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);
            return _reportsManager.GetOcrResult(reportId, fileId, _httpContextAccessor.GetCompanyName());
        });

        if (report == null)
        {
            return NotFound();
        }

        var blobFileName = report.QueueFiles.FirstOrDefault(x => !x.FileExtension.Contains("pdf"));

        return Ok(new OcrPlugin.App.BlazorClient.Shared.Reports.OcrResult(
            report.Id,
            report.ReportId,
            report.FileName,
            $"{blobFileName?.BlobFileName}.{blobFileName?.FileExtension}",
            report.TemplateName,
            report.ErrorMessage != null ? new OcrDocumentErrorDto(report.ErrorMessage.Type, report.ErrorMessage.Message) : null,
            report.CorrectedModels.Select(c => new CorrectedModelDto(c.PropertyName, c.Text, c.CorrectedText)).ToList(),
            report.Contracts.Select(c => new OcrPlugin.App.BlazorClient.Shared.Reports.DebtorCase(c.ContractId, c.Debtors.Select(d => new DebtorInCaseDto(
                GetCorrectedValue(report.CorrectedModels, nameof(c.ContractId), c.ContractId),
                GetCorrectedValue(report.CorrectedModels, nameof(d.DebtorName), d.DebtorName),
                GetCorrectedValue(report.CorrectedModels, nameof(d.PublicId), d.PublicId),
                GetCorrectedValue(report.CorrectedModels, nameof(d.Nip), d.Nip),
                GetCorrectedValue(report.CorrectedModels, nameof(d.Pesel), d.Pesel),
                GetCorrectedValue(report.CorrectedModels, nameof(d.Regon), d.Regon),
                GetCorrectedValue(report.CorrectedModels, nameof(d.Email), d.Email),
                d.Addresses.Select(a => new DebtorAddressDto(a.Street, a.City, a.PostalCode)).ToList(),
                d.PublicIdType)).ToList())).ToList()));
    }

    private static string GetCorrectedValue(ICollection<Spelling.CorrectedModel> correctedModels, string key, string defaultValue)
    {
        return correctedModels.FirstOrDefault(c => c.PropertyName == key)?.GetText() ?? defaultValue;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ReportCreateDto reportCreateDto)
    {
        try
        {
            var report = new Report
            {
                Id = reportCreateDto.ReportId,
                ReportFiles = reportCreateDto.FileNames.Select(fileName => new ReportFile
                {
                    FileName = fileName.FileName,
                    FileNameOnDisc = fileName.FileNameOnDisc
                }),
                TemplateName = reportCreateDto.TemplateName,
                UserName = _httpContextAccessor.GetUserName(),
                DateTime = DateTime.UtcNow,
            };

            await _reportsManager.Create(report, _httpContextAccessor.GetCompanyName());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e);
        }

        return Ok();
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] OcrResutltUpdateDto ocrResutltUpdateDto)
    {
        try
        {
            var updateOcrResult = ocrResutltUpdateDto.ToReportOcrResult();
            await _reportsManager.Update(updateOcrResult, _httpContextAccessor.GetCompanyName());
        }
        catch (Exception e)
        {
            // ???
            Console.WriteLine(e);
            throw;
        }

        return Ok();
    }

    [HttpPost("close")]
    [Authorize]
    public async Task<IActionResult> CloseReport(CloseReportDto closeReportDto)
    {
        return Ok();
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OcrPlugin.App.Azure.Blobs;
using OcrPlugin.App.BlazorClient.Client.modules.TemplateCreate.Cs;
using OcrPlugin.App.BlazorClient.Server.Common;
using OcrPlugin.App.BlazorClient.Shared.Templates;
using OcrPlugin.App.Common;
using OcrPlugin.App.Core.Models;
using OcrPlugin.App.Core.Templates;
using System.Drawing;
using System.Web.Http;

namespace OcrPlugin.App.BlazorClient.Server.Components.Templates;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TemplatesController : ControllerBase
{
    private readonly ITemplateManager _templateManager;
    private readonly IBlobManager _blobManager;
    private readonly OcrFunctionClient _ocrFunctionClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TemplatesController(
        ITemplateManager templateManager,
        IBlobManager blobManager,
        OcrFunctionClient ocrFunctionClient,
        IHttpContextAccessor httpContextAccessor)
    {
        _templateManager = templateManager;
        _blobManager = blobManager;
        _ocrFunctionClient = ocrFunctionClient;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("GetAll")]
    [Authorize]
    public async Task<IList<Template>> GetAll()
    {
        return (await _templateManager.GetAll(_httpContextAccessor.GetCompanyName())).ToList();
    }

    [HttpGet("GetAllNames")]
    [Authorize]
    public async Task<IList<string>> GetAllNames()
    {
        return (await _templateManager.GetAll(_httpContextAccessor.GetCompanyName())).Select(t => t.Name).ToList();
    }

    [HttpGet("{templateName}")]
    [Authorize]
    public async Task<IActionResult> GetByName(string templateName)
    {
        var template = await _templateManager.Get(templateName, _httpContextAccessor.GetCompanyName());
        if (template == null)
        {
            return NotFound();
        }

        return Ok(template);
    }

    [HttpPost("deactivate/{templateName}")]
    [Authorize]
    public async Task Deactivate([FromUri] string templateName)
    {
        await _templateManager.Deactivate(templateName, _httpContextAccessor.GetCompanyName());
    }

    [HttpPost("activate/{templateName}")]
    [Authorize]
    public async Task Activate([FromUri] string templateName)
    {
        await _templateManager.Activate(templateName, _httpContextAccessor.GetCompanyName());
    }

    [HttpPost("update")]
    [Authorize]
    public async Task Update([FromBody] Template template)
    {
        await _templateManager.Update(template, _httpContextAccessor.GetCompanyName());
    }

    [HttpPost("delete")]
    [Authorize]
    public async Task Delete([FromBody] Template template)
    {
        await _templateManager.Delete(template, _httpContextAccessor.GetCompanyName());
    }

    [HttpPost("validateProperties")]
    [Authorize]
    public async Task<IDictionary<string, string>> ValidateProperties([FromBody] ValidatePropertiesDto dto)
    {
        var ocrProperties = await _ocrFunctionClient.OcrProperties(dto.TemplateName, dto.Properties.Select(Map).ToList());

        return ocrProperties;
    }

    private static OcrPropertyDto Map(PropertyDto property)
        => new(property.Name, property.CordsStartX, property.CordsStartY, property.CordsEndX, property.CordsEndY);

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> Create()
    {
        if (!Request.Form.ContainsKey("Template"))
        {
            return BadRequest();
        }

        var jsonModel = Request.Form.First(f => f.Key == "Template").Value;
        var templateDto = JsonConvert.DeserializeObject<TemplateDto>(jsonModel);

        var myFileContent = Convert.FromBase64String(templateDto.FileBase64);

        var newTemplate = await NewTemplate(templateDto, myFileContent);

        try
        {
            await _templateManager.Create(newTemplate, _httpContextAccessor.GetCompanyName());
            await UploadBlob(templateDto, _httpContextAccessor.GetCompanyName(), myFileContent);
            return Ok();
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpPost("convert-to-image")]
    [Authorize]
    public IActionResult ConvertToImage()
    {
        var file = Request.Form.Files.GetFile("file");
        if (file == null || file.Length == 0)
        {
            return BadRequest("Please provide a valid PDF file.");
        }

        var imageStream = file.ConvertPdfToImage();

        return File(imageStream, "image/png");
    }

    private async Task<Template> NewTemplate(TemplateDto templateDto, byte[] myFileContent)
    {
        var newTemplate = new Template
        {
            Name = templateDto.Name,
            Properties = templateDto.Properties.Select(p => new Property
            {
                Name = p.Name,
                CordsEndX = p.CordsEndX,
                CordsEndY = p.CordsEndY,
                CordsStartX = p.CordsStartX,
                CordsStartY = p.CordsStartY
            }).ToList(),
            TitleTemplateMappings = templateDto.TitleTemplateMappings
                .Select(tm => new TitleTemplateMappings {Title = tm.Title}).ToList(),
            TemplateImageSize = await GetTemplateImageSize(myFileContent),
            Rank = 0,
            Type = templateDto.Type,
            FileName = templateDto.FileName,
            IsActive = templateDto.IsActive,
            Settings = new TemplateSettings
            {
                HasPublicId = templateDto.Settings.HasPublicId
            },
        };
        return newTemplate;
    }

    private async Task UploadBlob(TemplateDto template, string companyName, byte[] file)
    {
        await _blobManager.Upload(template.FileName, file, companyName);
        await UploadBlobMiniature(template, companyName, file);
    }

    private async Task UploadBlobMiniature(TemplateDto template, string companyName, byte[] file)
    {
        var thumbnail = await GetReducedImage(360, 480, file);
        await using var thumbnailStream = thumbnail.ToStream();
        await _blobManager.Upload($"tn_{template.FileName}", thumbnailStream, companyName);
    }

    private async Task<TemplateImageSize> GetTemplateImageSize(byte[] imageBytes)
    {
        // var imageBytes = await GetImageBytes(file);
        var templateImageSize = await GetImageSize(imageBytes);

        return templateImageSize;
    }

    private async Task<TemplateImageSize> GetImageSize(byte[] bytes)
    {
        var templateImageSize = new TemplateImageSize();
        await using (var ms = new MemoryStream(bytes))
        {
            var img = Image.FromStream(ms);

            templateImageSize.Height = img.Height;
            templateImageSize.Width = img.Width;
        }

        return templateImageSize;
    }

    private async Task<Image> GetReducedImage(int width, int height, byte[] resourceImage)
    {
        using var memoryStream = new MemoryStream(resourceImage);
        using var img = Image.FromStream(memoryStream);

        return img.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
    }
}
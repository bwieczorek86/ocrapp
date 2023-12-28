using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OcrPlugin.App.Azure.Common.GenerateBlobSasToken;
using OcrPlugin.App.BlazorClient.Shared.Auth;
using OcrPlugin.App.Common;

namespace OcrPlugin.App.BlazorClient.Server.Components.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SasTokenController : ControllerBase
    {
        private readonly ISasTokenGenerator _sasTokenGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SasTokenController(
            ISasTokenGenerator sasTokenGenerator,
            IHttpContextAccessor httpContextAccessor)
        {
            _sasTokenGenerator = sasTokenGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("gettoken")]
        [Authorize]
        public IActionResult GetSasToken()
        {
            if (!_httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }

            var companyName = _httpContextAccessor.GetCompanyName();
            var exampleTemplateImagesToken = _sasTokenGenerator.GetServiceSasTokenForContainer(Consts.BlobContainerNames.TemplateExampleImages(companyName));
            var blobToOcrToken = _sasTokenGenerator.GetServiceSasTokenForContainer(Consts.BlobContainerNames.BlobsToOcr(companyName));
            if (string.IsNullOrWhiteSpace(exampleTemplateImagesToken) || string.IsNullOrWhiteSpace(blobToOcrToken))
            {
                return Unauthorized();
            }

            return Ok(new SasTokens(exampleTemplateImagesToken, blobToOcrToken, companyName));
        }
    }
}
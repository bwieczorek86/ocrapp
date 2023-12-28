using OcrPlugin.App.Common;

namespace OcrPlugin.App.BlazorClient.Client.Common
{
    public class BlobUriBuilder
    {
        private readonly ILocalSessionService _localSessionService;
        private readonly IConfiguration _configuration;
        private readonly string _baseUri;

        public BlobUriBuilder(
            ILocalSessionService localSessionService, IConfiguration configuration)
        {
            _localSessionService = localSessionService;
            _configuration = configuration;
            _baseUri = _configuration["TemplatesBlobBaseUri"];
        }

        public async Task<string> GetTemplateBlobUri(string blobName, bool isMiniature)
        {
            var token = await _localSessionService.GetTemplatesTokenCookie();
            var companyName = await _localSessionService.GetCompanyNameCookie();

            var blobUri = !isMiniature ?
                $"{_baseUri}{Consts.BlobContainerNames.TemplateExampleImages(companyName)}/{blobName}{token}" :
                $"{_baseUri}{Consts.BlobContainerNames.TemplateExampleImages(companyName)}/tn_{blobName}{token}";

            return blobUri;
        }

        public async Task<string> GetBlobToOcrUri(string blobName)
        {
            var token = await _localSessionService.GetBlobReportsTokenCookie();
            var companyName = await _localSessionService.GetCompanyNameCookie();

            return $"{_baseUri}{Consts.BlobContainerNames.BlobsToOcr(companyName)}/{blobName}{token}";
        }
    }
}
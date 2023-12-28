using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using System;

namespace OcrPlugin.App.Azure.Common.GenerateBlobSasToken
{
    public class SasTokenGenerator : ISasTokenGenerator
    {
        private const string AccessToBlobContainer = "c";
        private readonly ILogger<SasTokenGenerator> _logger;
        private readonly BlobServiceClient _blobServiceClient;

        public SasTokenGenerator(
            IAzureClientFactory<BlobServiceClient> clientFactory,
            ILogger<SasTokenGenerator> logger)
        {
            _logger = logger;
            _blobServiceClient = clientFactory.CreateClient("BlobClient");
        }

        public string GetServiceSasTokenForContainer(string containerName, string storedPolicyName = null)
        {
            if (_blobServiceClient == null)
            {
                return LogAndReturn();
            }

            var container = _blobServiceClient.GetBlobContainerClient(containerName.ToLower());
            if (!container.CanGenerateSasUri)
            {
                return LogAndReturn();
            }

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = container.Name,
                Resource = AccessToBlobContainer
            };

            if (storedPolicyName == null)
            {
                sasBuilder.StartsOn = DateTimeOffset.UtcNow.AddMinutes(-1);
                sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddDays(1);
                sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
            }
            else
            {
                sasBuilder.Identifier = storedPolicyName;
            }

            var sasUri = container.GenerateSasUri(sasBuilder);
            var sasToken = sasUri.Query;

            return sasToken;
        }

        private string LogAndReturn()
        {
            _logger.LogError(@"BlobContainerClient must be authorized with Shared Key credentials to create a service SAS.");

            return string.Empty;
        }
    }
}
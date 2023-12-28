using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Blobs
{
    internal sealed class BlobManager : IBlobManager
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobManager(IAzureClientFactory<BlobServiceClient> clientFactory)
        {
            _blobServiceClient = clientFactory.CreateClient("BlobClient");
        }

        public async Task<IBlobFile<byte[]>> Get(string blobName, string containerName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName.ToLower());
            var blobClient = container.GetBlobClient(blobName);
            var doesBlobExists = (await blobClient.ExistsAsync()).Value;

            return doesBlobExists
                ? await GetBlobContent(blobClient)
                : BlobFile<byte[]>.Empty;
        }

        public async Task<BinaryData> GetBinaryData(string blobName, string containerName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName.ToLower());
            var blobClient = container.GetBlobClient(blobName);
            var doesBlobExists = (await blobClient.ExistsAsync()).Value;

            return doesBlobExists
                ? (await blobClient.DownloadContentAsync()).Value.Content
                : new BinaryData(string.Empty);
        }

        public async Task<IEnumerable<BinaryData>> GetBinaryData(string containerName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName.ToLower());
            var blobs = container.GetBlobs();

            var blobsToReturn = new List<BinaryData>(blobs.Count());
            foreach (var blob in blobs)
            {
                var blobClient = container.GetBlobClient(blob.Name);
                blobsToReturn.Add((await blobClient.DownloadContentAsync()).Value.Content);
            }

            return blobsToReturn;
        }

        public async Task Upload(string blobName, byte[] bytes, string containerName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName.ToLower());
            try
            {
                await using Stream stream = new MemoryStream(bytes);

                await container.UploadBlobAsync(blobName, stream);
            }
            catch (AggregateException ex)
            {
                throw;
            }
        }

        public async Task Upload(string blobName, Stream stream, string containerName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName.ToLower());
            try
            {
                await container.UploadBlobAsync(blobName, stream);
            }
            catch (AggregateException ex)
            {
                throw;
            }
        }

        public async Task Upload(string blobName, BinaryData binaryData, string containerName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName.ToLower());
            try
            {
                await container.UploadBlobAsync(blobName, binaryData);
            }
            catch (AggregateException ex)
            {
                throw new NotImplementedException($"Blob was not uploaded: {ex.Message}");
            }
        }

        public async Task Delete(string blobName, string containerName)
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName.ToLower());
            await DeleteBlob(blobName, container);
            await DeleteBlob($"tn_{blobName}", container);
        }

        private static async Task DeleteBlob(string blobName, BlobContainerClient container)
        {
            var blobClient = container.GetBlobClient(blobName);
            try
            {
                var doesBlobExists = (await blobClient.ExistsAsync()).Value;
                if (doesBlobExists)
                {
                    await container.DeleteBlobAsync(blobName);
                }
            }
            catch (RequestFailedException)
            {
                // The request fail sometimes when the blob does not exists
                // https://github.com/Azure/azure-sdk-for-net/issues/17129
                // The case is fixed, but it's happening sometimes.
                return;
            }
        }

        private static async Task<IBlobFile<byte[]>> GetBlobContent(BlobClient blob)
        {
            var contentStream = (await blob.DownloadContentAsync()).Value.Content.ToArray();

            return new BlobFile<byte[]>
            {
                Data = contentStream
            };
        }
    }
}
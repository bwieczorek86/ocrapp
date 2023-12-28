using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OcrPlugin.App.Azure.Blobs
{
    public interface IBlobManager
    {
        Task<BinaryData> GetBinaryData(string blobName, string containerName);
        Task<IEnumerable<BinaryData>> GetBinaryData(string containerName);
        Task<IBlobFile<byte[]>> Get(string blobName, string containerName);
        Task Upload(string blobName, byte[] bytes, string containerName);
        Task Upload(string blobName, Stream stream, string containerName);
        Task Upload(string blobName, BinaryData binaryData, string containerName);
        Task Delete(string blobName, string containerName);
    }
}
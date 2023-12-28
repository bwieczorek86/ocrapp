namespace OcrPlugin.App.Azure.Common.GenerateBlobSasToken
{
    public interface ISasTokenGenerator
    {
        string GetServiceSasTokenForContainer(string containerName, string storedPolicyName = null);
    }
}
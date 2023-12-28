namespace OcrPlugin.App.Azure.Common.CloudTableStorage
{
    public interface ICloudTableClientFactoryResolver
    {
        ICloudTableClientFactory Resolve(string name);
    }
}
using System.Collections.Generic;
using System.Linq;

namespace OcrPlugin.App.Azure.Common.CloudTableStorage
{
    public class CloudTableClientFactoryResolver : ICloudTableClientFactoryResolver
    {
        private readonly IEnumerable<ICloudTableClientFactory> _cloudTableClientFactories;

        public CloudTableClientFactoryResolver(IEnumerable<ICloudTableClientFactory> cloudTableClientFactories)
        {
            _cloudTableClientFactories = cloudTableClientFactories;
        }

        public ICloudTableClientFactory Resolve(string name)
        {
            return _cloudTableClientFactories.Single(x => x.Name == name);
        }
    }
}
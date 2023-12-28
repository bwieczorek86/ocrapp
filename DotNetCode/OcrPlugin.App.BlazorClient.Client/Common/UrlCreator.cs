using System.Web;

namespace OcrPlugin.App.BlazorClient.Client.Common
{
    internal sealed class UrlCreator : IUrlCreator
    {
        public string CreateRelative(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            var encodedPathUri = HttpUtility.UrlPathEncode(url);

            return encodedPathUri;
        }
    }
}
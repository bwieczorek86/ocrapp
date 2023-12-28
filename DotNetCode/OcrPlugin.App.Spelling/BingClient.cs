using OcrPlugin.Common;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OcrPlugin.App.Spelling;

public class BingClient
{
    private readonly HttpClient _client;
    private readonly FunctionAppSettings _functionAppSettings;

    public BingClient(
        HttpClient client,
        FunctionAppSettings functionAppSettings)
    {
        _client = client;
        _functionAppSettings = functionAppSettings;
        _client.Timeout = TimeSpan.FromSeconds(60);
    }

    public async Task<HttpResponseMessage> GetBingTokens(string queryString)
    {
        var url = GetUrl(queryString);

        _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _functionAppSettings.BingSubscriptionKey);
        var response = await _client.GetAsync(url);

        return response;
    }

    private string GetUrl(string queryString)
    {
        var requestUri = $"{_functionAppSettings.BingBaseUri}{queryString}";
        return requestUri;
    }
}
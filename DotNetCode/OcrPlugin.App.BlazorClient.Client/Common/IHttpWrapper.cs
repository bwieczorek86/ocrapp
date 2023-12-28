namespace OcrPlugin.App.BlazorClient.Client.Common
{
    public interface IHttpWrapper
    {
        Task<TClass> GetFromJsonAsync<TClass>(string uri);
        Task PostAsJsonAsync<TClass>(string uri, TClass tClass);
        Task PostAsync(string uri);
        Task<HttpResponseMessage> PostAsyncWithResponseWithoutToken<TClass>(string uri, TClass tClass);
        Task<HttpResponseMessage> PostAsyncWithResponse<TClass>(string uri, TClass tClass);
        Task<HttpResponseMessage> GetAsyncWithResponse(string uri);
        Task<T> GetAsync<T>(HttpRequestMessage request, Func<HttpResponseMessage, ValueTask> fallBack);
        Task<HttpResponseMessage> PostAsyncWithContentAndResponse(string uri, HttpContent content);
    }
}
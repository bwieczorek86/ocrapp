using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace OcrPlugin.App.BlazorClient.Client.Common
{
    public class HttpWrapper : IHttpWrapper
    {
        private const HttpContent EmptyBody = null;

        private readonly HttpClient _httpClient;
        private readonly ILocalSessionService _localSessionService;
        private readonly NavigationManager _navigationManager;

        public HttpWrapper(
            HttpClient httpClient,
            ILocalSessionService localSessionService,
            NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _localSessionService = localSessionService;
            _navigationManager = navigationManager;
        }

        public async Task<TClass> GetFromJsonAsync<TClass>(string uri)
        {
            var header = _httpClient.DefaultRequestHeaders.TryGetValues("Authorization", out _);
            if (!header)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _localSessionService.GetJwtToken());
            }

            var result = await _httpClient.GetFromJsonAsync<TClass>(uri);

            return result;
        }

        public async Task PostAsJsonAsync<TClass>(string uri, TClass tClass)
        {
            var header = _httpClient.DefaultRequestHeaders.TryGetValues("Authorization", out _);
            if (!header)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _localSessionService.GetJwtToken());
            }

            await _httpClient.PostAsJsonAsync(uri, tClass);
        }

        public async Task PostAsync(string uri)
        {
            var header = _httpClient.DefaultRequestHeaders.TryGetValues("Authorization", out _);
            if (!header)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _localSessionService.GetJwtToken());
            }

            await _httpClient.PostAsync(uri, EmptyBody);
        }

        public async Task<HttpResponseMessage> GetAsyncWithResponse(string uri)
        {
            var header = _httpClient.DefaultRequestHeaders.TryGetValues("Authorization", out _);
            if (!header)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _localSessionService.GetJwtToken());
            }

            return await _httpClient.GetAsync(uri);
        }

        public async Task<HttpResponseMessage> PostAsyncWithResponseWithoutToken<TClass>(string uri, TClass tClass)
        {
            var serialized = JsonConvert.SerializeObject(tClass);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);
            return response;
        }

        public async Task<HttpResponseMessage> PostAsyncWithResponse<TClass>(string uri, TClass tClass)
        {
            var header = _httpClient.DefaultRequestHeaders.TryGetValues("Authorization", out _);
            if (!header)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _localSessionService.GetJwtToken());
            }

            var serialized = JsonConvert.SerializeObject(tClass);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);
            return response;
        }

        public async Task<T> GetAsync<T>(HttpRequestMessage request, Func<HttpResponseMessage, ValueTask> fallBack)
        {
            var isHeaderPresent = request.Headers.TryGetValues("Authorization", out _);
            if (!isHeaderPresent)
            {
                request.Headers.Add("Authorization", "Bearer " + await _localSessionService.GetJwtToken());
            }

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _localSessionService.DeleteJwtToken();
                _navigationManager.NavigateTo("/login");
                return default;
            }

            if (!response.IsSuccessStatusCode)
            {
                await fallBack(response);
                return default;
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<HttpResponseMessage> PostAsyncWithContentAndResponse(string uri, HttpContent content)
        {
            var header = _httpClient.DefaultRequestHeaders.TryGetValues("Authorization", out _);
            if (!header)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + await _localSessionService.GetJwtToken());
            }

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

            var response = await _httpClient.PostAsync(uri, content);

            return response;
        }
    }
}
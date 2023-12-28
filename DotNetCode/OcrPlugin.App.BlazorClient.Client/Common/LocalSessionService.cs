using Microsoft.JSInterop;
using OcrPlugin.App.BlazorClient.Shared.Auth;

namespace OcrPlugin.App.BlazorClient.Client.Common
{
    public interface ILocalSessionService
    {
        Task CreateJwtToken(string email, string jwtBearer);
        Task<string> GetJwtToken();
        Task DeleteJwtToken();
        Task<string> GetCompanyNameCookie();
        Task<string> GetTemplatesTokenCookie();
        Task<string> GetBlobReportsTokenCookie();
        Task DeleteCompanyNameCookie();
        Task DeleteTokenCookie();
        Task SetCompanyNameCookie(string companyName);
        Task SetTokenCookie(SasTokens tokenKey);
    }

    public class LocalSessionService : ILocalSessionService
    {
        private const string TemplatesImagesToken = "templatesImagesToken";
        private const string BlobReportsToken = "blobReportsToken";

        private readonly IJSRuntime _jsRuntime;

        public LocalSessionService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task CreateJwtToken(string email, string jwtBearer)
        {
            var tokenBody = $"{email};{jwtBearer}";
            await CreateJwtTokenCookie("user", tokenBody);
        }

        public async Task<string> GetJwtToken()
        {
            var getJwtToken = await GetJwtTokenCookie("user");
            return getJwtToken;
        }

        public async Task DeleteJwtToken()
        {
            await DeleteJwtTokenCookie("user");
        }

        public async Task<string> GetCompanyNameCookie()
        {
            var getCompanyName = await ReadCookie("companyName");
            return getCompanyName;
        }

        public async Task<string> GetTemplatesTokenCookie()
            => await ReadCookie(TemplatesImagesToken);

        public async Task<string> GetBlobReportsTokenCookie()
            => await ReadCookie(BlobReportsToken);

        public async Task DeleteCompanyNameCookie()
            => await DeleteCookie("companyName");

        public async Task DeleteTokenCookie()
        {
            await DeleteCookie(TemplatesImagesToken);
            await DeleteCookie(BlobReportsToken);
        }

        public async Task SetCompanyNameCookie(string companyName)
        {
            await SetCookie("companyName", companyName.ToLower());
        }

        public async Task SetTokenCookie(SasTokens sasTokens)
        {
            await SetCookie(TemplatesImagesToken, sasTokens.TemplateImagesToken);
            await SetCookie(BlobReportsToken, sasTokens.BlobToOcrToken);
        }

        private async Task<string> ReadCookie(string name)
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", name);
        }

        private async Task SetCookie(string name, string value)
        {
            await _jsRuntime.InvokeAsync<string>("localStorage.setItem", name, value);
        }

        private async Task DeleteCookie(string name)
        {
            await _jsRuntime.InvokeAsync<string>("localStorage.removeItem", name);
        }

        private async Task CreateJwtTokenCookie(string name, string tokenBody)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", name, tokenBody)
                .ConfigureAwait(false);
        }

        private async Task<string> GetJwtTokenCookie(string name)
        {
            var userdata = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", name)
                .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(userdata))
            {
                var dataArray = userdata.Split(';', 2);
                if (dataArray.Length == 2)
                {
                    return dataArray[1];
                }
            }

            return null;
        }

        private async Task DeleteJwtTokenCookie(string user)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "user")
                .ConfigureAwait(false);
        }
    }
}
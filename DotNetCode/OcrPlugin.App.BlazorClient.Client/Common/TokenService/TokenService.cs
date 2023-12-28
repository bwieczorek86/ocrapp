using OcrPlugin.App.BlazorClient.Shared.Auth;
using System.Net.Http.Json;
using System.Web;

namespace OcrPlugin.App.BlazorClient.Client.Common.TokenService;

public class TokenService
{
    private readonly ILocalSessionService _localSession;
    private readonly IHttpWrapper _httpWrapper;

    public TokenService(
        IHttpWrapper httpWrapper,
        ILocalSessionService localSession)
    {
        _httpWrapper = httpWrapper;
        _localSession = localSession;
    }

    public async Task AddTokenToLocalSession()
    {
        using var httpClientHandler = new HttpClientHandler();
        using var httpClient = new HttpClient(httpClientHandler);

        // Todo dodać jwt token
        var response = await _httpWrapper.GetAsyncWithResponse($"api/SasToken/gettoken");

        if (response.IsSuccessStatusCode)
        {
            var tokenContent = await response.Content.ReadFromJsonAsync<SasTokens>();

            await _localSession.SetCompanyNameCookie(tokenContent!.CompanyName);
            await _localSession.SetTokenCookie(tokenContent);
        }
    }

    public async Task CheckIfTokenIsNotExpired()
    {
        var token1 = await _localSession.GetTemplatesTokenCookie();
        var token2 = await _localSession.GetBlobReportsTokenCookie();
        if (token1 == null || token2 == null)
        {
            await AddTokenToLocalSession();
        }
        else
        {
            var isAnyTokenInvalid = IsTokenDateInvalid(token1) || IsTokenDateInvalid(token2);
            if (isAnyTokenInvalid)
            {
                DeleteExpiredTokenFromLocalStorage();
                await AddTokenToLocalSession();
            }
        }
    }

    private static bool IsTokenDateInvalid(string token)
    {
        var validityDateParameterValue = HttpUtility.ParseQueryString(token).GetValues("se");
        return validityDateParameterValue?.Any() != true ||
               !DateTime.TryParse(validityDateParameterValue.First(), out var validityDateTime) ||
               DateTime.UtcNow > validityDateTime.ToUniversalTime();
    }

    private async void DeleteExpiredTokenFromLocalStorage()
    {
        await _localSession.DeleteTokenCookie();
        await _localSession.DeleteCompanyNameCookie();
    }
}
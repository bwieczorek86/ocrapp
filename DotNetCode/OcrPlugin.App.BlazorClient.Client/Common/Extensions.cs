using Microsoft.AspNetCore.Http;
using OcrPlugin.App.Common;

namespace OcrPlugin.App.BlazorClient.Client.Common
{
    public static class Extensions
    {
        public static string GetCompanyTableName(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext?.User.Claims.SingleOrDefault(claim => claim.Type == CustomClaimTypes.Company)?.Value;
        }

        public static string GetCompanyName(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext?.User.Claims.SingleOrDefault(claim => claim.Type == CustomClaimTypes.Company)?.Value;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }
    }
}
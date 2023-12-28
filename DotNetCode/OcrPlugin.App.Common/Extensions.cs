using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace OcrPlugin.App.Common
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

        public static string GetUserName(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext?.User.Identity!.Name;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }
    }
}
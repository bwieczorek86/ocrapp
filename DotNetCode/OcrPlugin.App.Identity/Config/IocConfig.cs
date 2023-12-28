using Microsoft.Extensions.DependencyInjection;

namespace OcrPlugin.App.Identity.Config
{
    public static class IocConfig
    {
        public static IServiceCollection RegisterIdentityServices(this IServiceCollection services)
        {
            return services.AddScoped<IJwtTokenProvider, JwtTokenProvider>()
                           .AddScoped<IPasswordHasher, PasswordHasher>();
        }
    }
}
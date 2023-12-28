using Microsoft.Extensions.DependencyInjection;
using OcrPlugin.App.Common;
using OcrPlugin.App.Integrations.Softlex;

namespace OcrPlugin.App.Integrations.Config
{
    public static class IocConfig
    {
        public static IServiceCollection RegisterIntegrations(this IServiceCollection services)
        {
            services.AddHostedService<SoftlexIntegrationBackgroundUpdater>();
            services.AddScoped<ISoftlexClientProvider, SoftlexClientProvider>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();

            return services.AddScoped<ISoftlexCloudIntegration, SoftlexCloudIntegration>();
        }
    }
}
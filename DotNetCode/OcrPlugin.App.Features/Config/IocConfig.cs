using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace OcrPlugin.App.Features.Config
{
    public static class IocConfig
    {
        public static IServiceCollection RegisterFeaturesModule(this IServiceCollection services, IEnumerable<string> features)
        {
            services.Configure<FeaturesSettings>(a => a.Features = features);

            return services.RegisterServices();
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IFeaturesManager, FeaturesManager>();
            services.AddScoped<ICompanyFeatureProvider, CompanyFeatureProvider>();

            return services;
        }
    }
}
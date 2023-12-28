using Microsoft.Extensions.DependencyInjection;
using OcrPlugin.App.Core.Templates;
using Scrutor;

namespace OcrPlugin.App.Core.Config
{
    public static class IocConfig
    {
        public static IServiceCollection RegisterCoreModule(this IServiceCollection services)
        {
            services.AddScoped<ITemplateManager, TemplateManager>();

            return services.Scan(scan => scan.FromCallingAssembly()
                .RegisterImplementedInterfaces());
        }

        private static IImplementationTypeSelector RegisterImplementedInterfaces(
            this IImplementationTypeSelector services)
        {
            return services.AddClasses(publicOnly: false)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithScopedLifetime();
        }
    }
}
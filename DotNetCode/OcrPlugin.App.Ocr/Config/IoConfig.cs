using Microsoft.Extensions.DependencyInjection;
using OcrPlugin.App.Ocr.TextSanitizing;
using Scrutor;

namespace OcrPlugin.App.Ocr.Config;

public static class IoConfig
{
    public static IServiceCollection RegisterOcrModule(this IServiceCollection services)
    {
        services.AddScoped<ITextSanitizer, GeneralTypeTextSanitizer>();
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
using Microsoft.Extensions.DependencyInjection;

namespace OcrPlugin.App.Spelling.Config
{
    public static class IocConfig
    {
        public static IConfigBuilder RegisterSpellingModule(this IServiceCollection services)
        {
            services.AddSingleton<BingSpellCheck>();
            services.AddHttpClient<BingClient>();

            return new SpellingConfigBuilder(services);
        }

        public static IServiceCollection AddExternalSpellingCorrector(this IConfigBuilder services)
        {
            return services.AddSingleton<ISpellingCorrector>(s =>
            {
                using var scope = s.CreateScope();
                var spellChecker = new ExternalSpellingCorrector(scope.ServiceProvider.GetRequiredService<BingSpellCheck>());

                return spellChecker;
            });
        }
    }
}
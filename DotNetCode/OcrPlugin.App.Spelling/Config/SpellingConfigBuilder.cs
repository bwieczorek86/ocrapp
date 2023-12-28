using Microsoft.Extensions.DependencyInjection;
using System;

namespace OcrPlugin.App.Spelling.Config
{
    internal class SpellingConfigBuilder : IConfigBuilder
    {
        private readonly IServiceCollection _services;

        internal SpellingConfigBuilder(IServiceCollection services)
        {
            _services = services;
        }

        IServiceCollection IConfigBuilder.AddSingleton<T>(Func<IServiceProvider, T> func)
        {
            T implementationInstance = func(_services.BuildServiceProvider());

            return _services.AddSingleton(typeof(T), implementationInstance);
        }
    }
}
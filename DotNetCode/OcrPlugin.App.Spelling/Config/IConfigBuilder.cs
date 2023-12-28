using Microsoft.Extensions.DependencyInjection;
using System;

namespace OcrPlugin.App.Spelling.Config
{
    public interface IConfigBuilder
    {
        internal IServiceCollection AddSingleton<T>(Func<IServiceProvider, T> func);
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OcrPlugin.App.Azure.Config;
using OcrPlugin.App.Core.Config;
using OcrPlugin.App.Features.Config;
using OcrPlugin.App.Ocr;
using OcrPlugin.App.Ocr.Config;
using OcrPlugin.App.Spelling.Config;
using OcrPlugin.Common;

var hostBuilder = new HostBuilder().ConfigureFunctionsWorkerDefaults();

hostBuilder.ConfigureLogging(c => c.AddConsole());

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var azureStorageConnectionString = config.GetConnectionString("AzureStorage");
var bingSettings = new FunctionAppSettings
{
    BingBaseUri = config.GetValue<string>("BingBaseUri"),
    BingSubscriptionKey = config.GetValue<string>("BingSubscriptionKey")
};

hostBuilder.ConfigureServices(serviceCollection =>
    serviceCollection.RegisterCoreModule()
        .AddSingleton(bingSettings)
        .RegisterFeaturesModule(new List<string>())
        .RegisterSpellingModule().AddExternalSpellingCorrector()
        .RegisterAzureStorageModule(azureStorageConnectionString)
        .RegisterOcrModule()
        .AddHttpContextAccessor());

var licenseKey = config.GetValue<string>("OcrLicenseKey");
OcrConfigurator.SetLicense(licenseKey!);

var host = hostBuilder.Build();

await host.RunAsync();
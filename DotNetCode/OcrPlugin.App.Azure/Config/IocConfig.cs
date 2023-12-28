using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using OcrPlugin.App.Azure.Common.CloudTableStorage;
using OcrPlugin.App.Azure.Common.Constants;
using Scrutor;

namespace OcrPlugin.App.Azure.Config
{
    public static class IocConfig
    {
        public static IServiceCollection RegisterAzureStorageModule(this IServiceCollection services, string azureStorageConnectionString)
        {
            services.AddSingleton<ICloudTableClientFactory>(
                CloudTableClientFactory.Instance(StorageAccountType.Production, azureStorageConnectionString));

            services.AddAzureClients(
                builder =>
                {
                    builder.AddBlobServiceClient(azureStorageConnectionString).WithName("BlobClient");
                });

            services.AddSingleton<ICloudTableClientFactoryResolver, CloudTableClientFactoryResolver>();

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
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using OcrPlugin.App.Core.Config;
using OcrPlugin.App.Identity.Config;
using OcrPlugin.App.Identity.Models;
using Scrutor;
using System.Reflection;

namespace OcrPlugin.App.BlazorClient.Server.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void RegisterDependencyInjection(this IServiceCollection services)
        {
            services
                .RegisterCoreModule()
                .RegisterIdentityServices()
                .RegisterScopedClassesMatchingInterface()
                .RegisterValidators();
        }

        private static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            return services.Scan(scan => scan.FromCallingAssembly()
                                             .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)), publicOnly: false)
                                             .AsImplementedInterfaces()
                                             .WithTransientLifetime());
        }

        private static IServiceCollection RegisterScopedClassesMatchingInterface(this IServiceCollection services)
        {
            return services.Scan(scan =>
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                var assemblies = entryAssembly!
                    .GetReferencedAssemblies()
                    .Where(assembly => assembly.Name!.StartsWith("OcrPlugin"))
                    .Select(Assembly.Load);

                scan.FromAssemblies(assemblies)
                    .AddClasses(publicOnly: false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsMatchingInterface()
                    .WithScopedLifetime();
            });
        }

        public static IdentityBuilder AddCustomIdentity(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAuthentication(options =>
                {
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddIdentityCookies(o => { });

            var builder = services.AddIdentityCore<ApplicationUser>(o =>
                {
                    o.Stores.MaxLengthForKeys = 128;
                })
                .AddDefaultTokenProviders();

            builder.AddSignInManager();
            return builder;
        }
    }
}
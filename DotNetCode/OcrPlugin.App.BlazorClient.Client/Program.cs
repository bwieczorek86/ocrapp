using Blazored.LocalStorage;
using Blazored.Modal;
using BlazorStyled;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OcrPlugin.App.BlazorClient.Client;
using OcrPlugin.App.BlazorClient.Client.Common;
using OcrPlugin.App.BlazorClient.Client.Common.TokenService;
using OcrPlugin.App.BlazorClient.Client.Utils;
using OcrPlugin.App.BlazorClient.Client.Utils.RouteHelper;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient(
        "OcrPlugin.App.BlazorClient.Server",
        client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("OcrPlugin.App.BlazorClient.Server"));

builder.Services.AddLocalization();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<SomethingChangedEvent>();
builder.Services.AddScoped<BlobUriBuilder>();
builder.Services.AddScoped<BetterNavigationManager>();
builder.Services.AddScoped<ILocalSessionService, LocalSessionService>();
builder.Services.AddScoped<IHttpWrapper, HttpWrapper>();
builder.Services.AddScoped<OcrApi>();
builder.Services.AddScoped<ITemplateTypeManager, TemplateTypeManager>();
builder.Services.AddScoped<IUrlCreator, UrlCreator>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddBlazorStyled();
builder.Services.AddBlazoredModal();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
});

var http = new HttpClient()
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
};

builder.Services.AddScoped(sp => http);

using var response = await http.GetAsync("appsettings.Development.json");
using var stream = await response.Content.ReadAsStreamAsync();

builder.Configuration.AddJsonStream(stream);

await builder.Build().RunAsync();

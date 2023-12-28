using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using OcrPlugin.App.Azure.Config;
using OcrPlugin.App.BlazorClient.Server.Components.Templates;
using OcrPlugin.App.BlazorClient.Server.Configuration;
using OcrPlugin.App.Core.Config;
using OcrPlugin.App.Integrations.Config;
using OcrPlugin.Common;
using System.Globalization;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

builder.Logging.ClearProviders();
builder.Host.UseNLog();
if (!environment.IsDevelopment())
{
    var instrumentationKey = configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
    builder.Services.AddApplicationInsightsTelemetry(instrumentationKey);
}

// Add services to the container.
builder.Services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.ValueCountLimit = 10;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddHttpContextAccessor();
builder.Services
    .RegisterCoreModule()
    .RegisterAzureStorageModule(configuration.GetConnectionString("AzureStorage"))
    .RegisterIntegrations();

builder.Services.AddHttpClient<OcrFunctionClient>(nameof(OcrFunctionClient));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("eb043e2f-c564-4db7-acf0-04356671d196")) // NOTE: THIS SHOULD BE A SECRET KEY NOT TO BE SHARED; A GUID IS RECOMMENDED
    };
});

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddHeaderPropagation();

builder.Services.AddLocalization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
});

builder.Services.RegisterDependencyInjection();

Regex.CacheSize = 50;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SwaggerSetupExample v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

var cultures = new List<CultureInfo>() { new("pl") };

app.UseHeaderPropagation();

if (app.Environment.IsDevelopment())
{
    RegisterDeveloperPage(app);
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

if (!environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRequestLocalization(new RequestLocalizationOptions()
{
    SupportedCultures = cultures,
    DefaultRequestCulture = new RequestCulture("pl"),
    SupportedUICultures = cultures
});

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

void RegisterDeveloperPage(IApplicationBuilder app)
{
    app.UseDeveloperExceptionPage();
    app.UseMiddleware<DeveloperExceptionPageMiddleware413Handler>();
}
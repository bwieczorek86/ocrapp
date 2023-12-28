using Newtonsoft.Json;
using OcrPlugin.App.BlazorClient.Client.Common;
using System.Diagnostics;
using System.Text;

namespace OcrPlugin.App.BlazorClient.Server.Components.Templates;

public class OcrFunctionClient
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _client;

    public OcrFunctionClient(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        HttpClient client)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _client = client;
        _client.Timeout = TimeSpan.FromSeconds(60);
    }

    public async Task<IDictionary<string, string>> OcrProperties(string templateName, List<OcrPropertyDto> ocrPropertyDtos)
    {
        var serializedData = SerializeData(templateName, ocrPropertyDtos);
        var ocrFunctionUrl = GetUrl();
        var response = await _client.PostAsync(ocrFunctionUrl, serializedData);

        return response.IsSuccessStatusCode switch
        {
            false when Debugger.IsAttached => throw new Exception("Check if local ocr function is running."),
            false => throw new Exception("Something went reaaaaaaaaaaly bad."),

            _ => await response.Content.ReadFromJsonAsync<IDictionary<string, string>>()
        };
    }

    private StringContent SerializeData(string templateName, ICollection<OcrPropertyDto> ocrPropertyDtos)
    {
        var companyName = _httpContextAccessor.GetCompanyName();
        var ocrPropertiesDto = new OcrPropertiesDto(templateName, companyName, ocrPropertyDtos);
        var json = JsonConvert.SerializeObject(ocrPropertiesDto);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        return data;
    }

    private string GetUrl()
    {
        var azureFunctionBaseAddress = _configuration["AppSettings:AzureFunctionsBaseUri"];
        var azureFunctionToken = _configuration["AppSettings:AzureFunctionsOcrToken"];

        return $"{azureFunctionBaseAddress}validateProperties?code={azureFunctionToken}";
    }
}
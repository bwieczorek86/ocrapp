using OcrPlugin.App.BlazorClient.Client.DTOs;
using OcrPlugin.App.BlazorClient.Shared.Templates;
using System.Text.Json;

namespace OcrPlugin.App.BlazorClient.Client.Common
{
    public class OcrApi
    {
        private readonly IHttpWrapper _httpWrapper;

        public OcrApi(IHttpWrapper httpWrapper)
        {
            _httpWrapper = httpWrapper;
        }

        public async Task<IDictionary<string, string>> ValidateProperties(Template template)
        {
            var validateProperties = new ValidatePropertiesDto(template.Name, template.Properties.Where(x => x.IsNotEmpty()).Select(Map).ToList());
            var response = await _httpWrapper.PostAsyncWithResponse("/api/templates/validateProperties", validateProperties);

            // TODO we need to catch the exception
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IDictionary<string, string>>(content, GetJsonOptions());
        }

        private static PropertyDto Map(Property property)
            => new(property.Name, property.CordsStartX, property.CordsStartY, property.CordsEndX, property.CordsEndY);

        private static JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
    }
}
using OcrPlugin.App.BlazorClient.Client.DTOs;

namespace OcrPlugin.App.BlazorClient.Client.Common
{
    public class TemplateTypeManager : ITemplateTypeManager
    {
        public Type ResolveType(string templateType)
        {
            return typeof(GeneralType);
        }
    }
}

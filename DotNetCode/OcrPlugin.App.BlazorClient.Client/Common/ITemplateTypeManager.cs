namespace OcrPlugin.App.BlazorClient.Client.Common
{
    public interface ITemplateTypeManager
    {
        Type ResolveType(string templateType);
    }
}

namespace OcrPlugin.App.BlazorClient.Shared.Auth;

public record SasTokens(string TemplateImagesToken, string BlobToOcrToken, string CompanyName)
{
    public override string ToString()
    {
        return $"{{ templateImagesToken = {TemplateImagesToken}, blobToOcrToken = {BlobToOcrToken}, companyName = {CompanyName} }}";
    }
}
namespace OcrPlugin.App.BlazorClient.Shared.Reports;

public record CorrectedModelDto(string PropertyName, string Text, string CorrectedText)
{
    public string GetText()
    {
        return string.IsNullOrWhiteSpace(CorrectedText)
            ? Text
            : CorrectedText;
    }

    public string GetCorrectedText()
    {
        return CorrectedText;
    }
}
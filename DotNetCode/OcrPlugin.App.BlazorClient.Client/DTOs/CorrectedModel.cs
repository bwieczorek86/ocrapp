namespace OcrPlugin.App.BlazorClient.Client.DTOs
{
    public class CorrectedModel
    {
        public string PropertyName { get; set; }
        public string Text { private get; set; }
        public string CorrectedText { private get; set; }

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
}
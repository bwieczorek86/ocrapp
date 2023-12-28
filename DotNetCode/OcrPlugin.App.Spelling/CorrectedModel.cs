namespace OcrPlugin.App.Spelling
{
    public class CorrectedModel
    {
        public string PropertyName { get; set; }
        public string Text { get; set; }
        public string CorrectedText { get; set; }

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
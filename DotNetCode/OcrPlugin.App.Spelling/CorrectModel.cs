using System;

namespace OcrPlugin.App.Spelling
{
    public class CorrectModel
    {
        public string PropertyName { get; set; }
        public string Text { get; set; }

        [Obsolete("Do not use, for internal framework usage.", error: true)]
        public CorrectModel()
        {
        }

        public CorrectModel(string propertyName, string ocredText)
        {
            PropertyName = propertyName;
            Text = ocredText;
        }
    }
}
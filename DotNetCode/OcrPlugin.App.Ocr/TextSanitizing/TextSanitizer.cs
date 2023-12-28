using OcrPlugin.App.Spelling;
using System.Text.RegularExpressions;

namespace OcrPlugin.App.Ocr.TextSanitizing
{
    internal abstract class TextSanitizer
    {
        private static readonly string LeaveOnlyNumbersRegex = @"[^\d]";

        protected void SanitizeProperty(IReadOnlyCollection<OcredModel> properties, string propertyName, Func<string, string> sanitizeFunc)
        {
            var property = properties!.FirstOrDefault(c => c.PropertyName == propertyName);
            if (property != null)
            {
                property.Text = sanitizeFunc(property.Text);
            }
        }

        protected string LeaveOnlyLettersAndWhiteSpace(string text)
        {
            return new(text.Where(@char => char.IsLetter(@char) || char.IsWhiteSpace(@char)).ToArray());
        }

        protected string LeaveOnlyNumbers(string text)
        {
            return Regex.Replace(text, LeaveOnlyNumbersRegex, string.Empty);
        }
    }
}
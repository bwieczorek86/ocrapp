using OcrPlugin.App.Core.Templates.TemplateTypes;
using OcrPlugin.App.Spelling;

namespace OcrPlugin.App.Ocr.TextSanitizing
{
    internal class GeneralTypeTextSanitizer : TextSanitizer, ITextSanitizer
    {
        public bool DoesApply(string type)
        {
            return type == nameof(GeneralType);
        }

        public IReadOnlyCollection<OcredModel> Sanitize(IReadOnlyCollection<OcredModel> properties)
        {
            SanitizeProperty(properties, nameof(GeneralType.DebtorName), LeaveOnlyLettersAndWhiteSpace);
            SanitizeProperty(properties, nameof(GeneralType.City), LeaveOnlyLettersAndWhiteSpace);
            SanitizeProperty(properties, nameof(GeneralType.Nip), LeaveOnlyNumbers);
            SanitizeProperty(properties, nameof(GeneralType.Pesel), LeaveOnlyNumbers);
            SanitizeProperty(properties, nameof(GeneralType.Regon), LeaveOnlyNumbers);
            SanitizeProperty(properties, nameof(GeneralType.PublicId), LeaveOnlyNumbers);

            return properties;
        }
    }
}
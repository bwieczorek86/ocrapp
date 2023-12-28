using OcrPlugin.App.Azure.Storage.ClientSettings;
using OcrPlugin.App.Core.Models;

namespace OcrPlugin.App.Core.AppSettings
{
    public static class SettingsMapper
    {
        public static CompanySettings ToCompanySetting(this CompanySettingsEntity entity)
        {
            return new()
            {
                Property = entity.Property,
                Value = entity.Value,
            };
        }

        public static CompanySettingsEntity ToCompanySettingEntity(this CompanySettings companySettings)
        {
            return new(companySettings.Property)
            {
                Value = companySettings.Value,
            };
        }
    }
}
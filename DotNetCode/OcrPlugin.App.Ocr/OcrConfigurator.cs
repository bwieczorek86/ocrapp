namespace OcrPlugin.App.Ocr
{
    public static class OcrConfigurator
    {
        public static void SetLicense(string licenseKey)
        {
            IronOcr.License.LicenseKey = licenseKey;
        }
    }
}
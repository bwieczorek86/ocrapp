namespace OcrPlugin.App.BlazorClient.Client.Utils
{
    public static class ThemeFactory
    {
        private static readonly PrimaryTheme Theme = new();

        public static Theme GetTheme()
        {
            return Theme;
        }
    }
}
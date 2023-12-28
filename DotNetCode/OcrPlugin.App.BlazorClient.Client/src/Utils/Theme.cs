namespace OcrPlugin.App.BlazorClient.Client.Utils
{
    public abstract class Theme
    {
        public abstract Color Colors { get; set; }
        public abstract Dictionary<ColorScheme, ButtonColor> Buttons { get; set; }
    }
}
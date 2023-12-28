namespace OcrPlugin.App.BlazorClient.Client.Utils
{
    public class PrimaryTheme : Theme
    {
        public override Color Colors { get; set; } = new()
        {
            Primary = "#343434",
            Secondary = "#F7FFF7",
            Tertiary = "#2F3061",
            Alert = "#D8320E",
            Warning = "#F6EC10",
            Success = "#10F610"
        };

        public override Dictionary<ColorScheme, ButtonColor> Buttons { get; set; } = new()
        {
            {
                ColorScheme.Primary, new ButtonColor
                {
                    Color = "#FFE66D",
                    Hover = "#6CA6C1"
                }
            }
        };
    }
}
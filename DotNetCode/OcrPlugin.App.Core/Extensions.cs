namespace OcrPlugin.App.Core
{
    public static class CommonExtensions
    {
        public static bool IsPdf(this string @string)
        {
            return @string.EndsWith("pdf");
        }
    }
}
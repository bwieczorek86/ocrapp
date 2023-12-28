namespace OcrPlugin.App.BlazorClient.Client.Utils
{
    public static class Routes
    {
        public const string Home = "/";
        public const string Login = "/login";
        public const string TemplatesList = "/templates/list";
        public const string TemplateCreate = "/template-create";
        public const string OcrAll = "/ocr";
        public const string AdministratorPanel = "/panel";
        public const string Reports = "/reports";

        public static string TemplateOcr(string templateName) => $"/ocr/{templateName}";
    }
}
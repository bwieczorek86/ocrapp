namespace OcrPlugin.App.Common;

public static class Consts
{
    public static class FileProcessingStatus
    {
        public static string Processed = "Gotowy";
        public static string Processing = "Przetwarzanie";
        public static string Inited = "Wystartował";
    }

    public static class BlobContainerNames
    {
        public static string TemplateExampleImages(string companyName) => companyName;
        public static string BlobsToOcr(string companyName) => $"{companyName}-blobs-to-ocr";
        public static string Reports(string companyName) => $"{companyName}-reports";
    }
}
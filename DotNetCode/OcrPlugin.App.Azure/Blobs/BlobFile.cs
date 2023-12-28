namespace OcrPlugin.App.Azure.Blobs
{
    public class BlobFile<T> : IBlobFile<T>
    {
        public T Data { get; set; }

        public static readonly BlobFile<T> Empty = new();
    }
}
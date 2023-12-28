namespace OcrPlugin.App.Azure.Blobs
{
    public interface IBlobFile<T>
    {
        public T Data { get; set; }
    }
}
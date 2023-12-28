using System.Runtime.Serialization;

namespace OcrPlugin.App.Ocr
{
    [Serializable]
    public class DirectoryEmptyException : Exception
    {
        public DirectoryEmptyException()
        {
        }

        public DirectoryEmptyException(string message)
            : base(message)
        {
        }

        public DirectoryEmptyException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DirectoryEmptyException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
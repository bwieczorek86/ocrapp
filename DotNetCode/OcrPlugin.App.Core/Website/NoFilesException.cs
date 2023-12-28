using System;
using System.Runtime.Serialization;

namespace OcrPlugin.App.Core.Website
{
    [Serializable]
    public class NoFilesException : Exception
    {
        public NoFilesException()
        {
        }

        public NoFilesException(string message)
            : base(message)
        {
        }

        public NoFilesException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected NoFilesException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
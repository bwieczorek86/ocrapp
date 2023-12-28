using System.Runtime.Serialization;

namespace OcrPlugin.App.Ocr.Models;

[Serializable]
public class OcrEngineException : Exception
{
    public OcrEngineException()
    {
    }

    public OcrEngineException(string message)
        : base(message)
    {
    }

    public OcrEngineException(string message, Exception inner)
        : base(message, inner)
    {
    }

    protected OcrEngineException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
    }
}
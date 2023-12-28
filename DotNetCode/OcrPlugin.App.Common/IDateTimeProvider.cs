using System;

namespace OcrPlugin.App.Common
{
    public interface IDateTimeProvider
    {
        public DateTime GetUtcNow();
    }
}

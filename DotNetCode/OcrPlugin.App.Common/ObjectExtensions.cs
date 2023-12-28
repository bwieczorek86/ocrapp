using Newtonsoft.Json;
using System.IO;

namespace OcrPlugin.App.Common
{
    public static class ObjectExtensions
    {
        public static void Serialize(this object value, Stream s)
        {
            using var writer = new StreamWriter(s);
            using var jsonWriter = new JsonTextWriter(writer);
            var jsonSerializer = new JsonSerializer();

            jsonSerializer.Serialize(jsonWriter, value);
            jsonWriter.Flush();
        }

        public static T Deserialize<T>(this Stream s)
        {
            using (StreamReader reader = new StreamReader(s))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer ser = new JsonSerializer();
                return ser.Deserialize<T>(jsonReader);
            }
        }
    }
}
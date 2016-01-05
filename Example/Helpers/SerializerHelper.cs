using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace Example
{
    public class SerializerHelper
    {
        public static void Serialize<T>(T obj, string file)
        {
            using (var fs = File.CreateText(file))
            {
                var serializer = new JsonSerializer();
                serializer.Converters.Add(new StringEnumConverter());
                serializer.Formatting = Formatting.Indented;

                serializer.Serialize(fs, obj);
            }
        }

        public static T Deserialize<T>(string file) where T : class
        {
            using (var fs = File.OpenText(file))
            {
                var serializer = new JsonSerializer();
                serializer.Converters.Add(new StringEnumConverter());

                var result = serializer.Deserialize(fs, typeof(T));
                return result as T;
            }
        }
    }
}

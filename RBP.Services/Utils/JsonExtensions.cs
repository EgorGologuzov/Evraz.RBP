using Newtonsoft.Json;

namespace RBP.Services.Utils
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T value, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(value, formatting);
        }

        public static T FromJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static object? FromJson(this string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type);
        }
    }
}
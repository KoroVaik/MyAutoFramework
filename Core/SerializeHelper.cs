using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core
{
    public static class SerializeHelper
    {
        public static T? DeserializeJson<T>(string json)
        {
            T content = default!;
            try
            {
                content = JsonConvert.DeserializeObject<T>(json)!;
            }
            catch (Exception ex)
            {
                throw new JsonException($"Error deserializing json: {json}", ex);
            }
            return content;
        }

        public static JObject DeserializeJsonToJObject(string json)
        {
            JObject content = default!;
            try
            {
                content = JObject.Parse(json)!;
            }
            catch (JsonSerializationException ex)
            {
                throw new JsonSerializationException($"Error deserializing json: {json}", ex);
            }
            return content;
        }
    }
}

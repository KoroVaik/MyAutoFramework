using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Helper
{
    public static class JsonHelper
    {
        public static JObject ParseJsonFile(string filePath)
        {
            var jsonContent = ReadFile(filePath);
            return JObject.Parse(jsonContent)
                ?? throw new Exception($"Could not deserialize json by path: {filePath}");
        }

        public static T DeserializeJsonFile<T>(string filePath)
        {
            var jsonContent = ReadFile(filePath);
            return SerializeHelper.DeserializeJson<T>(jsonContent) 
                ?? throw new Exception($"Could not deserialize json by path: {filePath}");
        }

        public static T DeserializeJsonFile<T>(string filePath, string jsonPath)
        {
            var jsonContent = ReadFile(filePath);
            var jObj = SerializeHelper.DeserializeJsonToJObject(jsonContent);
            var jToken = jObj.SelectToken(jsonPath, errorWhenNoMatch: true) 
                ?? throw new JsonException($"The JToken is null by path {jsonPath}");
            return jToken.ToObject<T>()!;
        }

        private static string ReadFile(string filePath)
        {
            if (!filePath.EndsWith(".json")) 
                throw 
                    new ArgumentException($"The specified path doesn't represent a JSON file. File path:\n{filePath}");
            return File.ReadAllText(filePath);
        }
    }
}

using Core.Helper;
using Newtonsoft.Json.Linq;

namespace Core.TestDataBuilder
{
    public class JsonDataChain
    {
        private string _jsonFilePath;
        public string ContainingDirectory => new FileInfo(_jsonFilePath).Directory!.Name;
        public string FileName => new FileInfo(_jsonFilePath).Name;
        public JObject Data { get; private set; }

        public JsonDataChain(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;
            Data = JsonHelper.ParseJsonFile(_jsonFilePath);
        }

        private JsonDataChain()
        {
            _jsonFilePath = null!;
            Data = new();
        }

        public void Merge(JsonDataChain targetDataChain)
        {
            Data.Merge(targetDataChain.Data);
            _jsonFilePath = targetDataChain._jsonFilePath;
        }

        public static JsonDataChain Empty()
        {
            return new JsonDataChain();
        }
    }
}

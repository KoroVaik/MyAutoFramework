using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Core.TestDataBuilder
{
    public class TestDataProvider
    {
        private readonly JObject _testDataJObj;

        public string RawJson => JsonConvert.SerializeObject(_testDataJObj, Formatting.Indented);
        public TestDataProvider(TestContext testContext)
        {
            var testClassName = testContext.Test.Type!.Name;
            var foldersToTestClassFile = testContext.Test.Namespace!
                .Split(".")
                .Skip(1)
                .Append(testClassName + ".cs")
                .ToArray();
            var testClassRelativePath = Path.Combine(foldersToTestClassFile);
            var testClassPath = Path.Combine(Directory.GetCurrentDirectory(), testClassRelativePath);

            _testDataJObj = new JsonDataAggregator(testClassPath).Build();
        }

        public T Get<T>(string alias)
        {
            if (GetEntitesToken<T>() is not JObject entitiesObj)
                throw new KeyNotFoundException($"Test data of type '{typeof(T).Name}' must be an object.");


            var testDataJToken = entitiesObj.GetValue(alias);

            if (testDataJToken == null)
                throw new KeyNotFoundException($"Test data of type '{typeof(T).Name}' with alias '{alias}' not found or null." +
                    $"\nFound aliases:\n{string.Join("\n", entitiesObj.Properties().Select(p => p.Name))}");

            var testDataObject = testDataJToken.ToObject<T>();

            return testDataObject!;
        }

        protected JToken GetEntitesToken<T>()
        {
            var testDataEntities = _testDataJObj.Properties();
            var testDataEntitiesByType = testDataEntities.FirstOrDefault(p => p.Name == typeof(T).Name + "s");

            return testDataEntitiesByType?.Value
                ?? throw new KeyNotFoundException($"Test data of type '{typeof(T).Name}' not found.");
        }
    }
}

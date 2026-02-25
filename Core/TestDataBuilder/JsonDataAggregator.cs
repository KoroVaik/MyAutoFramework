using System.Runtime.CompilerServices;
using Core.TestDataBuilder.ChainHandlers;
using Newtonsoft.Json.Linq;

namespace Core.TestDataBuilder
{

    public class JsonDataAggregator
    {
        const string _testDataFolderName = "TestData";
        private readonly string _testClassFilePath;
        private readonly string _testClassFolderPath;
        readonly List<AbstractDataChainHandler> _chainHandlers = new() {
            //new TemplateChainHandler()
        };

        public JsonDataAggregator(string testClassFilePath)
        {
            _testClassFilePath = testClassFilePath;
            _testClassFolderPath = Path.GetDirectoryName(_testClassFilePath)!;
        }

        public JObject Build()
        {
            var testData = AggregateTestData();

            new TemplateChainHandler().Handle(testData);
            new DynamicDataChainHandler().Handle(testData);
            return testData.Data;
        }

        private JsonDataChain AggregateTestData()
        {
            var binFolderPath = Directory.GetCurrentDirectory();
            var binFolder = new JsonDataFolder(binFolderPath);

            var resultData = MergeTestDataChainsInFolder(binFolder);

            return resultData;
        }

        private JsonDataChain MergeTestDataChainsInFolder(JsonDataFolder folder)
        {
            var aggregatedDataChain = JsonDataChain.Empty();

            var innerFolders = folder.SearchInnerFolders();

            var testDataFolder = innerFolders.FirstOrDefault(f => f.Name == _testDataFolderName);
            if (testDataFolder != null)
            {
                var testDataChain = MergeTemplateDataChainsInFolder(testDataFolder);
                aggregatedDataChain.Merge(testDataChain);
            }

            if (folder.FolderPath == _testClassFolderPath)
            { 
                var testClassName = Path.GetFileNameWithoutExtension(_testClassFilePath);
                var jsonChain = folder.CollectDataChains().FirstOrDefault(c => c.FileName.StartsWith(testClassName));
                if (jsonChain != null)
                {
                    aggregatedDataChain.Merge(jsonChain);
                }
                else
                    throw new FileNotFoundException($"Expected test data file for test class '{testClassName}' not found in path: {folder.FolderPath}");
            }
            else
            {
                var followingFolderName = Path.GetRelativePath(folder.FolderPath, _testClassFilePath).Split(Path.DirectorySeparatorChar)[0];
                var followingFolder = innerFolders.FirstOrDefault(f => f.Name == followingFolderName);
                if (followingFolder != null)
                {
                    var followingFolderDataChain = MergeTestDataChainsInFolder(followingFolder);
                    aggregatedDataChain.Merge(followingFolderDataChain);
                }
                else
                    throw new DirectoryNotFoundException($"Expected folder '{followingFolderName}' not found in path: {folder.FolderPath}");
            }

            return aggregatedDataChain;
        }

        private JsonDataChain MergeTemplateDataChainsInFolder(JsonDataFolder folder)
        {
            var aggregatedDataChain = JsonDataChain.Empty();

            foreach (var jsonChain in folder.CollectDataChains())
            {
                aggregatedDataChain.Merge(jsonChain);
            }

            foreach (var innerFolder in folder.SearchInnerFolders())
            {
                var innerAggregatedChain = MergeTemplateDataChainsInFolder(innerFolder);
                aggregatedDataChain.Merge(innerAggregatedChain);
            }

            return aggregatedDataChain;
        }
    }
}

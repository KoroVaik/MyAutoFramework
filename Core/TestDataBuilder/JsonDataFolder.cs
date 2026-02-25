namespace Core.TestDataBuilder
{
    internal class JsonDataFolder
    {
        readonly string[] _testDataFilesSuffixes = ["TestData.json", "Template.json", "Templates.json"];

        string _folderPath;
        DirectoryInfo _directoryInfo;
        public string FolderPath => _folderPath;
        public string Name => _directoryInfo.Name!;

        public JsonDataFolder(string folderPath)
        {
            _folderPath = folderPath;
            _directoryInfo = new DirectoryInfo(_folderPath);

        }

        public JsonDataChain[] CollectDataChains()
        {
            var files = Directory.GetFiles(_folderPath);
            var jsonFiles = files.Where(filePath => _testDataFilesSuffixes.Any(suff => filePath.EndsWith(suff)));
            var jsonChains = jsonFiles.Select(filePath => new JsonDataChain(filePath)).ToArray();
            return jsonChains;
        }

        public JsonDataFolder[] SearchInnerFolders()
        {
            var folders = Directory.GetDirectories(_folderPath);
            var innerFolders = folders.Select(folderPath => new JsonDataFolder(folderPath));
            return innerFolders.ToArray();
        }
    }
}

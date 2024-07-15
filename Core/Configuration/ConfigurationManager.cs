using Core.Configuration.Models;
using Microsoft.Extensions.Configuration;

namespace Core.Configuration
{
    public abstract class ConfigurationManager<T> where T : class
    {
        private readonly string _defaultSettingsFileName;
        private readonly string _pathToSettingsFolder;
        private readonly List<string> _settingFileNames = new();

        private static readonly object _lockObj = new();

        public ConfigurationManager(
            string defaultSettingsFileName = "settings.json",
            string? pathToSettingsFolder = null,
            List<string>? additionalSettingFiles = null)
        {
            _defaultSettingsFileName = defaultSettingsFileName;
            _pathToSettingsFolder = pathToSettingsFolder ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings");
            AddJsonAsSource(defaultSettingsFileName);

            additionalSettingFiles?.ForEach(AddJsonAsSource);
        }

        public abstract T? Current { get; protected set; }

        public void LoadEnvSettings()
        {
            lock (_lockObj)
            {
                if (Current == null)
                {
                    var envName = GetEnvName();
                    AddEnvironmentConfigs(envName);
                    Current = GetConfiguration().Get<Configurations<T>>().EnironmentConfigurations!;
                }
            }
        }

        private void AddEnvironmentConfigs(string envName)
        {
            var envConfigJson = _defaultSettingsFileName.Replace(".json", $".{envName}.json");
            AddJsonAsSource(envConfigJson);
        }

        protected void AddJsonAsSource(string jsonFileName)
        {
            _settingFileNames.Add(jsonFileName);
        }

        private string GetEnvName()
        {
            var builder = GetConfiguration();
            var currentEnv = builder.Get<Configurations>().Environment;
            return currentEnv!;
        }

        protected IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(_pathToSettingsFolder);

            _settingFileNames.ForEach(path => builder.AddJsonFile(path));

            builder.AddEnvironmentVariables()
            .AddJsonFile(_defaultSettingsFileName.Replace(".json", ".local.json"), optional: true);

            return builder.Build();
        }
    }
}

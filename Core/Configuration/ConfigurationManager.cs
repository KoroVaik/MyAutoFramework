using Core.Configuration.Models;
using Microsoft.Extensions.Configuration;

namespace Core.Configuration
{
    public abstract class ConfigurationManager<T> where T : class
    {
        private const string SettingsFileName = "settings.json";
        private static readonly string _localSettingsFileName = SettingsFileName.Replace(".json", ".local.json");
        private readonly string _pathToSettingsFolder;
        private readonly List<string> _settingJsonsToAdd;

        public ConfigurationManager(string pathToSettingsFolder = null, List<string> additionalSettingFiles = null)
        {
            _pathToSettingsFolder = pathToSettingsFolder ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings");
            _settingJsonsToAdd = new() { SettingsFileName };
            additionalSettingFiles.ForEach(AddJsonAsSource);
        }

        private static readonly object _lockObj = new();

        public abstract T CurrentConfigurations { get; protected set; }

        public void LoadEnvSettings()
        {
            if (CurrentConfigurations == null)
            {
                lock (_lockObj)
                {
                    var envName = GetEnvName();
                    AddEnvironmentConfigs(envName);
                    CurrentConfigurations = GetConfiguration().Get<Configurations<T>>().EnironmentConfigurations;
                }
            }
        }

        protected void AddJsonAsSource(string jsonFileName)
        {
            _settingJsonsToAdd.Add(jsonFileName);
        }

        private void AddEnvironmentConfigs(string envName)
        {
            var envConfigJson = SettingsFileName.Replace(".json", envName);
            AddJsonAsSource(envConfigJson);
        }

        private string GetEnvName()
        {
                var builder = GetConfiguration();
                var currentEnv = builder.Get<Configurations>().Environment;
                return currentEnv;
        }

        protected IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(_pathToSettingsFolder);

            _settingJsonsToAdd.ForEach(path => builder.AddJsonFile(path));

            builder.AddEnvironmentVariables()
            .AddJsonFile(_localSettingsFileName, optional: true);

            return builder.Build();
        }
    }
}

using Core.Configuration.Models;
using Microsoft.Extensions.Configuration;

namespace Core.Configuration
{
    public static class ConfigurationManager
    {
        private static readonly string _pathToSettings = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings");
        private static readonly string _configurationFileName = "settings.json";
        private static readonly string _localSettingsFileName = _configurationFileName.Replace(".json", ".local.json");
        private static readonly List<string> _jsonsToAdd = new() { _configurationFileName };

        private static readonly object _lockObj = new();

        public static Configurations CurrentConfigurations { get; private set; } = null!;

        public static void LoadSettingsForEnv<T>() where T : Configurations
        {
            if (CurrentConfigurations == null)
            {
                lock (_lockObj)
                {
                    var envName = GetEnvName();
                    AddEnvironmentConfigs(envName);
                    CurrentConfigurations = GetConfiguration().Get<T>();
                }
            }
        }

        public static void AddJsonAsSource(string jsonFileName)
        {
            _jsonsToAdd.Add(jsonFileName);
        }

        private static void AddEnvironmentConfigs(string envName)
        {
            var envConfigJson = _configurationFileName.Replace(".json", envName);
            AddJsonAsSource(envConfigJson);
        }

        private static string GetEnvName()
        {
                var builder = GetConfiguration();
                var currentEnv = builder.Get<Configurations>().Environment;
                return currentEnv;
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(_pathToSettings);

            _jsonsToAdd.ForEach(path => builder.AddJsonFile(path));

            builder.AddEnvironmentVariables()
            .AddJsonFile(_localSettingsFileName, optional: true);

            return builder.Build();
        }
    }
}

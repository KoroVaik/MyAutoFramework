using Core.Configuration.Models;
using Core.Extensions;
using Microsoft.Extensions.Configuration;

namespace Core.Configuration;

public abstract class ConfigurationManager<T> : ConfigurationManager where T : BaseTestConfigurations
{
    private readonly string _customEnvSettingsFileName;
    private readonly string _settingsFolder;
    private readonly List<string> _settingFileNames = new();

    private readonly Lazy<IConfiguration> _currentConfigs;

    protected ConfigurationManager(
        string? customEnvSettingsFileName = null,
        string? customSettingsFolder = null)
    {
        _customEnvSettingsFileName = customEnvSettingsFileName ?? SettingsFileName;
        _settingsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            customSettingsFolder ?? SettingsFolder);

        _currentConfigs = new Lazy<IConfiguration>(LoadEnvConfiguration);
    }

    public T CurrentConfigs
    {
        get
        {
            var configs = _currentConfigs.Value.Get<GenericEnvConfigurations<T>>()?.Configs
                ?? throw new Exception($"Configuratons must be of type {nameof(GenericEnvConfigurations<T>)} in settings json");
            Validate(configs);
            return configs;
        }
    }

    protected void AddJsonAsSource(string jsonFileName)
    {
        _settingFileNames.Add(jsonFileName);
    }

    private IConfiguration LoadEnvConfiguration()
    {
        var envName = GetEnvName();
        var envConfigJson =
            _customEnvSettingsFileName.Replace(".json", $".{envName.GetDescription().ToLower()}.json");
        AddJsonAsSource(envConfigJson);

        return GetConfiguration();
    }

    protected override IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(_settingsFolder);

        _settingFileNames.ForEach(path => builder.AddJsonFile(path));

        builder.AddConfiguration(base.GetConfiguration());

        return builder.Build();
    }
}
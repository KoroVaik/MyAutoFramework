using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Core.Configuration.Models;
using Microsoft.Extensions.Configuration;

namespace Core.Configuration
{
    public class ConfigurationManager
    {
        protected const string SettingsFileName = "settings.json";
        protected const string SettingsFolder = "Settings";

        public static Environments GetEnvName()
        {
            var builder = new ConfigurationManager().GetConfiguration();
            var configs = builder.Get<EnvConfigurations>()
                ?? throw new Exception($"Configuratons must be of type {nameof(EnvConfigurations)} in settings json");
            Validate(configs);

            var currentEnvStr = configs.Environment;
            var currentEnv = (Environments)Enum.Parse(typeof(Environments), currentEnvStr.ToUpper());

            return currentEnv;
        }

        protected virtual IConfiguration GetConfiguration()
        {
            var pathToSettingsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFolder);
            var builder = new ConfigurationBuilder()
                .SetBasePath(pathToSettingsFolder);

            builder.AddEnvironmentVariables()
                .AddJsonFile(SettingsFileName.Replace(".json", ".local.json"), optional: true);

            return builder.Build();
        }

        protected static void Validate<TModel>(TModel dataToValidate) where TModel : class
        {
            var ctx = new ValidationContext(dataToValidate);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(dataToValidate, ctx, results))
                return;

            var serializedObject = JsonSerializer.Serialize(dataToValidate);

            var errors = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
            throw new Exception($"Configuration validation failed for <{typeof(TModel).Name}>:" +
                $"{Environment.NewLine}{errors}" +
                $"{Environment.NewLine}Configs data: {serializedObject}");
        }
    }
}
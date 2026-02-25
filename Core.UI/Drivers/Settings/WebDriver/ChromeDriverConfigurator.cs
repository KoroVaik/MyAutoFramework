using Core.UI.Drivers.Factory;
using Core.Utils;
using OpenQA.Selenium.Chrome;

namespace Core.UI.Drivers.Settings.WebDriver
{
    public class ChromeDriverConfigurator : DriverConfigurator
    {
        public override ChromeDriver ConfigureDriver(WebDriverOptions webDriverOptions)
        {
            var binaryDirectory = DriverBinaryManager.GetChromeBinaryDirectory();
            return new ChromeDriver(binaryDirectory, GetOptions(webDriverOptions));
        }

        private ChromeOptions GetOptions(WebDriverOptions options)
        {
            var driverOptions = new ChromeOptions();
            if (options.IsHeadless)
            {
                _arguments.Add("--headless=new");
                _arguments.Add($"--window-size={options.ScreenWidth},{options.ScreenHeight}");
                _arguments.Add($"force-device-scale-factor={options.ScaleFactor}");
            }
            else
            {
                _arguments.Add("--start-maximized");
            }
            if (options.Arguments != null && options.Arguments.Any()) 
                _arguments = _arguments.Union(options.Arguments).ToList();
            driverOptions.AddArguments(_arguments);

            driverOptions.AddUserProfilePreference("download.default_directory", FileSystemUtils.DOWNLOADS_FOLDER_PATH);
            driverOptions.AddUserProfilePreference("download.prompt_for_download", false);
            driverOptions.AddUserProfilePreference("credentials_enable_service", false);
            driverOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            if (options.ProfilePreferences != null && options.ProfilePreferences.Any())
            {
                foreach (var preference in options.ProfilePreferences.Select(p => p.Split("=")))
                {
                    driverOptions.AddUserProfilePreference(preference[0], preference[1]);
                }
            }
            return driverOptions;
        }



    }
}

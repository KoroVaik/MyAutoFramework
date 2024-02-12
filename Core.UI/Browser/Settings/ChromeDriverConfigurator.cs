using Core.Configuration.Models;
using Core.UI.Browser.Configurations;
using Core.Utils;
using OpenQA.Selenium.Chrome;

namespace Core.UI.Browser.Settings
{
    public class ChromeDriverConfigurator : DriverConfigurator
    {
        private readonly WebDriverOptions _webDriverOptions;

        public ChromeDriverConfigurator(WebDriverOptions webDriverOptions)
        {
            _webDriverOptions = webDriverOptions;
        }

        public override ChromeDriver GetDriver()
        {
            var binaryDirectory = DriverBinaryManager.GetChromeBinaryDirectory();
            return new ChromeDriver(binaryDirectory, GetOptions(_webDriverOptions));
        }

        private ChromeOptions GetOptions(WebDriverOptions options)
        {
            var driverOptions = new ChromeOptions();
            if (options.isHeadless)
            {
                _arguments.Add("--headless=new");
                _arguments.Add($"--window-size={options.ScreenWidth},{options.ScreenHaight}");
                _arguments.Add($"force-device-scale-factor={options.ScaleFactor}");
            }
            else
            {
                _arguments.Add("--start-maximized");
            }
            if (options.Arguments!.Any()) _arguments = _arguments.Union(options.Arguments!).ToList();
            driverOptions.AddArguments(_arguments);

            driverOptions.AddUserProfilePreference("download.default_directory", FileSystemUtils.DOWNLOADS_FOLDER_PATH);
            driverOptions.AddUserProfilePreference("download.prompt_for_download", false);
            driverOptions.AddUserProfilePreference("credentials_enable_service", false);
            driverOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            if (options.ProfilePreferences!.Any())
            {
                foreach (var preference in options.ProfilePreferences!.Select(p => p.Split("=")))
                {
                    driverOptions.AddUserProfilePreference(preference[0], preference[1]);
                }
            }
            return driverOptions;
        }



    }
}

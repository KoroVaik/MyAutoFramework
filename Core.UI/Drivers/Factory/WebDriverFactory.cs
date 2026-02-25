using Core.UI.Configurations;
using Core.UI.Drivers.Settings.WebDriver;
using Core.UI.SearchContext;
using OpenQA.Selenium;

namespace Core.UI.Drivers.Factory
{
    public class WebDriverFactory
    {
        UiConfigurations _configs;

        public WebDriverFactory(UiConfigurations configurations)
        {
            _configs = configurations;
        }

        public virtual IWebDriver GetDriver()
        {
            var webDriverType = _configs.WebDriverType;

            var driverConfigurator = webDriverType switch
            {
                WebDriverType.LocalChrome => new ChromeDriverConfigurator(),
                _ => throw new NoSuchDriverException($"Web driver configurator for driver type '{webDriverType}' is not found.")
            };

            var options = _configs.DefaultWebDriversOptions.FirstOrDefault(o => o.WebDriverType == webDriverType)
                ?? throw new ArgumentException($"{webDriverType} web driver type is not listed in the array by path Configs.DefaultWebDriversOptions .");

            return driverConfigurator.ConfigureDriver(options);
        }
    }
}

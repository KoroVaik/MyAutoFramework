using Core.UI.Browser.Configurations;
using Core.UI.Browser.Settings;
using OpenQA.Selenium;

namespace Core.UI.Browser.Drivers
{
    public class WebDriverFactory : IWebDriverFactory
    {
        public IWebDriver GetDriver(BrowserType browserType, WebDriverOptions webDriverOptions)
        {
            var driverConfigurator = browserType switch
            {
                BrowserType.Chrome => new ChromeDriverConfigurator(webDriverOptions),
                _ => throw new NoSuchDriverException($"{browserType} browser is not supported")
            };
            return driverConfigurator.GetDriver();
        }
    }
}

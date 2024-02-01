using Core.Browser.Settings;
using Core.Configuration.Models;
using OpenQA.Selenium;

namespace Core.Browser.Drivers
{
    public class WebDriverFactory : IWebDriverFactory
    {
        public IWebDriver GetDriver(BrowserType browserType, WebDriverOptions webDriverOptions)
        {
            var driverConfigurator = browserType switch
            {
                BrowserType.Chrome => new ChromeDriverConfigurator(webDriverOptions)
            };
            return driverConfigurator.GetDriver();
        }
    }
}

using Core.UI.Browser.Configurations;
using OpenQA.Selenium;

namespace Core.UI.Browser.Drivers
{
    public interface IWebDriverFactory
    {
        IWebDriver GetDriver(BrowserType browserType, WebDriverOptions webDriverOptions);
    }
}

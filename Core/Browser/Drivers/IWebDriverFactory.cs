using Core.Configuration.Models;
using OpenQA.Selenium;

namespace Core.Browser.Drivers
{
    public interface IWebDriverFactory
    {
        IWebDriver GetDriver(BrowserType browserType, WebDriverOptions webDriverOptions);
    }
}

using Core.UI.Drivers.Factory;
using OpenQA.Selenium;

namespace Core.UI.Drivers.Settings
{
    public abstract class DriverConfigurator
    {
        protected List<string> _arguments = new()
            {
                "--disable-infobars",
                "--disable-extensions",
                "--disable-notifications",
                "--enable-automation",
                "--no-sandbox",
                "--disable-save-password-bubble"
            };

        public abstract IWebDriver ConfigureDriver(WebDriverOptions webDriverOptions);
    }
}

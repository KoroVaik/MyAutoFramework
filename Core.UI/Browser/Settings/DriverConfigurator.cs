using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Core.UI.Browser.Settings
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

        public abstract IWebDriver GetDriver();
    }
}

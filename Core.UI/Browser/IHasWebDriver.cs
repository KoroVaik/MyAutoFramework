using OpenQA.Selenium;

namespace Core.UI.Browser
{
    internal interface IHasWebDriver
    {
        IWebDriver WebDriver { get; }
    }
}

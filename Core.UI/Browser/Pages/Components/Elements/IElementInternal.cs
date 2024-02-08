using OpenQA.Selenium;

namespace Core.UI.Browser.Pages.Components.Elements
{
    internal interface IElementInternal
    {
        void Initialize(IBrowser browser, IWebElement webElement, IEnumerable<By> searchPathFragments);
    }
}

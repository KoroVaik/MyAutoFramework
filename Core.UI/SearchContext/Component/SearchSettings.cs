using OpenQA.Selenium;

namespace Core.UI.SearchContext.Component
{
    public class SearchSettings
    {
        public By Locator { get; internal set; } = null!;
        public int WaitTimeout { get; internal set; } = 4000;
        public bool IsRequired { get; internal set; } = true;
    }
}

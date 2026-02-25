using Core.UI.Extensions;
using OpenQA.Selenium;

namespace Core.UI.SearchContext.Component
{
    public class ComponentSearcher
    {
        protected readonly ISearchContext _currentContext;
        public By[] SearchContextPathFragments { get; private set; }

        public ComponentSearcher(ISearchContext context, List<By> searchContextPath)
        {
            _currentContext = context;
            SearchContextPathFragments = searchContextPath.ToArray();
        }

        public ElementWrapper SearchComponent(SearchSettings searchSettings)
        {
            string searchPathLog = BuildSearchPathLog(searchSettings.Locator);

            IWebElement? webElement = WaitHelper.Wait.ForNotDefault(
                () => _currentContext.FindElement(searchSettings.Locator),
                $"Element was not found by path: \n" + searchPathLog,
                searchSettings.WaitTimeout,
                throwOnFailure: searchSettings.IsRequired);

            return WrapComponent(webElement);
        }

        public IEnumerable<ElementWrapper> SearchComponents(SearchSettings searchSettings)
        {
            string searchPathLog = BuildSearchPathLog(searchSettings.Locator);
            IEnumerable<IWebElement> webElements = null!;

            WaitHelper.Wait.Until(() =>
            {
                webElements = _currentContext.FindElements(searchSettings.Locator);
                return webElements.Any();
            },
            "Elements were not found by path: \n" + searchPathLog,
            searchSettings.WaitTimeout);

            return webElements.Select(WrapComponent);
        }

        private ElementWrapper WrapComponent(IWebElement? webElement)
        {
            return new(webElement);
        }

        private string BuildSearchPathLog(By by)
        {
            var searchPathFragments = SearchContextPathFragments.Concat([by]);
            string searchPath = ByHelper.ConvertByFragmentsToString(searchPathFragments);
            return searchPath;
        }
    }
}

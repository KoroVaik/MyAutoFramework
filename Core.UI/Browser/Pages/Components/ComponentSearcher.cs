using Core.UI.Browser.Pages.Components.Elements;
using OpenQA.Selenium;
using Serilog;

namespace Core.UI.Browser.Pages.Components
{
    internal class ComponentSearcher
    {
        private static readonly ILogger _logger = Log.ForContext<ComponentSearcher>();

        private readonly ISearchContext _searchContext;
        private readonly IBrowser _browser;
        private readonly By[] _searchContextPathFragments;

        public ComponentSearcher(IBrowser browser)
        {
            _searchContext = ((IHasWebDriver)browser).WebDriver;
            _browser = browser;
            _searchContextPathFragments = new By[0];
        }

        public ComponentSearcher(IBrowser browser, ISearchContext searchContext, By[] searchContextPathFragments)
        {
            _searchContext = searchContext;
            _browser = browser;
            _searchContextPathFragments = searchContextPathFragments;
        }

        public TComponent SearchComponent<TComponent>(By by, int waitTimeout = 4000) where TComponent : Element
        {
            try
            {
                IWebElement? webElement = null;
                _browser.WaitForAction(() => webElement = _searchContext.FindElement(by), waitTimeout);
                return InitComponent<TComponent>(by, webElement!);
            }
            catch (WebDriverTimeoutException)
            {
                TComponent component = InitComponent<TComponent>(by, null);
                var searchPath = component is IHasSearchPath path ? path.SearchPath : by.ToString();
                _logger.Warning($"Element was not found (after {waitTimeout}ms) by path: \n" + searchPath);
                return component;
            }
        }

        public IEnumerable<TComponent> SearchComponents<TComponent>(By by, int waitTimeout = 4000) where TComponent : Element
        {
                IEnumerable<IWebElement> webElements = new List<IWebElement>();
                _browser.WaitForOrContinue(() =>
                {
                    webElements = _searchContext.FindElements(by);
                    return webElements.Any();
                }, waitTimeout);

                return webElements.Select(elem => InitComponent<TComponent>(by, elem));
        }

        private TComponent InitComponent<TComponent>(By by, IWebElement? webElement) where TComponent : Element
        {
            TComponent element = Activator.CreateInstance<TComponent>();
            element.Initialize(_browser, webElement, GetSearchContextPathFragmentsFor(by));
            return element;
        }

        private IEnumerable<By> GetSearchContextPathFragmentsFor(By by)
        {
            var searchContextPathFragments = _searchContextPathFragments.ToList();
            searchContextPathFragments.Add(by);
            return searchContextPathFragments;
        }
    }
}

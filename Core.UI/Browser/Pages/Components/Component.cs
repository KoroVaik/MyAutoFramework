using Core.UI.Browser.Pages.Components.Elements;
using OpenQA.Selenium;

namespace Core.UI.Browser.Pages.Components
{
    public class Component : Element, ISearchComponent
    {
        protected bool IsRoot => WebElement == null;

        private ComponentSearcher ComponentSearcher => new(Browser, WebElement, SearchPathFragments);

        public virtual TComponent GetComponent<TComponent>(By by, int waitTimeout) where TComponent : Element
        {
            return ComponentSearcher.SearchComponent<TComponent>(by, waitTimeout);
        }

        public virtual IEnumerable<TComponent> GetComponents<TComponent>(By by, int waitTimeout) where TComponent : Element
        {
            return ComponentSearcher.SearchComponents<TComponent>(by, waitTimeout);
        }
    }
}

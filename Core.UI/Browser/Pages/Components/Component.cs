using Core.UI.Browser.Pages.Components.Elements;
using OpenQA.Selenium;

namespace Core.UI.Browser.Pages.Components
{
    public class Component : Element, IComponent
    {
        protected bool IsRoot => WebElement == null;

        private ComponentSearcher ComponentSearcher => IsRoot
                ? new(Browser)
                : new(Browser, WebElement, SearchPathFragments);

        public virtual TComponent GetComponent<TComponent>(By by, int waitTimeout) where TComponent : class, IElement
        {
            return ComponentSearcher.SearchComponent<TComponent>(by, waitTimeout);
        }

        public virtual IEnumerable<TComponent> GetComponents<TComponent>(By by, int waitTimeout) where TComponent : class, IElement
        {
            return ComponentSearcher.SearchComponents<TComponent>(by, waitTimeout);
        }
    }
}

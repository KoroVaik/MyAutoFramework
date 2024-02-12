using Core.UI.Browser.Pages.Components.Elements;
using OpenQA.Selenium;

namespace Core.UI.Browser.Pages.Components
{
    public interface ISearchComponent
    {
        TComponent GetComponent<TComponent>(By by, int waitTimeout = 4000) where TComponent : Element;

        IEnumerable<TComponent> GetComponents<TComponent>(By by, int waitTimeout = 4000) where TComponent : Element;
    }
}

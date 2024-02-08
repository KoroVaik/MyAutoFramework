using Core.UI.Browser.Pages.Components.Elements;
using OpenQA.Selenium;

namespace Core.UI.Browser.Pages.Components
{
    public interface IComponent : IElement
    {
        TComponent GetComponent<TComponent>(By by, int waitTimeout = 4000) where TComponent : class, IElement;

        IEnumerable<TComponent> GetComponents<TComponent>(By by, int waitTimeout = 4000) where TComponent : class, IElement;
    }
}

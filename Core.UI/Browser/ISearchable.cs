using Core.UI.Browser.Pages.Components.Elements;
using OpenQA.Selenium;

namespace Core.UI.Browser
{
    public interface ISearchable
    {
        TComponent GetComponent<TComponent>(By by, int waitTimeout = 4000) 
            where TComponent : class, IElement;

        IReadOnlyCollection<TComponent> GetComponents<TComponent>(By by, int waitTimeout = 4000) 
            where TComponent : class, IElement;
    }
}

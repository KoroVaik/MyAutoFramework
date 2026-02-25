using Core.UI.SearchContext.Abstractions;
using Core.UI.WebDriverWrapper;

namespace Core.UI.SearchContext.Pages
{
    public class PageFactory
    {
        Browser _browser { get; }
        CustomContextEventHandlers? _contextEventHandlersByType;

        public PageFactory(Browser browser, CustomContextEventHandlers? contextEventHandlersByType)
        {
            _browser = browser;
            _contextEventHandlersByType = contextEventHandlersByType;
        }

        public virtual TPage GetPage<TPage>() where TPage : BasePage
        {
            var page = Activator.CreateInstance<TPage>();
            page.Initialize(_browser);
            _contextEventHandlersByType?.AssignHandlers(page);
            page.OpenContext();

            return page;
        }

        private void AssignEventHandlers(UiContext component)
        {
            var componentType = component.GetType();
            if (_contextEventHandlersByType != null && _contextEventHandlersByType.TryGetValue(componentType, out var handler))
            {
                component.SetEventHandlers(handler);
            }
        }
    }

    public class InvokationContext
    {
        public BasePage? CurrentPage { get; set; }

        public void CloseContext()
        {
            CurrentPage?.CloseContext();
            CurrentPage = null!;
        }
    }
}

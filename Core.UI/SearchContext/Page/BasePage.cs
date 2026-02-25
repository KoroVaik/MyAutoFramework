using AspectInjector.Broker;
using Core.UI.SearchContext.Abstractions;
using Core.UI.SearchContext.Component;
using Core.UI.WebDriverWrapper;
using OpenQA.Selenium;
using System.Reflection;

namespace Core.UI.SearchContext.Pages
{
    public class BasePage : UiContext, ISearcher, IHasBrowser
    {
        private Browser? _browser;

        //internal override event ContextEventHandler<ContextEventArgs>? OnContextOpened;
        //internal override event ContextEventHandler<ContextEventArgs>? OnContextClosed;
        //internal override event ActionEventHandler<ActionEventArgs>? OnBeforeAction;
        //internal override event ActionEventHandler<ActionEventArgs>? OnAfterAction;

        public PageMetaData MetaData { get; private set; }

        Browser IHasBrowser.Browser
        {
            get => _browser ?? throw new InvalidOperationException("Browser is not initialized in the page.");
            set => _browser = value;
        }

        ComponentSearcher ComponentSearcher => new ComponentSearcher(((IHasBrowser)this).Browser.WebDriver, []);

        public virtual void WaitForPageIsLoaded()
        {

        }

        public virtual bool IsLoaded()
        {
            var documentState = _browser!.ExecuteScript<string>("return document.readyState;");
            return documentState == "complete";
        }

        public void CloseAlert(bool isConfirmed)
        {
            var alert = _browser!.SwitchToAlert();
            if (isConfirmed)
                alert.Accept();
            else
                alert.Dismiss();

            _browser.SwitchToDefaultContent();
        }

        public void Refresh()
        {
            _browser!.RefreshCurrentPage();
        }

        internal void Initialize(Browser browser)
        {
            MetaData = GetPageMetaData(this);
            _browser ??= browser;
        }

        internal void OpenContext()
        {
            ContextEventHandlers.OnContextOpened(this, new ContextEventArgs());
        }

        internal void CloseContext()
        {
            ContextEventHandlers.OnContextClosed(this, new ContextEventArgs());
        }

        public ElementWrapper SearchComponent(SearchSettings searchSettings)
        {
            return ComponentSearcher.SearchComponent(searchSettings);
        }

        public IEnumerable<ElementWrapper> SearchComponents(SearchSettings searchSettings)
        {
            return ComponentSearcher.SearchComponents(searchSettings);
        }

        private static PageMetaData GetPageMetaData(BasePage page)
        {
            var attr = page.GetType().GetCustomAttribute<PageMetaDataAttribute>(true);
            return attr == null
                ? throw new InvalidOperationException($"PageMetaDataAttribute is not defined for page '{page.GetType().FullName}'")
                : attr.MetaData;
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class PageMetaDataAttribute : Attribute
        {
            public PageMetaData MetaData { get; private set; }
            public PageMetaDataAttribute(string resourseUri, string pageName)
            {
                MetaData = new PageMetaData()
                {
                    ResourseUri = resourseUri,
                    PageName = pageName
                };
            }
        }

        public class PageMetaData
        {
            public string ResourseUri { get; internal set; } = null!;
            public string PageName { get; internal set; } = null!;
        }

        [Aspect(Scope.PerInstance)]
        [Injection(typeof(UiActionAttribute))]
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        public class UiActionAttribute : Attribute
        {
            [Advice(Kind.Around, Targets = Target.Method)]
            public object OnMethod([Argument(Source.Name)] string methodName,
                                    [Argument(Source.Instance)] object instance,
                                    [Argument(Source.Target)] Func<object[], object> targetAction,
                                    [Argument(Source.Arguments)] object[] args)
            {
                var page = CheckForPageInstance(instance, methodName);
                page.ContextEventHandlers.OnBeforeAction(page, new ActionEventArgs(methodName, args, null));
                var returnValue = targetAction(args);
                page.ContextEventHandlers.OnAfterAction(page, new ActionEventArgs(methodName, args, returnValue));

                if (returnValue != null)
                {
                    var message = $"UI action '{methodName}' returned value: '{returnValue}' from element by path: ";
                }
                else
                {
                    var message = $"UI action '{methodName}' on element by path: ";
                }
                return returnValue!;
            }

            private BasePage CheckForPageInstance(object instance, string methodName)
            {
                if (instance is BasePage page)
                {
                    return page;
                }
                else
                {
                    throw new Exception($"Method '{methodName}' at type '{instance.GetType().Name}' does not support Action attribute.");
                }
            }
        }
    }
}

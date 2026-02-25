using AngleSharp.Dom;
using AspectInjector.Broker;
using Core.UI.SearchContext.Abstractions;
using Core.UI.WebDriverWrapper;
using OpenQA.Selenium;

namespace Core.UI.SearchContext.Component
{
    public class ComponentList<TBaseComponent> : List<TBaseComponent> where TBaseComponent : BaseComponent
    {

    }

    public class BaseComponent : UiContext, ISearcher, IHasBrowser
    {
        ComponentSearcher Searcher => new ComponentSearcher(Element.WebElement, GetComponentLocators());
        public SearchSettings SearchSettings { get; private set; } = null!;
        public ComponentMetadata Metadata { get; private set; }
        protected ElementWrapper Element { get; private set; } = null!;
        Browser IHasBrowser.Browser { get; set; } = null!;

        //internal override event ContextEventHandler<ContextEventArgs>? OnContextOpened;
        //internal override event ContextEventHandler<ContextEventArgs>? OnContextClosed;
        //internal override event ActionEventHandler<ActionEventArgs>? OnBeforeAction;
        //internal override event ActionEventHandler<ActionEventArgs>? OnAfterAction;

        ElementWrapper ISearcher.SearchComponent(SearchSettings searchSettings)
        {
            var component = Searcher.SearchComponent(searchSettings);
            return component;
        }

        IEnumerable<ElementWrapper> ISearcher.SearchComponents(SearchSettings searchSettings)
        {
            var components = Searcher.SearchComponents(searchSettings);
            return components;
        }

        private List<By> GetComponentLocators()
        {
            return ParentContext is BaseComponent parentComponent
                ? parentComponent.GetComponentLocators().Append(SearchSettings.Locator).ToList()
                : new List<By>();
        }

        internal void InitializeComponent(ElementWrapper element, UiContext parentContext, Browser browser, ComponentMetadata metadata,
            SearchSettings searchSettings)
        {
            Element = element;
            ((IHasBrowser)this).Browser = browser;
            Metadata = metadata;
            SearchSettings = searchSettings;
            ParentContext = parentContext;
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
                var component = CheckForComponentInstance(instance, methodName);
                component.ContextEventHandlers.OnBeforeAction(component, new ActionEventArgs(methodName, args, null));
                var returnValue = targetAction(args);
                component.ContextEventHandlers.OnAfterAction(component, new ActionEventArgs(methodName, args, returnValue));

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

            private BaseComponent CheckForComponentInstance(object instance, string methodName)
            {
                if (instance is BaseComponent component)
                {
                    return component;
                }
                else
                {
                    throw new Exception($"Method '{methodName}' at type '{instance.GetType().Name}' does not support Action attribute.");
                }
            }
        }
    }
}

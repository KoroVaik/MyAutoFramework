using Core.UI.SearchContext.Abstractions;
using Core.UI.WebDriverWrapper;

namespace Core.UI.SearchContext.Component
{
    public class ComponentFactory
    {
        Browser _browser;
        CustomContextEventHandlers? _contextEventHandlersByType;

        public ComponentFactory(Browser browser, CustomContextEventHandlers? contextEventHandlersByType)
        {
            _browser = browser;
            _contextEventHandlersByType = contextEventHandlersByType;
        }

        public List<BaseComponent> InitializeComponents(Type componentsListType, List<ElementWrapper> webElements, UiContext parentContext, ComponentMetadata metadata, SearchSettings searchSettings)
        {
            if (!typeof(List<BaseComponent>).IsAssignableFrom(componentsListType))
                throw new ArgumentException($"Type {componentsListType.FullName} is not a valid components list type.");

            var componentsList = (List<BaseComponent>)Activator.CreateInstance(componentsListType)!;
            var componentType = componentsListType.GenericTypeArguments[0];
            foreach (var webElement in webElements)
            {
                var component = InitializeComponent(componentType, webElement, parentContext, metadata, searchSettings);
                componentsList.Add(component);
            }

            return componentsList;
        }

        public BaseComponent InitializeComponent(Type componentType, ElementWrapper webElement, UiContext parentContext, ComponentMetadata metadata, SearchSettings searchSettings)
        {
            if (!typeof(BaseComponent).IsAssignableFrom(componentType))
                throw new ArgumentException($"Type {componentType.FullName} is not a valid component type.");

            var component = (BaseComponent)Activator.CreateInstance(componentType)!;

            component.InitializeComponent(webElement, parentContext, _browser, metadata, searchSettings);

            _contextEventHandlersByType?.AssignHandlers(component);


            return component;
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
}

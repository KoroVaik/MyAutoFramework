using AngleSharp.Dom;
using AspectInjector.Broker;
using Core.Services;
using Core.UI.SearchContext.Abstractions;
using Core.UI.SearchContext.Component;
using System.Reflection;

namespace Core.UI.SearchContext.Aspects
{
    //applicable in AbstractPage and BaseComponent
    [Aspect(Scope.Global)]
    public class InitializeComponentAspect
    {
        ComponentFactory _componentFactory;
        public InitializeComponentAspect()
        {
            _componentFactory = ScopedService.Get<ComponentFactory>();
        }

        [Advice(Kind.Around)]
        public object Around(
            [Argument(Source.Name)] string name,
            [Argument(Source.Arguments)] object[] args,
            [Argument(Source.Target)] Func<object[], object> target,
            [Argument(Source.Metadata)] MethodBase metadata,
            [Argument(Source.ReturnType)] Type returnType,
            [Argument(Source.Instance)] object instance
        )
        {
            var currentContext = instance as UiContext
                ?? throw new ArgumentException($"Instance type '{instance.GetType().Name}' is not of type '{nameof(UiContext)}'");

            var currentSearcher = (currentContext as ISearcher)
                ?? throw new ArgumentException($"Instance type '{instance.GetType().Name}' does not implement '{nameof(ISearcher)}'");


            var searchSettings = GetSearchSettings(metadata);
            var componentMetadata = new ComponentMetadata(name, ComponentType.Regular);

            if (typeof(BaseComponent).IsAssignableFrom(returnType))
            {
                var elemnet = currentSearcher.SearchComponent(searchSettings);

                var component = _componentFactory.InitializeComponent(returnType, elemnet, currentContext, componentMetadata, searchSettings);

                return component;
            }
            else if (typeof(List<BaseComponent>).IsAssignableFrom(returnType))
            {
                var elemnets = currentSearcher.SearchComponents(searchSettings).ToList();
                var components = _componentFactory.InitializeComponents(returnType, elemnets, currentContext, componentMetadata, searchSettings);
                return components;
            }
            else
                throw new NotSupportedException($"Return type '{returnType.Name}' is not supported for component property '{metadata.Name}'");
        }

        private SearchSettings GetSearchSettings(MethodBase method)
        {
            var attributes = GetComponentAttributes<IApplyToComponent<SearchSettings>>(method);
            var newSettings = new SearchSettings();

            foreach (var attr in attributes)
                attr.ApplyOn(newSettings);

            return newSettings;
        }

        private List<TAttributeType> GetComponentAttributes<TAttributeType>(MethodBase method)
        {
            var property = method?.DeclaringType?
                .GetProperties()
                .FirstOrDefault(p =>
                    p.GetGetMethod() == method ||
                    p.GetSetMethod() == method);

            var componentAttr_ClassLvl = method?.DeclaringType?.GetCustomAttributes()
                .OfType<TAttributeType>();

            var componentAttr_PropLvl = property?.GetCustomAttributes()
                .OfType<TAttributeType>(); 

            if (componentAttr_PropLvl?.Any() ?? false)
                return componentAttr_PropLvl.ToList();
            else if (componentAttr_ClassLvl?.Any() ?? false)
                return componentAttr_ClassLvl.ToList();
            else
                throw new Exception($"Component doesnt have any attr of type {nameof(TAttributeType)}.");
        }
    }

}

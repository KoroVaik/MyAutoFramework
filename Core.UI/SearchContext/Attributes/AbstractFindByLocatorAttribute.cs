using AspectInjector.Broker;
using Core.UI.SearchContext.Aspects;
using Core.UI.SearchContext.Component;
using OpenQA.Selenium;

namespace Core.UI.SearchContext.Attributes
{
    [Injection(typeof(InitializeComponentAspect), Inherited = true)]
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public abstract class AbstractFindByLocatorAttribute : Attribute, IApplyToComponent<SearchSettings>
    {
        internal By Locator { get; }

        public AbstractFindByLocatorAttribute(By locator)
        {
            Locator = locator;
        }

        public void ApplyOn(SearchSettings settings)
        {
            settings.Locator = Locator;
        }
    }

}

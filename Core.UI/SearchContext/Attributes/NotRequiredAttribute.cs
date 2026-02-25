using Core.UI.SearchContext.Component;

namespace Core.UI.SearchContext.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotRequiredAttribute : Attribute, IApplyToComponent<BaseComponent>
    {
        public void ApplyOn(BaseComponent componentToApply)
        {
            componentToApply.SearchSettings.IsRequired = false;
        }
    }

}

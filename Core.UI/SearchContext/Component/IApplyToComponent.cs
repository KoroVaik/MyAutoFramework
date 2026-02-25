namespace Core.UI.SearchContext.Component
{
    public interface IApplyToComponent<TModificator> where TModificator : class
    {
        public void ApplyOn(TModificator componentToApply);
    }

}

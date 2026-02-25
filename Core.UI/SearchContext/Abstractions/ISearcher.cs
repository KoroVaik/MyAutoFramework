using Core.UI.SearchContext.Component;

namespace Core.UI.SearchContext.Abstractions
{

    public interface ISearcher
    {
        public ElementWrapper SearchComponent(SearchSettings searchSettings);
        public IEnumerable<ElementWrapper> SearchComponents(SearchSettings searchSettings);
    }

}

using Core.UI.BaseComponents;
using Core.UI.BaseComponents.Interfaces;
using Core.UI.SearchContext.Attributes;
using Core.UI.SearchContext.Component;
using Core.UI.SearchContext.Pages;

namespace HisaPortalUI
{
    [PageMetaData("resoirce/some", "Search Page")]
    public class SearchPage : BasePage
    {
        [FindByCss("form[action='/search']")]
        public SearchForm SearchForm => new();
    }

    public class SearchForm : BaseComponent, IForm
    {
        [FindByCss("textarea")]
        public Input SearchInput => new();

        public string GetValue()
        {
            throw new NotImplementedException();
        }

        public void SetValue(string value)
        {
            SearchInput.TypeText(value);
        }

        public void Submit()
        {
            throw new NotImplementedException();
        }
    }
}

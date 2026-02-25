using Core.UI.WebDriverWrapper;

namespace Core.UI.SearchContext.Abstractions
{
    public interface IHasBrowser
    {
        public Browser Browser { get; set; }
    }
}

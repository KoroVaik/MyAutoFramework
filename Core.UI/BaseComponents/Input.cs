using AngleSharp.Dom;
using Core.UI.SearchContext.Abstractions;
using Core.UI.SearchContext.Component;

namespace Core.UI.BaseComponents
{
    public class Input : BaseComponent
    {
        [UiAction]
        public void TypeText(string text)
        {
            Element.TypeText(text);
        }
    }
}

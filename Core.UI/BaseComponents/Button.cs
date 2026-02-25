using AngleSharp.Dom;
using Core.UI.SearchContext.Abstractions;
using Core.UI.SearchContext.Component;

namespace Core.UI.BaseComponents
{
    public class Button : BaseComponent
    {
        [UiAction]
        public void Click()
        {
            Element.Click();
        }
    }
}

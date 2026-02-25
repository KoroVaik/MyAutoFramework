using OpenQA.Selenium;

namespace Core.UI.SearchContext.Attributes
{
    public class FindByCssAttribute : AbstractFindByLocatorAttribute
    {
        public FindByCssAttribute(string locator) : base(By.CssSelector(locator))
        { }
    }

}

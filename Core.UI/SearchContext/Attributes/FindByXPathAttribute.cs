using OpenQA.Selenium;

namespace Core.UI.SearchContext.Attributes
{
    public class FindByXPathAttribute : AbstractFindByLocatorAttribute
    {
        public FindByXPathAttribute(string locator) : base(By.XPath(locator))
        { }
    }

}

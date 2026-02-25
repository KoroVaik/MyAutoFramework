using OpenQA.Selenium;
using Serilog;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Core.UI.SearchContext.Component
{
    public class ElementWrapper
    {
        static readonly ILogger _logger = Log.ForContext<ElementWrapper>();
        const string ElementNotInitialized = "Element is not initialized";
        IWebElement? _webElement;

        public ElementWrapper(IWebElement? webElement)
        {
            _webElement = webElement;
        }

        internal IWebElement WebElement => _webElement 
            ?? throw new InvalidOperationException(ElementNotInitialized);

        public string TagName => WebElement.TagName;

        public virtual string Text => WebElement.Text;

        public virtual bool Enabled
        {
            get
            {
                try
                {
                    return WebElement.Enabled;
                }
                catch { return false; }
            }
        }

        public virtual bool Visible
        {
            get
            {
                try
                {
                    return WebElement.Displayed;
                }
                catch { return false; }
            }
        }

        public virtual bool Exists => _webElement != null;

        public virtual bool IsInteractable => Exists && Visible && Enabled;

        public bool Selected => WebElement.Selected;

        public Point Location => WebElement.Location;

        public Size Size => WebElement.Size;

        public bool Displayed => throw new NotImplementedException();
        
        public string? GetAttribute(string attributeName) => WebElement.GetAttribute(attributeName);

        public string GetCssValue(string propertyName) => WebElement.GetCssValue(propertyName);

        public string GetDomAttribute(string attributeName) => GetDomAttribute(attributeName);

        public string[] GetClasses() => GetAttribute("class")?.Split(" ") ?? new string[0];

        public ISearchContext GetShadowRoot() => WebElement.GetShadowRoot();

        public bool HasAttribute(string attributeName) => GetAttribute(attributeName) != null;

        public bool HasClass(string className) => GetClasses().Contains(className);

        public void Click() => WebElement.Click();
        public void TypeText(string text) => WebElement.SendKeys(text);

        public void Submit()
        {
            WebElement.Submit();
        }
    }
}

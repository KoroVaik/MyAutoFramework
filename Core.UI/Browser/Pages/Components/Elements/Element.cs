using Core.UI.Extensions;
using OpenQA.Selenium;
using Serilog;
using System.Drawing;

namespace Core.UI.Browser.Pages.Components.Elements
{
    public class Element : IElement, IElementInternal, IHasSearchPath
    {
        private static readonly ILogger _logger = Log.ForContext<Element>();

        private IBrowser _browser;
        private IWebElement _webElement;
        private By[] _searchPathFragments;

        protected IBrowser Browser => _browser;

        protected IWebElement WebElement => _webElement;

        protected By[] SearchPathFragments => _searchPathFragments;

        public string SearchPath => XPathHelper.ConvertByFragmentsToString(_searchPathFragments);

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

        public void Initialize(IBrowser browser, IWebElement webElement, IEnumerable<By> searchPathFragments)
        {
            _browser = browser;
            _webElement = webElement;
            _searchPathFragments = searchPathFragments.ToArray();
        }

        public virtual void ClearWithKeyboardActions()
        {
            var actions = _browser.BrowserActions;
            actions.MoveToElement(WebElement).Click().KeyDown(Keys.Control).SendKeys("a").KeyUp(Keys.Control).Release().Perform();

            actions.SendKeys(Keys.Backspace).Perform();
        }

        public virtual void Click(bool clickForce = false)
        {
            if (!clickForce) WaitForInteractable();
            try
            {
                ScrollIntoView();
                WebElement.Click(); 
            }
            catch
            {
                _logger.Error($"Can not click on element by path: {SearchPath}");
            }
        }

        public void ClickJs() => _browser.ExecuteScript("arguments[0].click();", WebElement);
        
        public string GetAttribute(string attributeName) => WebElement.GetAttribute(attributeName);

        public string GetCssValue(string propertyName) => WebElement.GetCssValue(propertyName);

        public string GetDomAttribute(string attributeName) => GetDomAttribute(attributeName);

        public string[] GetClasses() => GetAttribute("class")?.Split(" ") ?? new string[0];

        public ISearchContext GetShadowRoot() => WebElement.GetShadowRoot();

        public IWebElement GetWebElement() => WebElement ?? throw new NoSuchElementException( SearchPath);

        public bool HasAttribute(string attributeName) => GetAttribute(attributeName) != null;

        public bool HasClass(string className) => GetClasses().Contains(className);

        public void HoverMouse() => _browser.BrowserActions.MoveToElement(WebElement);

        public void ScrollIntoView()
        {
            _browser.ExecuteScript("arguments[0].scrollIntoViewIfNeeded(true);", WebElement);
        }

        public void SendKeys(string text, bool clearBeforeSend = true)
        {
            WaitForInteractable();
            if (clearBeforeSend) WebElement.Clear();
            WebElement.SendKeys(text);
        }

        public void SetAttribute(string attributeName, string value)
        {
            _browser.ExecuteScript($"arguments[0].setAttribute('{attributeName}', '{value}');");
        }

        public void Submit()
        {
            WebElement.Submit();
        }

        protected virtual void WaitForInteractable()
        {
            _browser.WaitFor(() => IsInteractable, 
                exceptionText: $"Element is not interactable: {SearchPath}");
        }
    }
}

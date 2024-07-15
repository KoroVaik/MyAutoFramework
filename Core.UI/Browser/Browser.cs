using Core.UI.Browser.Pages;
using Core.UI.Browser.Pages.Components.Elements;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Serilog;
using System.Collections.ObjectModel;
using Log = Serilog.Log;

namespace Core.UI.Browser
{
    public class Browser : IBrowser, IHasWebDriver
    {
        private static readonly ILogger _logger = Log.ForContext<Browser>();
        private static readonly string _screenshotPath = Path.Combine(Path.GetTempPath(), "Screenshots");
        private readonly string _baseUrl;

        public Browser(IWebDriver webDriver, string baseUrl)
        {
            WebDriver = webDriver;
            _baseUrl = baseUrl;
        }

        public IWebDriver WebDriver { get; }

        public BasePage CurrentPage { get; init; }

        public Actions BrowserActions => throw new NotImplementedException();

        public IDevTools DevTools => (IDevTools)WebDriver;

        public string CurrentUrl => WebDriver.Url;

        public string Title => WebDriver.Title;

        public Uri CurrentUri => new Uri(CurrentUrl);

        public ReadOnlyCollection<string> WindowHandles => WebDriver.WindowHandles;

        public string CurrentWindowHandle => WebDriver.CurrentWindowHandle;

        public void CloseWindow()
        {
            WebDriver.Close();
        }

        public void Dispose()
        {
            WebDriver.Dispose();
        }

        public void ExecuteScript(string script, params object[] args)
        {
            ((IJavaScriptExecutor)WebDriver).ExecuteScript(script, args);
        }

        public T ExecuteScript<T>(string script, params object[] args)
        {
           return (T)((IJavaScriptExecutor)WebDriver).ExecuteScript(script, args);
        }

        public Dictionary<string, string> GetCookies()
        {
            return WebDriver.Manage().Cookies.AllCookies
                .ToDictionary(item => item.Name, item => item.Value);
        }

        public void NavigateBack()
        {
            WebDriver.Navigate().Back();
        }

        public void NavigateTo(string url)
        {
            WebDriver.Navigate().GoToUrl(url);
        }

        public void OpenNewTab()
        {
            WebDriver.SwitchTo().NewWindow(WindowType.Tab);
        }

        public TPage OpenPage<TPage>() where TPage : BasePage
        {
            return OpenPage<TPage>(string.Empty);
        }

        public TPage OpenPage<TPage>(params string[] pageUrlParams) where TPage : BasePage
        {
            var page = GetPage<TPage>();
            var fullPageUrl = new Uri(
                new Uri(_baseUrl),  
                string.Format(page.Url, pageUrlParams));
            WebDriver.Navigate().GoToUrl(fullPageUrl.ToString());
            return page;
        }

        public TPage GetPage<TPage>() where TPage : BasePage
        {
            var page = Activator.CreateInstance<TPage>();
            page.Initialize(this);
            return page;
        }

        public void Quit()
        {
            WebDriver.Quit();
        }

        public void RefreshCurrentPage()
        {
            WebDriver.Navigate().Refresh();
        }

        public string SaveScreenshot(string fileName)
        {
            Directory.CreateDirectory(_screenshotPath);
            fileName = $"Screenshot-{DateTime.Now:yyyy-MM-dd-HH-mm-ss zzz}: {fileName}.png";
            var screenShotFilePath = Path.Combine(_screenshotPath, fileName);
            var screenshot = ((ITakesScreenshot)WebDriver).GetScreenshot();
            screenshot.SaveAsFile(screenShotFilePath);

            _logger.Information($"Screenshot is saved by path: {screenShotFilePath}");
            return screenShotFilePath;
        }

        public IAlert SwitchToAlert()
        {
            return WebDriver.SwitchTo().Alert();
        }

        public void SwitchToFirstWindow()
        {
            var handle = WindowHandles.First();
            WebDriver.SwitchTo().Window(handle);
        }

        public void SwitchToFrame(Element frame)
        {
            WebDriver.SwitchTo().Frame(frame.WebElement);
        }

        public void SwitchToFrame(string frame)
        {
            WebDriver.SwitchTo().Frame(frame);
        }

        public void SwitchToWindow(string handleNumber)
        {
            WebDriver.SwitchTo().Window(handleNumber);
        }

        public void SwitchToDefaultContent()
        {
            WebDriver.SwitchTo().DefaultContent();
        }

        public bool WaitForOrContinue(Func<bool> func, int timeout = 4000, int tickSize = 200)
        {
            try
            {
                WaitFor(func, timeout, tickSize);
                return true;
            }
            catch (WebDriverTimeoutException ex)
            {
                _logger.Warning(ex.Message);
                return false;
            }
        }

        public void WaitForAction(Action action, int timeout = 4000, int tickSize = 200, string? exceptionText = null)
        {
            bool ActionFunc()
            {
                try
                {
                    action();
                    return true;
                }
                catch
                {
                    throw;
                }
            }
            WaitFor(ActionFunc, timeout, tickSize, exceptionText);
        }

        public bool WaitFor(Func<bool> waitForFunc, int timeout = 4000, int tickSize = 200, string? exceptionText = null)
        {
            try
            {
                var wait = new WebDriverWait(WebDriver, TimeSpan.FromMilliseconds(timeout))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(tickSize),
                };
                wait.IgnoreExceptionTypes(
                    typeof(NoSuchElementException),
                    typeof(StaleElementReferenceException));
                wait.Until(_ => waitForFunc());

                return true;
            }
            catch (Exception e) when (e is WebDriverTimeoutException || e is TimeoutException)
            {
                exceptionText = exceptionText ?? $"Timed out after {timeout} ms.";
                e.Data.Add(nameof(TimeoutException), exceptionText);
                throw;
            }
        }
    }
}

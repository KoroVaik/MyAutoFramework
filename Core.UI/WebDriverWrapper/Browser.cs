using Core.UI.Drivers.Factory;
using Core.UI.SearchContext.Component;
using Core.UI.SearchContext.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Serilog;
using System.Collections.ObjectModel;
using Log = Serilog.Log;

namespace Core.UI.WebDriverWrapper
{
    public class Browser : IDisposable
    {
        private static readonly ILogger _logger = Log.ForContext<Browser>();
        private static readonly string _screenshotPath = Path.Combine(Path.GetTempPath(), "Screenshots");

        public Browser(WebDriverFactory webDriverFactory)
        {
            WebDriver = webDriverFactory.GetDriver();
        }

        public IWebDriver WebDriver { get; }

        public BasePage CurrentPage { get; set; } = null!;

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
            var jsExecutor = (IJavaScriptExecutor)WebDriver;
            var result = jsExecutor.ExecuteScript(script, args);
           return (T)result!;
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

        public void NavigateTo(Uri uri)
        {
            WebDriver.Navigate().GoToUrl(uri.ToString());
        }

        public void NavigateTo(string url)
        {
            WebDriver.Navigate().GoToUrl(url);
        }

        public void OpenNewTab()
        {
            WebDriver.SwitchTo().NewWindow(WindowType.Tab);
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

        public void SwitchToFrame(ElementWrapper frame)
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

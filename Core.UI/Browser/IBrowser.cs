using Core.UI.Browser.Pages;
using Core.UI.Browser.Pages.Components.Elements;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Interactions;
using System.Collections.ObjectModel;

namespace Core.UI.Browser
{
    public interface IBrowser : IWaitable, IDisposable
    {
        IPage CurrentPage { get; }

        Actions BrowserActions { get; }

        IDevTools DevTools { get; }

        string PageUrl { get; }

        string Title { get; }

        string PathAndQuery { get; }

        IPage OpenPage(Type pageType);

        TPage OpenPage<TPage>() where TPage : class, IPage;

        TPage OpenPage<TPage>(string url, params string[] pageUrlParams) where TPage : class, IPage;

        void NavigateTo(string url);

        void NavigateBack();

        void ScrollToBottom();

        Dictionary<string, string> GetCookies();

        void SwitchToFrame(IElement frame);

        void SwitchToFrame(string frame);

        ReadOnlyCollection<string> WaindowHandles { get; }

        string CurrentWindowHandle { get; }

        void SwitchToWindow(string handleNumber);

        void OpenNewTab();

        void SwitchToFirstWindow();

        void CloseWindow();

        IAlert SwitchToAlert();

        string SaveScreenshot(string fileName);

        void ExecuteScript(string script, params object[] args);

        T ExecuteScript<T>(string script, params object[] args);

        void RefreshCurrentPage();

        void Quit();
    }
}
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
        BasePage CurrentPage { get; }

        Actions BrowserActions { get; }

        IDevTools DevTools { get; }

        string CurrentUrl { get; }

        Uri CurrentUri { get; }

        string Title { get; }

        TPage OpenPage<TPage>() where TPage : BasePage;

        TPage OpenPage<TPage>(params string[] pageUrlParams) where TPage : BasePage;

        TPage GetPage<TPage>() where TPage : BasePage;

        void NavigateTo(string url);

        void NavigateBack();

        Dictionary<string, string> GetCookies();

        void SwitchToFrame(Element frame);

        void SwitchToFrame(string frame);

        void SwitchToDefaultContent();

        ReadOnlyCollection<string> WindowHandles { get; }

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
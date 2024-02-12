using Core.UI.Browser.Pages.Components;
using OpenQA.Selenium;

namespace Core.UI.Browser.Pages
{
    public abstract class BasePage : ISearchComponent
    {
        private IBrowser? _browser;

        private ComponentSearcher ComponentSearcher => new ComponentSearcher(_browser!);

        public abstract string Url { get; }

        public string Title => _browser!.CurrentPage.Title;

        public virtual void WaitForPageIsLoaded()
        {

        }

        public virtual bool Isloaded()
        {
            var documentState = _browser!.ExecuteScript<string>("return document.readyState;");
            return documentState == "complete";
        }

        public void CloseAlert(bool isConfirmed)
        {
            var alert = _browser!.SwitchToAlert();
            if (isConfirmed)
                alert.Accept();
            else
                alert.Dismiss();

            _browser.SwitchToDefaultContent();
        }

        public void Refresh()
        {
            _browser!.RefreshCurrentPage();
        }

        TComponent ISearchComponent.GetComponent<TComponent>(By by, int waitTimeout)
        {
            return ComponentSearcher.SearchComponent<TComponent>(by, waitTimeout);
        }

        IEnumerable<TComponent> ISearchComponent.GetComponents<TComponent>(By by, int waitTimeout)
        {
            return ComponentSearcher.SearchComponents<TComponent>(by, waitTimeout);
        }

        internal void Initialize(IBrowser browser)
        {
            _browser ??= browser;
        }
    }
}

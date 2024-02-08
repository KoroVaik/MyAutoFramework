using OpenQA.Selenium;
using System.Drawing;

namespace Core.UI.Browser.Pages.Components.Elements
{
    public interface IElement
    {
        string TagName { get; }

        string Text { get; }

        bool Enabled { get; }

        bool Visible { get; }

        bool Exists { get; }

        bool IsInteractable { get; }

        bool Selected { get; }

        Point Location { get; }

        Size Size { get; }

        void SendKeys(string text, bool clearBeforeSend = true);

        void ClearWithKeyboardActions();

        void Submit();

        void Click(bool clickForce = false);

        string GetAttribute(string attributeName);

        string GetDomAttribute(string attributeName);

        string GetCssValue(string propertyName);

        string[] GetClasses();

        ISearchContext GetShadowRoot();

        void ClickJs();

        void SetAttribute(string attributeName, string value);

        void HoverMouse();

        void ScrollIntoView();

        bool HasClass(string className);

        bool HasAttribute(string attributeName);
    }
}
using Allure.Net.Commons;
using Core.Services;
using Core.UI.Configurations;
using Core.UI.SearchContext;
using Core.UI.SearchContext.Abstractions;
using Core.UI.SearchContext.Component;
using Core.UI.SearchContext.Pages;
using Core.UI.WebDriverWrapper;
using static Core.UI.SearchContext.Pages.BasePage;

namespace HisaPortalUI;

public class UiSteps
{
    readonly InvokationContext _context = new();
    readonly PageFactory _pageFactory;
    readonly Browser _browser;
    readonly UiConfigurations _configurations;

    public UiSteps(Browser browser, UiConfigurations configurations, PageFactory pageFactory)
    {
        _browser = browser;
        _pageFactory = pageFactory;
        _configurations = configurations;

        _browser.NavigateTo(new Uri(_configurations.BaseUrl));
    }

    public SearchPage Login => GetPage<SearchPage>();

    public void ClearContext ()
    {
        _context.CloseContext();
    }

    public void CloseContext()
    {
        _context.CloseContext();
        _browser.Dispose();
    }

    protected TPage GetPage<TPage>() where TPage : BasePage
    {
        if (_context.CurrentPage is TPage page)
            return page;

        _context.CloseContext();

        var newPage = _pageFactory.GetPage<TPage>();
        _context.CurrentPage = newPage;

        return newPage;
    }
}

public class AllureTestContextController
{
    int counter = 0;
    public AllurePageObjectContext _currentPageContext = null!;

    public AllurePageObjectContext OpenNewPageContext(PageMetaData pageMetaData)
    {
        var context = new AllurePageObjectContext(pageMetaData);
        context.OpenPageContext();
        _currentPageContext = context;

        Console.WriteLine("Counter: " + ++counter);
        return context;
    }

    public void CloseCurrentPageContext()
    {
        _currentPageContext.ClosePageContext();
        Console.WriteLine("Counter: " + --counter);
    }

    public void OpenNewComponentContext(BaseComponent componentReference)
    {
        _currentPageContext.OpenSubComponentContext(componentReference);
        Console.WriteLine("Counter: " + ++counter);
    }

    public void CloseChildContextsForComponent(BaseComponent callerReference)
    {
        if (_currentPageContext.IsClosedContext)
            return;

        var targetComponentContext = _currentPageContext.FindSubComponent(callerReference);
        var closedContexts = targetComponentContext.CloseComponentContext();
        Console.WriteLine("Counter: " + --counter);
    }
}

public class AllurePageObjectContext : AllureComponentContext
{
    PageMetaData _pageMetaData;

    public AllurePageObjectContext(PageMetaData pageMetaData)
    {
        _pageMetaData = pageMetaData;
    }

    public void OpenPageContext()
    {
        ExtendedApi.StartStep($"Page lvl step");
        AllureContext = AllureLifecycle.Instance.Context;
        IsClosedContext = false;
    }

    public void ClosePageContext()
    {
        var stepName = "";
        if (AllureContext is not null && AllureContext.ContainerContextDepth + AllureContext.StepContextDepth > 1)
            stepName = $"Closing page lvl step";

        CloseComponentContext();
        IsClosedContext = true;
        CloseStep();
    }
}

public class AllureComponentContext
{
    public bool IsClosedContext = false;
    protected AllureContext? AllureContext;
    List<AllureComponentContext> _subContextsHistory = new();
    AllureComponentContext? _activeSubContext;
    BaseComponent _componentReference = null!;

    public void OpenSubComponentContext(BaseComponent componentReference)
    {
        if (_activeSubContext is null)
        {
            ExtendedApi.StartStep($"Component lvl step");
            _activeSubContext = new AllureComponentContext()
            {
                _componentReference = componentReference,
                AllureContext = AllureLifecycle.Instance.Context,
                IsClosedContext = true
            };
            _subContextsHistory.Add(_activeSubContext);
        }
        else
        {
            _activeSubContext.OpenSubComponentContext(componentReference);
        }
    }

    //child first
    public List<AllureComponentContext> CloseComponentContext()
    {
        if (IsClosedContext)
            return [];

        List<AllureComponentContext> closedComponents = new();

        if (_activeSubContext is null)
        {
            closedComponents.Add(this);
        }
        else
        {
             closedComponents = _activeSubContext.CloseComponentContext();
            closedComponents.Add(this);
            _activeSubContext = null;
        }

        CloseStep();
        IsClosedContext = true;

        return closedComponents;
    }

    public AllureComponentContext FindSubComponent(BaseComponent componentReference)
    {
        if (_componentReference == componentReference)
        {
            return this;
        }

        return _activeSubContext?.FindSubComponent(componentReference)
            ?? throw new Exception($"Component not found {componentReference.Metadata.Name}");
    }

    protected void CloseStep()
    {
        //if (AllureContext != AllureLifecycle.Instance.Context)
        //    throw new Exception($"Allure context mismatch");

        ExtendedApi.PassStep();
    }
}

public class AllurePageContextEventHandler : ContextEventHandlers
{
    AllureTestContextController _allureTestContext;

    public AllurePageContextEventHandler(AllureTestContextController allureTestContext)
    {
        _allureTestContext = allureTestContext;
    }

    public override void OnContextOpened(UiContext context, ContextEventArgs s)
    {
        _allureTestContext.OpenNewPageContext(((BasePage)context)!.MetaData);
    }

    public override void OnContextClosed(UiContext context, ContextEventArgs s)
    {
        _allureTestContext.CloseCurrentPageContext();
    }
    public override void OnBeforeAction(UiContext context, ActionEventArgs s)
    {
        ExtendedApi.StartStep(s.ActionName);
    }
    public override void OnAfterAction(UiContext context, ActionEventArgs s)
    {
        ExtendedApi.PassStep();
    }
}


public class AllureComponentEventHandlers : ContextEventHandlers
{
    AllureTestContextController _allureTestContext;

    public AllureComponentEventHandlers(AllureTestContextController allureTestContext)
    {
        _allureTestContext = allureTestContext;
    }

    public override void OnContextOpened(UiContext context, ContextEventArgs s)
    {
        _allureTestContext.OpenNewComponentContext((BaseComponent)context);
    }

    public override void OnContextClosed(UiContext context, ContextEventArgs s)
    {
        _allureTestContext.CloseChildContextsForComponent((BaseComponent)context);
    }
    public override void OnBeforeAction(UiContext context, ActionEventArgs s)
    {
        ExtendedApi.StartStep(s.ActionName);
    }
    public override void OnAfterAction(UiContext context, ActionEventArgs s)
    {
        ExtendedApi.PassStep();
    }
}
using Allure.Net.Commons;
using Core;
using Core.Services;
using Core.TestDataBuilder;
using Core.UI.Configurations;
using Core.UI.Drivers.Factory;
using Core.UI.SearchContext;
using Core.UI.SearchContext.Abstractions;
using Core.UI.SearchContext.Component;
using Core.UI.SearchContext.Pages;
using Core.UI.WebDriverWrapper;
using HisaPortalUI;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace HisaPortalTests;

public class BaseTest : AbstractTest
{
    protected TestDataProvider TestDataProvider => ScopedService.Get<TestDataProvider>();
    protected UiSteps Steps => ScopedService.Get<UiSteps>();
    private AllureTestContextController _allureTestContext = new();

    protected void TestCaseStep(string step, Action action)
    {
        Steps.ClearContext();

        AllureApi.Step(step, () =>
        {
            action();
        });
    }

    [SetUp]
    public void SetUpBaseTest()
    {
        ScopedService.Register<UiConfigurationManager>();
        ScopedService.Register<UiConfigurations>(serviceProvider => serviceProvider.GetService<UiConfigurationManager>()!.CurrentConfigs);
        ScopedService.Register<TestContext>(TestContext.CurrentContext);
        ScopedService.Register<TestDataProvider>();

        var allureHandlers = new CustomContextEventHandlers()
        {
            [typeof(BaseComponent)] = new AllureComponentEventHandlers(_allureTestContext),
            [typeof(BasePage)] = new AllurePageContextEventHandler(_allureTestContext)
        };
        ScopedService.Register<CustomContextEventHandlers>(allureHandlers);
        ScopedService.Register<ComponentFactory>();
        ScopedService.Register<PageFactory>();

        ScopedService.Register<WebDriverFactory>();
        ScopedService.Register<Browser>();
        ScopedService.Register<UiSteps>();
    }

    [TearDown]
    public void TearDownBaseTest()
    {
        Steps.CloseContext();
    }
}

using Allure.NUnit;
using Core.Services;
using NUnit.Framework;

namespace Core;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[AllureNUnit]
public class AbstractTest
{
    [TearDown]
    public void TearDown()
    {
        ScopedService.ResetProvider();
    }
}
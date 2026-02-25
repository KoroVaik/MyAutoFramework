using Allure.Net.Commons;
using HisaPortalUI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisaPortalTests.Suites.Smoke
{
    public class LoginTests : BaseTest
    {
        [Test]
        public void SampleLoginTest()
        {
            TestCaseStep("Step1", () => {
                Steps.Login.SearchForm.SetValue("Test Search ");
                Steps.Login.SearchForm.SetValue("Test Search2 ");

                TestCaseStep("Step1.22", () =>
                {
                    Steps.Login.SearchForm.SetValue("Test Search2212 ");
                    Steps.Login.SearchForm.SetValue("Test Search2212 ");
                });
            } );
            TestCaseStep("Step2", () => {
                Steps.Login.SearchForm.SetValue("Test Search2212");
            });


            var a = TestDataProvider.Get<User>("UserForLogin");
            var a2 = TestDataProvider.Get<User>("User1");
            var h = TestDataProvider.Get<Horse>("Horse1");
            var h2 = TestDataProvider.Get<Horse>("HorseForLogin");
            var n = TestDataProvider.RawJson;
            Assert.Pass("This is a placeholder test.");
        }
    }

    public class User
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Type { get; set; } = null!;
    }

    public class Horse
    {
        public string HorseName { get; set; } = null!;
        public string HorseType { get; set; } = null!;
    }
}

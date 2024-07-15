using WebDriverManager;
using WebDriverManager.DriverConfigs;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace Core.UI.Browser.Settings.WebDriver
{
    public static class DriverBinaryManager
    {
        public static string GetChromeBinaryDirectory() => GetBinaryDirectory(new ChromeConfig());

        public static string GetBinaryDirectory(IDriverConfig driverConfig)
        {
            string binaryPath;
            try
            {
                binaryPath = new DriverManager().SetUpDriver(driverConfig, VersionResolveStrategy.MatchingBrowser);
            }
            catch
            {
                binaryPath = new DriverManager().SetUpDriver(driverConfig);
            }

            return Path.GetDirectoryName(binaryPath)!;
        }
    }
}

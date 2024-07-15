using OpenQA.Selenium;

namespace Core.UI.Extensions
{
    internal static class ByHelper
    {
        internal static string ConvertByFragmentsToString(IEnumerable<By> fragments)
        {
            return $"\n{fragments.First()}     <----Root\n" + string.Join("\n", fragments.Skip(1)) + "    <----Child";
        }
    }
}

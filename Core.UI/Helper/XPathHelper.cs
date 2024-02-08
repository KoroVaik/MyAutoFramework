using OpenQA.Selenium;

namespace Core.UI.Extensions
{
    internal static class XPathHelper
    {
        internal static string ConvertByFragmentsToString(IEnumerable<By> fragments)
        {
            return $"\n{fragments.First()}     <----Root" + string.Join("\n", fragments.Skip(1)) + "    <----Child";
        }
    }
}

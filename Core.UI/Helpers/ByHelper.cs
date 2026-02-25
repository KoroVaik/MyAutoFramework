using OpenQA.Selenium;

namespace Core.UI.Extensions
{
    internal static class ByHelper
    {
        internal static string ConvertByFragmentsToString(IEnumerable<By> fragments)
        {
            if (!fragments.Any())
            {
                return "<Empty Search Path>";
            }
            else if (fragments.Count() == 1)
            {
                return $"{fragments.First()}   <----Root/Child";
            }
            else
            {
                return $"\n{fragments.First()}   <----Root\n" + string.Join("\n", fragments.Skip(1)) + "   <----Child";
            }
        }
    }
}

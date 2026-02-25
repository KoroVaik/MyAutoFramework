namespace Core.Helper
{
    public static class ComparisonHandler
    {
        public static Func<string, string, bool> GetStringHandler(StringCompareStrategy strategy)
        {
            return strategy switch
            {
                StringCompareStrategy.Equals => (string actual, string expected) => actual.Equals(expected),
                StringCompareStrategy.Contains => (string actual, string expected) => actual.Contains(expected),
                StringCompareStrategy.EndsWith => (string actual, string expected) => actual.EndsWith(expected),
                StringCompareStrategy.StartsWith => (string actual, string expected) => actual.StartsWith(expected),
                _ => throw new NotImplementedException()
            };
        }
    }
}

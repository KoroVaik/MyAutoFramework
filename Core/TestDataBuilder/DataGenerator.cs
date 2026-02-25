using System.Text;

namespace Core.TestDataBuilder
{
    public class DataGenerator
    {
        const string _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string EmailGenerator(string domain = "gmail.com")
        {
            return $"hisa.test.person+{new Random().Next(100000, 100000000)}@{domain}";
        }

        public static double LatLngGenerator(int min, int max)
        {
            var testNumber = new Random().NextDouble() * (max - min) + min;
            return Math.Round(testNumber, 7);
        }

        public static string StringGenerator(int stringLength)
        {
            var randomString = StringGenerator();
            return string.Join("", Enumerable.Repeat(randomString, stringLength / randomString.Length + 1)).Substring(0, stringLength); ;
        }

        public static string StringGenerator()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 25);
        }

        public static string WordGenerator()
        {
            Random random = new Random();
            StringBuilder result = new StringBuilder(10);
            for (int i = 0; i < 10; i++)
            {
                int index = random.Next(_chars.Length);
                result.Append(_chars[index]);
            }

            return result.ToString();
        }

        public static int IntGenerator(int max = int.MaxValue)
        {
            return new Random().Next(max);
        }

        public static DateOnly GenerateDate(TimeSpan? timeSpanTemp = null)
        {
            TimeSpan timeSpan = timeSpanTemp ?? TimeSpan.FromDays(365.25); //-1 years

            DateTime currentDateTime = DateTime.Now;

            DateTime lowerLimit = currentDateTime.Add(-timeSpan);

            Random random = new Random();
            long ticks = random.NextInt64(lowerLimit.Ticks, currentDateTime.Ticks);

            return DateOnly.FromDateTime(new DateTime(ticks));
        }

        public static DateOnly GenerateDate(int maxYears, int minYears = 18)
        {
            DateTime minDateTime = DateTime.Now.AddYears(-maxYears);

            DateTime maxDateTime = DateTime.Now.AddYears(-minYears);

            Random random = new Random();
            long ticks = random.NextInt64(minDateTime.Ticks, maxDateTime.Ticks);

            return DateOnly.FromDateTime(new DateTime(ticks));
        }
    }
}

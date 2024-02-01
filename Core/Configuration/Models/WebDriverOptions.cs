using Core.Browser;

namespace Core.Configuration.Models
{
    public class WebDriverOptions
    {
        public bool isHeadless { get; set; }

        public int ScreenWidth { get; set; }

        public int ScreenHaight { get; set; }

        public float ScaleFactor { get; set; }

        public BrowserType Browser { get; set; }

        public List<string> Arguments { get; set;}

        //Key=Value
        public List<string> ProfilePreferences { get; set;}
    }
}

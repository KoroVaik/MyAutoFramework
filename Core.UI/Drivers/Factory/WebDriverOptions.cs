using System.ComponentModel.DataAnnotations;

namespace Core.UI.Drivers.Factory
{
    public class WebDriverOptions
    {
        [Required] public bool IsHeadless { get; set; }

        public int? ScreenWidth { get; set; }

        public int? ScreenHeight { get; set; }

        public float? ScaleFactor { get; set; }

        [Required] public WebDriverType WebDriverType { get; set; }

        public List<string>? Arguments { get; set; }

        //Key=Value
        public List<string>? ProfilePreferences { get; set; }
    }
}
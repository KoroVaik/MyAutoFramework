using Core.Configuration.Models;
using Core.UI.Drivers.Factory;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.UI.Configurations
{
    public class UiConfigurations : BaseTestConfigurations
    {
        [Required] 
        public string BaseUrl { get; set; } = null!;

        [Required] 
        public WebDriverType WebDriverType { get; set; }

        [Required] 
        public List<WebDriverOptions> DefaultWebDriversOptions { get; init; } = null!;
    }
}

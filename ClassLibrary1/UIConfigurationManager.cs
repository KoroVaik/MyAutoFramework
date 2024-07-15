using Core.Configuration;
using Core.UI.Configurations;

namespace Core.UI.Browser.Configurations
{
    public class UIConfigurationManager : ConfigurationManager<UIConfigurations>
    {
        public override UIConfigurations? Current { get; protected set; }
    }
}

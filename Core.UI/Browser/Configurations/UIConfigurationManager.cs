using Core.Configuration;

namespace Core.UI.Browser.Configurations
{
    public class UIConfigurationManager : ConfigurationManager<UIConfigurations>
    {
        public override UIConfigurations CurrentConfigurations { get; protected set; }
    }
}

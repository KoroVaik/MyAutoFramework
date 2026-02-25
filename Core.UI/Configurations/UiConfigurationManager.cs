using Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UI.Configurations
{
    public class UiConfigurationManager : ConfigurationManager<UiConfigurations>
    {
        public UiConfigurationManager()
        {
            AddJsonAsSource("webDriverOptions.json");
        }
    }
}

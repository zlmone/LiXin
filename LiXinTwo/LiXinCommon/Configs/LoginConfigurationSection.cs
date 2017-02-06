using System.Configuration;

namespace LiXinCommon.Configs
{
    public class LoginConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("Configurations")]
        public LoginConfigurationElement LoginConfigurations
        {
            get { return (LoginConfigurationElement) base["Configurations"]; }
            set { base["Configurations"] = value; }
        }
    }
}
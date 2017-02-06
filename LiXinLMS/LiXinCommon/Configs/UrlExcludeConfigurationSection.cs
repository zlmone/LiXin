using System.Configuration;

namespace LiXinCommon.Configs
{
    /// <summary>
    ///     权限管理系统中，要排除的Url配置项
    /// </summary>
    public class UrlExcludeConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("ExcludeUrls")]
        public RetechUrlConfigurationElementCollection ExcludeUrls
        {
            get { return (RetechUrlConfigurationElementCollection) base["ExcludeUrls"]; }
            set { base["ExcludeUrls"] = value; }
        }
    }

    public class RetechUrlConfigurationElement : ConfigurationElement
    {
        // Methods
        public RetechUrlConfigurationElement()
        {
        }

        public RetechUrlConfigurationElement(string url)
        {
            Url = url;
        }


        [ConfigurationProperty("Url", IsRequired = true)]
        public string Url
        {
            get { return (string) base["Url"]; }
            set { base["Url"] = value; }
        }
    }

    public class RetechUrlConfigurationElementCollection : ConfigurationElementCollection
    {
        // Methods
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        protected new RetechUrlConfigurationElement BaseGet(int index)
        {
            return (RetechUrlConfigurationElement) base.BaseGet(index);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RetechUrlConfigurationElement();
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new RetechUrlConfigurationElement(elementName);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RetechUrlConfigurationElement) element).Url;
        }

        // Properties
    }
}
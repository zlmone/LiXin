using System.Configuration;

namespace LiXinCommon.Configs
{
    public class LoginConfigurationElement : ConfigurationElement
    {
        /// <summary>
        ///     ��������������
        /// </summary>
        [ConfigurationProperty("allowFailurePasswordCount", IsRequired = true)]
        public int AllowFailurePasswordCount
        {
            get { return (int) base["allowFailurePasswordCount"]; }
            set { base["allowFailurePasswordCount"] = value; }
        }

        /// <summary>
        ///     �Ƿ���ʾ��¼��֤��
        /// </summary>
        [ConfigurationProperty("isShowLoginVaildateCode", IsRequired = true)]
        public bool IsShowLoginVaildateCode
        {
            get { return (bool) base["isShowLoginVaildateCode"]; }
            set { base["isShowLoginVaildateCode"] = value; }
        }

        [ConfigurationProperty("SuperAdmin", IsRequired = true)]
        public string SuperAdmin
        {
            get { return (string) base["SuperAdmin"]; }
            set { base["SuperAdmin"] = value; }
        }

        [ConfigurationProperty("SuperPassword", IsRequired = true)]
        public string SuperPassword
        {
            get { return (string) base["SuperPassword"]; }
            set { base["SuperPassword"] = value; }
        }
    }
}
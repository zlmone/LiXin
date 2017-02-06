using System;
using System.Configuration;
using DotNet.Common.FileConvert.ConfigSection;

namespace DotNet.Common.FileConvert
{
    public class BaseConfig
    {
        /// <summary>
        /// 读取配置文件
        /// </summary>
        protected static readonly FileConvertConfigSection ConfigSection =
            (FileConvertConfigSection)ConfigurationManager.GetSection("DotNet.Common.FileConvert");

        /// <summary>
        /// 转换超时时间
        /// </summary>
        protected static int TimeOut
        {
            get
            {
                var time = 0;
                if (ConfigSection != null)
                    time = Convert.ToInt32(ConfigSection.KeyValues["timeOut"].Value);
                return time == 0 ? 1000 * 60 * 30 : time;
            }
        }
    }
}
